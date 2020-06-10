using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Classe che gestisce tutta le chiamate grafiche dell'interfaccia o degli effetti particellare/animazioni
public class ViewManager : MonoBehaviour
{
    private Image[] cuori = new Image[3]; // Variabili per le immaggini dei cuori
    private Text testoPunteggio; // Variabile che mi serve per assegnare il valore del punteggio al testo "In-Game"
    private Text testoMessaggio, valorePunteggio; // Crea le variabili di appoggio dei testi mostrati a fine partita
    private ComboManager refComboManager; // Prendiamo la referenza del combo manager per poter visualizzare le combo
    private GameObject testoPunteggioFinale; // Variabile che mi serve per assegnare il valore del punteggio nel Post-Partita

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            refComboManager = GameObject.FindGameObjectWithTag("ComboManager").GetComponent<ComboManager>(); // Prende l'oggetto con il tag ComboManager
            cuori = GameObject.FindGameObjectWithTag("Cuore").GetComponentsInChildren<Image>(); // Recuperiamo i cuori
            testoPunteggio = GameObject.FindGameObjectWithTag("Punteggio").GetComponent<Text>(); // Prendiamo la referenza al punteggio in game per poi cambiarlo al momento giusto 
        }
        catch (System.Exception e)
        {
            refComboManager = null;
            cuori = null;
            testoPunteggio = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Metodo che lancia la coroutine che si occupa di girare il mazzo di carte dopo 2 secondi
    public void GiraCarteMazzo(GameObject[,] mazzoCarte)
    {
        StartCoroutine("GiraCarte", mazzoCarte); // Lanciamo la Coroutine che gira le carte dopo 2 secondi 
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

    // Metodo che lancia la coroutine per eliminare un cuore dalla UI
    public void DistruggiCuori(int vite)
    {
        StartCoroutine("DistruggiCuore", cuori[vite]); // Rimuove uno dei cuori disponibili
    }

    // Coroutine che mostra l'animazione di distruzione del cuore
    private IEnumerator DistruggiCuore(Image cuore)
    {
        cuore.GetComponent<Animator>().enabled = true; // Lancia l'animazione
        yield return new WaitForSeconds(1); // Aspetta 1 secondo
        Destroy(cuore.gameObject); // Distrugge l'oggetto
    }

    // Aggiorna il valore del punteggio nella UI
    public void VisualizzaPunteggio(int punteggio)
    {
        testoPunteggio.text = punteggio.ToString(); // Assegna il testo del punteggio "In-Game"
    }

    // Aggiorna il messaggio di fine partita nella UI
    public void CambiaMessaggio(string finePartita)
    {
        if (finePartita.Equals("vittoria"))
        {
            GameManager.messaggio = "HAI VINTO!!!";
        }
        else
        {
            GameManager.messaggio = "HAI PERSO...";
        }
    }

    // Lancia l'effetto particellare del click della carta che è stata cliccata
    public void ParticelleClickCarta(GameObject cartaCliccata)
    {
        cartaCliccata.GetComponentsInChildren<ParticleSystem>()[0].Play(); // Lancia l'effetto particellare del click sulla carta 1
    }

    // Lancia l'effetto particellare della distruzione della carta passata
    public void ParticelleDistruggiCarta(GameObject cartaDaDistruggere)
    {
        cartaDaDistruggere.GetComponentsInChildren<ParticleSystem>()[1].Play(); // Lancia l'effetto particellare della distruzione sulla carta passata
    }

    // Lancia l'effetto della combo 
    public void EseguiCombo(int combo)
    {
        StartCoroutine("MostraCombo", combo); // Lanciamo la coroutine che mostra la combo attuale
    }

    // Coroutine che mostra l'animazione della combo attuale
    private IEnumerator MostraCombo(int combo)
    {
        refComboManager.GetComponentInChildren<SpriteRenderer>().sprite = refComboManager.GetSpritesCombo(combo); // Prendiamo la sprite corretta da visualizzare in base alla combo
        refComboManager.GetComponentInChildren<Animator>().enabled = true; // Lanciamo l'animazione che la mostra
        yield return new WaitForSeconds(1); // Aspetta 1 secondo
        refComboManager.GetComponentInChildren<Animator>().enabled = false; // Disattiviamo l'animazione in modo da poterla lanciare nuovamente la prossima volta
    }

    public void AnimazioneFineLivello(GameObject levelManager)
    {
        testoPunteggioFinale = GameObject.FindGameObjectWithTag("Punteggio");
        testoPunteggioFinale.SetActive(false);
        testoMessaggio = GameObject.FindGameObjectWithTag("Messaggio").GetComponent<Text>(); // Recuperiamo l'oggetto messaggio
        testoMessaggio.text = GameManager.messaggio; // Lo valorizziamo con la variabile condivisa messaggio
        if (GameManager.messaggio.Equals("HAI VINTO!!!")) // Se abbiamo vinto mostriamo il punteggio finale ottenuto
        {

            testoPunteggioFinale.SetActive(true); // Se l'utente  ha vinto si mostra il testo "Punteggio"
            valorePunteggio = GameObject.FindGameObjectWithTag("PunteggioFinale").GetComponent<Text>(); // Recupera il testo per inserirci il punteggio finale
            valorePunteggio.text = GameManager.punteggio.ToString(); // Inserisci il punteggio finale
            levelManager.GetComponentsInChildren<ParticleSystem>()[0].Play(); // Lancia l'animazione della vittoria    
            testoMessaggio.color = Color.green; // Mettiamo il colore della scritta in verde
        }
        else // Se abbiamo perso
        {
            levelManager.GetComponentsInChildren<ParticleSystem>()[1].Play(); // Lancia l'animazione della sconfitta
            testoMessaggio.color = Color.red; // Mettiamo il colore della scritta in rosso
        }
    }
}