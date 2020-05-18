using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Classe che gestisce il gioco nella sua interezza
public class GameManager : MonoBehaviour
{
    private GeneratoreCarte refGeneratore;
    private GameObject cartaCliccata_1 = null, cartaCliccata_2 = null; // Variabili per memorizzare le carte che sono state cliccate
    private int contatoreClick = 2; // Contatore che memorizza il numero di click disponibili
    private bool abilitaClick = true, distruggiCheck = false; // Check che permettono di abilitare il click delle carte e il suono del flip
    private GameObject[] carteCliccate = new GameObject[2]; // Variabile di appoggio da passare alla Coroutine che verificherà se esse sono uguali
    private int vite = 2, combo = 1; // Variabili che contengono le vite e la combo  attuali
    private Text testoPunteggio; // Variabile che mi serve per assegnare il valore del punteggio al testo "In-Game"
    private Image[] cuori = new Image[3]; // Variabili per le immaggini dei cuori
    private int carteRimanenti = 16; // Contatore della carte rimanenti da accoppiare prima del termine della partita
    private AudioManager refAudioManager; // Prendiamo la referenza all'audio manager per poter lanciare i suoni
    private ComboManager refComboManager; // Prendiamo la referenza del combo manager per poter visualizzare le combo

    // Start is called before the first frame update
    void Start()
    {
        refGeneratore = GameObject.FindGameObjectWithTag("GeneratoreCarte").GetComponent<GeneratoreCarte>(); // Prendiamo la referenza del generatore di carte
        GameObject[,] mazzoCarte = refGeneratore.getMazzoCarte(); // Recuperiamo il mazzo di carte generato dal generatore
        StartCoroutine("GiraCarte", mazzoCarte); // Lanciamo la Coroutine che gira le carte dopo 2 secondi 
        testoPunteggio = GameObject.FindGameObjectWithTag("Punteggio").GetComponent<Text>(); // Prendiamo la referenza al punteggio in game per poi cambiarlo al momento giusto 
        cuori = GameObject.FindGameObjectWithTag("Cuore").GetComponentsInChildren<Image>(); // Recuperiamo i cuori
        SharedVariables.punteggio = 0; // Azzeriamo il punteggio ad inizio partita visto che potrebbe essere ancora presente quello della partita precedente
        refAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); // Prende l'oggetto con il tag AudioManager in modo da averlo disponibile in ogni scena visto che il GameManager è sempre presente
        refComboManager = GameObject.FindGameObjectWithTag("ComboManager").GetComponent<ComboManager>(); // Prende l'oggetto con il tag ComboManager
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Metodo per sottrarre un click disponibile e verificare se il contatore è a 0 per poi lanciare il metodo che verifica se le carte cliccate sono uguali
    public void SottraiClick(GameObject cartaCliccata)
    {
        if (abilitaClick == true && contatoreClick > 0) // Se l'utente può cliccare ed ha ancora click disponibili allora pparte il metodo
        {
            if (cartaCliccata_1 != cartaCliccata) { // Questo if serve per la seconda carta cliccata: se essa è proprio la prima carta cliccata, allora non diminuisce i click disponibili
                contatoreClick -= 1;
            }
            if (contatoreClick == 1 && cartaCliccata_1 != cartaCliccata) // Quando viene cliccata la prima carta, la gira per mostrarla
            {
                cartaCliccata_1 = cartaCliccata;
                StartCoroutine("AnimazioneCarta", cartaCliccata_1);  // Coroutine che gestisce l'animazione del flip della carta 1
                cartaCliccata_1.GetComponentsInChildren<ParticleSystem>()[0].Play(); // Lancia l'effetto particellare del click sulla carta 1
            }
            else if (contatoreClick == 0) // Quando viene cliccata la seconda carta che non sia la stessa identica prima carta cliccata si entra nell'else
            {
                abilitaClick = false; // Disabilita i click fino a fine controllo
                cartaCliccata_2 = cartaCliccata;
                StartCoroutine("AnimazioneCarta", cartaCliccata_2); // Coroutine che gestisce l'animazione del flip della carta 2
                cartaCliccata_2.GetComponentsInChildren<ParticleSystem>()[0].Play(); // Lancia l'animazione del click sulla carta 2
                CheckNomi(); // Parte il metodo per il controllo delle carte cliccate
            }
        }
    }

