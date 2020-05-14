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
    private bool abilitaClick = true;
    private GameObject[] carteCliccate = new GameObject[2]; // Variabile di appoggio da passare alla Coroutine che verificherà se esse sono uguali
    private int vite = 2, combo = 1; // Variabili che contengono le vite e la combo  attuali
    private Text testoPunteggio; // Variabile che mi serve per assegnare il valore del punteggio al testo "In-Game"
    private Image[] cuori = new Image[3]; // Variabili per le immaggini dei cuori
    private int carteRimanenti = 16; // Contatore della carte rimanenti da accoppiare prima del termine della partita
    private AudioManager refAudioManager; // Prendiamo la referenza all'audio manager per poter lanciare i suoni

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
                cartaCliccata_1.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                cartaCliccata_1.GetComponentsInChildren<ParticleSystem>()[0].Play(); // Lancia l'animazione del click sulla carta 1
            }
            else if (contatoreClick == 0) // Quando viene cliccata la seconda carta che non sia la stessa identica prima carta cliccata si entra nell'else
            {
                abilitaClick = false; // Disabilita i click fino a fine controllo
                cartaCliccata_2 = cartaCliccata;
                cartaCliccata_2.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false; //Mostra la seconda carta cliccata
                cartaCliccata_2.GetComponentsInChildren<ParticleSystem>()[0].Play(); // Lancia l'animazione del click sulla carta 2
                CheckNomi(); // Parte il metodo per il controllo delle carte cliccate
            }
        }
    }

    // Metodo che controlla se le carte cliccate sono identiche per poi andarle a distruggere
    private void CheckNomi()
    {
        carteCliccate = new GameObject[] { cartaCliccata_1, cartaCliccata_2 };

        if (cartaCliccata_1.GetComponent<SpriteRenderer>().sprite.name.Equals(cartaCliccata_2.GetComponent<SpriteRenderer>().sprite.name))
        { // Se sono uguali disattivale
            StartCoroutine("GestisciCoppiaCarte", "distruggi"); // Parte  la coroutine che aspetta .5 secondi e distrugge le carte
        }
        else
        { // Altrimenti reset carte 
            StartCoroutine("GestisciCoppiaCarte", "reset"); // Parte  la coroutine che aspetta .5 secondi e rigira le carte
        }
        cartaCliccata_1 = null; // Azzera la prima carta così può riniziare il controllo della prossima coppia di carte cliccate
    }

    // Coroutine che abilita il retro di tutte le carte del mazzo ad inizio partita dopo 2 secondi
    private IEnumerator GiraCarte(GameObject[,] mazzoCarte)
    {
        Cursor.lockState = CursorLockMode.Locked; // Blocca il cursore dell'utente
        yield return new WaitForSeconds(2);
        foreach (GameObject carta in mazzoCarte)
        {
            carta.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
        }
        Cursor.lockState = CursorLockMode.Confined; // Riabilita il cursore dell'utente
    }

    // Coroutine che si occupa di distruggere le carte se uguali e di rigirarle se diverse
    private IEnumerator GestisciCoppiaCarte(string azione)
    {
        if (azione.Equals("distruggi")) {
            carteCliccate[0].GetComponentsInChildren<ParticleSystem>()[1].Play(); // Lancia l'animazione della distruzione sulla carta 1
            carteCliccate[1].GetComponentsInChildren<ParticleSystem>()[1].Play(); // Lancia l'animazione della distruzione sulla carta 2
            refAudioManager.GetCardDestroy().Play();
            yield return new WaitForSeconds(1); // Aspetta un secondo
            carteCliccate[0].SetActive(false); // Distruggi Carta 1
            carteCliccate[1].SetActive(false); // Distruggi Carta 2
            SharedVariables.punteggio += 100 * combo; // Aumenta il punteggio
            testoPunteggio.text = SharedVariables.punteggio.ToString(); // Assegna il testo del punteggio "In-Game"
            combo++; // Aumenta la combo attuale
            carteRimanenti -= 2; // Diminuisce le carte rimanenti per la vittoria
            if (carteRimanenti == 0) // Se non rimangono più carte, la partita termina con successo
            {
                TerminaPartita("vittoria");
            }
        }
        else if (azione.Equals("reset"))
        {
            yield return new WaitForSeconds(1); // Aspetta un secondo
            carteCliccate[0].GetComponent<ScriptCarta>().SetCliccata(); // Indica che la prima carta non è più cliccata
            carteCliccate[1].GetComponent<ScriptCarta>().SetCliccata(); // Indica che la seconda carta non è più cliccata 
            carteCliccate[0].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true; // Rigira la carta 1
            carteCliccate[1].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true; // Rigira la carta 2
            combo = 1; // Resetta le combo
            cuori[vite].enabled = false; // Rimuove uno dei cuori disponibili
            vite -= 1; // Diminuisce le vite 
            if (vite == -1) // Se non ci sono più vite termina la partita con una sconfitta
            {
                TerminaPartita("sconfitta");
            }
        }
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

    // Getter dell'Audio Manager
    public AudioManager GetAudioManager()
    {
        return refAudioManager;
    }
}