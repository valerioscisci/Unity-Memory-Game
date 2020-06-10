using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Classe che gestisce il gioco nella sua interezza
public class GameManager : MonoBehaviour
{
    private GeneratoreCarte refGeneratore; // Prendiamo la referenza del Generatore carte
    private AudioManager refAudioManager; // Prendiamo la referenza dell'audio manager per poter lanciare i suoni
    private ViewManager refViewManager; // Prendiamo la referenza del view manager per poter gestire i cambiamente grafici
    private LevelManager refLevelManager; // Prendiamo la referenza del level manager per poter gestire il cambio livello
    private GameObject cartaCliccata_1 = null, cartaCliccata_2 = null; // Variabili per memorizzare le carte che sono state cliccate
    private GameObject[] carteCliccate = new GameObject[2]; // Variabile di appoggio da passare alla Coroutine che verificherà se esse sono uguali
    private int contatoreClick = 2; // Contatore che memorizza il numero di click disponibili
    private int vite = 2, combo = 1; // Variabili che contengono le vite e la combo  attuali
    private int carteRimanenti = 16; // Contatore della carte rimanenti da accoppiare prima del termine della partita
    private bool abilitaClick = true, distruggiCheck = false; // Check che permettono di abilitare il click delle carte e il suono del flip
    static public string messaggio = ""; // Variabile per contenere il messaggio a fine partita
    static public int punteggio = 0; // Messaggio per contenere il punteggio accumulato in una partita

    // Start is called before the first frame update
    void Start()
    {
        refGeneratore = GameObject.FindGameObjectWithTag("GeneratoreCarte").GetComponent<GeneratoreCarte>(); // Prendiamo la referenza del generatore di carte
        refViewManager = GameObject.FindGameObjectWithTag("ViewManager").GetComponent<ViewManager>(); // Prende l'oggetto con il tag ComboManager
        refAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); // Prende l'oggetto con il tag AudioManager in modo da averlo disponibile in ogni scena visto che il GameManager è sempre presente
        refLevelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>(); // Prende l'oggetto con il tag LevelManager
        GameObject[,] mazzoCarte = refGeneratore.getMazzoCarte(); // Recuperiamo il mazzo di carte generato dal generatore
        refViewManager.GiraCarteMazzo(mazzoCarte); // Lancia il metodo che gira le carte a inizio partita
        punteggio = 0; // Azzeriamo il punteggio ad inizio partita visto che potrebbe essere ancora presente quello della partita precedente
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
                refViewManager.ParticelleClickCarta(cartaCliccata_1); // Lancia l'effetto particellare della carta cliccata
            }
            else if (contatoreClick == 0) // Quando viene cliccata la seconda carta che non sia la stessa identica prima carta cliccata si entra nell'else
            {
                abilitaClick = false; // Disabilita i click fino a fine controllo
                cartaCliccata_2 = cartaCliccata;
                StartCoroutine("AnimazioneCarta", cartaCliccata_2); // Coroutine che gestisce l'animazione del flip della carta 2
                refViewManager.ParticelleClickCarta(cartaCliccata_2); // Lancia l'effetto particellare della carta cliccata
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

    // Coroutine che si occupa di distruggere le carte se uguali e di rigirarle se diverse
    private IEnumerator GestisciCoppiaCarte(string azione)
    {
        distruggiCheck = true; // Indica che il suono delle carte non può partire mentre si stanno confrontanto due carte
        if (azione.Equals("distruggi")) {
            
            yield return new WaitForSeconds(1); // Aspetta un secondo
            refViewManager.ParticelleDistruggiCarta(carteCliccate[0]); // Lancia l'effetto particellare della carta distrutta
            refViewManager.ParticelleDistruggiCarta(carteCliccate[1]); // Lancia l'effetto particellare della carta distrutta
            refAudioManager.GetCardDestroy().Play(); // Lancio l'audio della distruzione delle carte
            yield return new WaitForSeconds(1); // Aspetta un secondo
            carteCliccate[0].SetActive(false); // Distruggi Carta 1
            carteCliccate[1].SetActive(false); // Distruggi Carta 2
            punteggio += 100 * combo; // Aumenta il punteggio
            refViewManager.VisualizzaPunteggio(punteggio); // Lancia il metodo che aggiorna il testo del punteggio
            refViewManager.EseguiCombo(combo);
            combo++; // Aumenta la combo attuale
            carteRimanenti -= 2; // Diminuisce le carte rimanenti per la vittoria
            if (carteRimanenti == 0) // Se non rimangono più carte, la partita termina con successo
            {
                refLevelManager.TerminaPartita("vittoria", refViewManager);
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
            yield return new WaitForSeconds(0.2f); // Aspetta 0.15 secondi
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
            refViewManager.DistruggiCuori(vite); // Chiama il metodo che distrugge un cuore
            vite -= 1; // Diminuisce le vite 
            if (vite == -1) // Se non ci sono più vite termina la partita con una sconfitta
            {
                yield return new WaitForSeconds(1); // Aspetta un secondo
                refLevelManager.TerminaPartita("sconfitta", refViewManager);
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

    // Coroutine che mostra l'animazione della combo attuale
    IEnumerator AnimazioneCarta(GameObject cartaCliccata)
    {
        cartaCliccata.GetComponentsInChildren<Animator>()[0].SetBool("cliccata", true); // Lancia l'animazione del retro della carta passata come parametro
        yield return new WaitForSeconds(0.2f); // Aspetta 0.15 secondi
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