    // Metodo che controlla se le carte cliccate sono identiche per poi andarle a distruggere
    private void CheckNomi()
    {
        carteCliccate = new GameObject[] { cartaCliccata_1, cartaCliccata_2 };

        if (cartaCliccata_1.GetComponentsInChildren<SpriteRenderer>()[1].sprite.name.Equals(cartaCliccata_2.GetComponentsInChildren<SpriteRenderer>()[1].sprite.name))
        { // Se sono uguali disattivale
            StartCoroutine("GestisciCoppiaCarte", "distruggi"); // Parte  la coroutine che aspetta .5 secondi e distrugge le carte
        }
        else
        { // Altrimenti reset carte 
            StartCoroutine("GestisciCoppiaCarte", "reset"); // Parte  la coroutine che aspetta .5 secondi e rigira le carte
        }
        cartaCliccata_1 = null; // Azzera la prima carta così può riniziare il controllo della prossima coppia di carte cliccate
    }

    // Coroutine che abilita il retro di tutte le carte del mazzo e lancia l'animazione del flip ad inizio partita dopo 2 secondi
    private IEnumerator GiraCarte(GameObject[,] mazzoCarte)
    {
        Cursor.lockState = CursorLockMode.Locked; // Blocca il cursore dell'utente
        yield return new WaitForSeconds(2);
        foreach (GameObject carta in mazzoCarte)
        {
            carta.GetComponentsInChildren<SpriteRenderer>()[0].enabled = true; // Abilitiamo il retro di ogni carta
            carta.GetComponentsInChildren<Animator>()[0].enabled = true; // Lanciamo l'animazione del retro della carta
            carta.GetComponentsInChildren<Animator>()[1].enabled = true; // Lanciamo l'animazione del fronte della carta
        }
        Cursor.lockState = CursorLockMode.Confined; // Riabilita il cursore dell'utente
    }

    // Coroutine che si occupa di distruggere le carte se uguali e di rigirarle se diverse
    private IEnumerator GestisciCoppiaCarte(string azione)
    {
        distruggiCheck = true; // Indica che il suono delle carte non può partire mentre si stanno confrontanto due carte
        if (azione.Equals("distruggi")) {
            
            yield return new WaitForSeconds(1); // Aspetta un secondo
            carteCliccate[0].GetComponentsInChildren<ParticleSystem>()[1].Play(); // Lancia l'effetto particellare della distruzione sulla carta 1
            carteCliccate[1].GetComponentsInChildren<ParticleSystem>()[1].Play(); // Lancia l'effetto particellare della distruzione sulla carta 2
            yield return new WaitForSeconds(1); // Aspetta un secondo
            refAudioManager.GetCardDestroy().Play(); // Lancio l'audio della distruzione delle carte
            carteCliccate[0].SetActive(false); // Distruggi Carta 1
            carteCliccate[1].SetActive(false); // Distruggi Carta 2
            SharedVariables.punteggio += 100 * combo; // Aumenta il punteggio
            testoPunteggio.text = SharedVariables.punteggio.ToString(); // Assegna il testo del punteggio "In-Game"
            StartCoroutine("MostraCombo"); // Lanciamo la coroutine che mostra la combo attuale
            combo++; // Aumenta la combo attuale
            carteRimanenti -= 2; // Diminuisce le carte rimanenti per la vittoria
            if (carteRimanenti == 0) // Se non rimangono più carte, la partita termina con successo
            {
                TerminaPartita("vittoria");
            }
        }
        else if (azione.Equals("reset"))
        {
            // NOTA: animazioni di fronte e retro devono partire in differita altrimenti si sovrappongono
            // NOTA2: servono due variabili booleane per ogni animazione, perciò abbiamo cliccata e comboErrata per la prima mentre cliccataFronte e comboErrataFronte per la seconda.
            // Il motivo è che altrimenti nell'albero delle animazioni si avrebbe il loop dell'animazione, mentre gestendo le variabili manualmente non ho problemi di sorta
            yield return new WaitForSeconds(1); // Aspetta un secondo
            carteCliccate[0].GetComponentsInChildren<Animator>()[1].SetBool("cliccataFronte", false); // Fa partire l'animazione del fronte della carta 1
            carteCliccate[1].GetComponentsInChildren<Animator>()[1].SetBool("cliccataFronte", false); // Fa partire l'animazione del fronte della carta 2
            yield return new WaitForSeconds(0.3f); // Aspetta 0.3 secondi
            carteCliccate[0].GetComponentsInChildren<Animator>()[0].SetBool("cliccata", false); // Fa partire l'animazione del retro della carta 1
            carteCliccate[1].GetComponentsInChildren<Animator>()[0].SetBool("cliccata", false); // Fa partire l'animazione del restro della carta 2
            // Segno che la coppia è errata con le prossime 4 istruzioni, permettendo la rotazione delle carte
            carteCliccate[0].GetComponentsInChildren<Animator>()[0].SetBool("comboErrata", true);
            carteCliccate[1].GetComponentsInChildren<Animator>()[0].SetBool("comboErrata", true);
            carteCliccate[0].GetComponentsInChildren<Animator>()[1].SetBool("comboErrataFronte", true);
            carteCliccate[1].GetComponentsInChildren<Animator>()[1].SetBool("comboErrataFronte", true);
            carteCliccate[0].GetComponent<ScriptCarta>().SetCliccata(); // Indica che la prima carta non è più cliccata
            carteCliccate[1].GetComponent<ScriptCarta>().SetCliccata(); // Indica che la seconda carta non è più cliccata
            combo = 1; // Resetta le combo
            StartCoroutine("DistruggiCuore", cuori[vite]); // Rimuove uno dei cuori disponibili
            vite -= 1; // Diminuisce le vite 
            if (vite == -1) // Se non ci sono più vite termina la partita con una sconfitta
            {
                yield return new WaitForSeconds(1); // Aspetta un secondo
                TerminaPartita("sconfitta");
            }
            yield return new WaitForSeconds(1); // Aspetta un secondo
            // Con le prossime 4 istruzioni rimetto le variabili per la comboErrata a false così nell'albero delle animazioni si attende che una nuova coppia sia cliccata prima di ripartire con le animazioni per farle tornare in posizione se errate
            carteCliccate[0].GetComponentsInChildren<Animator>()[0].SetBool("comboErrata", false);
            carteCliccate[1].GetComponentsInChildren<Animator>()[0].SetBool("comboErrata", false);
            carteCliccate[0].GetComponentsInChildren<Animator>()[1].SetBool("comboErrataFronte", false);
            carteCliccate[1].GetComponentsInChildren<Animator>()[1].SetBool("comboErrataFronte", false);
        }
        distruggiCheck = false; // Riattiviamo il suono del flip delle carte
        abilitaClick = true; // Abilita click
        contatoreClick = 2; // Reset contatore
    }

