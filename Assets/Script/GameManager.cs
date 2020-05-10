using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che gestisce il gioco nella sua interezza
public class GameManager : MonoBehaviour
{
    private GeneratoreCarte refGeneratore;
    private GameObject cartaCliccata_1 = null, cartaCliccata_2 = null; // Variabili per memorizzare le carte che sono state cliccate
    private int contatoreClick = 2; // Contatore che memorizza il numero di click disponibili
    private bool abilitaClick = true;
    private GameObject[] carteCliccate = new GameObject[2]; // Variabile di appoggio da passare alla Coroutine che verificherà se esse sono uguali

// Start is called before the first frame update
void Start()
    {
        refGeneratore = GameObject.FindGameObjectWithTag("GeneratoreCarte").GetComponent<GeneratoreCarte>(); // Prendiamo la referenza del generatore di carte
        GameObject[,] mazzoCarte = refGeneratore.getMazzoCarte(); // Recuperiamo il mazzo di carte generato dal generatore
        StartCoroutine("GiraCarte", mazzoCarte); // Lanciamo la Coroutine che gira le carte dopo 2 secondi 
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
            if (contatoreClick == 1) // Quando viene cliccata la prima carta, la gira per mostrarla
            {
                cartaCliccata_1 = cartaCliccata;
                cartaCliccata_1.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            }
            else if (contatoreClick == 0) // Quando viene cliccata la seconda carta che non sia la stessa identica prima carta cliccata si entra nell'else
            {
                abilitaClick = false; // Disabilita i click fino a fine controllo
                cartaCliccata_2 = cartaCliccata;
                cartaCliccata_2.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false; //Mostra la seconda carta cliccata
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
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(2);
        foreach (GameObject carta in mazzoCarte)
        {
            carta.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
        }
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Coroutine che si occupa di distruggere le carte se uguali e di rigirarle se diverse
    private IEnumerator GestisciCoppiaCarte(string azione)
    {
        yield return new WaitForSeconds(0.5f); // Aspetta mezzo secondo

        if (azione.Equals("distruggi")) {
            carteCliccate[0].SetActive(false); // Distruggi Carta 1
            carteCliccate[1].SetActive(false); // Distruggi Carta 2
        }
        else if (azione.Equals("reset"))
        {
            carteCliccate[0].GetComponent<ScriptCarta>().setCliccata(); // Indica che la prima carta non è più cliccata
            carteCliccate[1].GetComponent<ScriptCarta>().setCliccata(); // Indica che la seconda carta non è più cliccata 
            carteCliccate[0].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true; // Rigira la carta 1
            carteCliccate[1].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true; // Rigira la carta 2
        }
        abilitaClick = true; // Abilita click
        contatoreClick = 2; // Reset contatore
    }
}