    // Metodo che si occupa di terminare la partita
    private void TerminaPartita(string finePartita)
    {
        if (finePartita.Equals("vittoria"))
        {
            SharedVariables.messaggio = "HAI VINTO!!!";
        } else
        {
            SharedVariables.messaggio = "HAI PERSO...";
        }
        SceneManager.LoadScene("FinePartita", LoadSceneMode.Single); // Cambio di scena che mostra il risultato della partita, 
    }

    // Coroutine che mostra l'animazione di distruzione del cuore
    IEnumerator DistruggiCuore(Image cuore)
    {
        cuore.GetComponent<Animator>().enabled = true; // Lancia l'animazione
        yield return new WaitForSeconds(1); // Aspetta 1 secondo
        Destroy(cuore.gameObject); // Distrugge l'oggetto
    }

    // Coroutine che mostra l'animazione della combo attuale
    IEnumerator MostraCombo()
    {
        refComboManager.GetComponentInChildren<SpriteRenderer>().sprite = refComboManager.GetSpritesCombo(combo); // Prendiamo la sprite corretta da visualizzare in base alla combo
        refComboManager.GetComponentInChildren<Animator>().enabled = true; // Lanciamo l'animazione che la mostra
        yield return new WaitForSeconds(1); // Aspetta 1 secondo
        refComboManager.GetComponentInChildren<Animator>().enabled = false; // Disattiviamo l'animazione in modo da poterla lanciare nuovamente la prossima volta
    }

    // Coroutine che mostra l'animazione della combo attuale
    IEnumerator AnimazioneCarta(GameObject cartaCliccata)
    {
        cartaCliccata.GetComponentsInChildren<Animator>()[0].SetBool("cliccata", true); // Lancia l'animazione del retro della carta passata come parametro
        yield return new WaitForSeconds(0.3f); // Aspetta 0.3 secondi
        cartaCliccata.GetComponentsInChildren<Animator>()[1].SetBool("cliccataFronte", true); // Lancia l'animazione del fronte della carta passata come parametro
    }

    // Getter dell'Audio Manager
    public AudioManager GetAudioManager()
    {
        return refAudioManager;
    }

    // Getter di Distruggi Check
    public bool GetDistruggiCheck()
    {
        return distruggiCheck;
    }
}