using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Classe che si occupa di gestire gli spostamenti all'interno del meenu di gioco e di mostrare i messaggi a fine partita
public class LevelManager : MonoBehaviour
{
    private Button bottonePlay, bottoneQuit, bottoneMenu; // Crea le variabili dei bottoi del menu
    private Text testoMessaggio, valorePunteggio; // CRea le variabili di appoggio dei testi mostrati a fine partita
    private GameObject testoPunteggio;

    // Start is called before the first frame update
    void Start()
    {
        // Se presente  nella scena, aggiunge un listener sul bottone Play
        try
        {
            bottonePlay = GameObject.FindGameObjectWithTag("Play").GetComponent<Button>();
        }
        catch (System.Exception e)
        {
            bottonePlay = null;
        }
        if (bottonePlay != null)
        {
            bottonePlay.onClick.AddListener(IniziaPartita);
        }

        // Se presente  nella scena, aggiunge un listener sul bottone Quit
        try
        {
            bottoneQuit = GameObject.FindGameObjectWithTag("Quit").GetComponent<Button>();
        }
        catch (System.Exception e)
        {
            bottoneQuit = null;
        }
        if (bottoneQuit != null)
        {
            bottoneQuit.onClick.AddListener(TerminaPartita);
        }

        // Se presente  nella scena, aggiunge un listener sul bottone Menu
        try
        { 
            bottoneMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Button>();
        } catch (System.Exception e)
        {
            bottoneMenu = null;
        }
        if (bottoneMenu != null)
        {
            testoPunteggio = GameObject.FindGameObjectWithTag("Punteggio");
            testoPunteggio.SetActive(false);
            bottoneMenu.onClick.AddListener(TornaAlMenu);
            testoMessaggio = GameObject.FindGameObjectWithTag("Messaggio").GetComponent<Text>(); // Recuperiamo l'oggetto messaggio
            testoMessaggio.text = SharedVariables.messaggio; // Lo valorizziamo con la variabile condivisa messaggio
            if (SharedVariables.messaggio.Equals("HAI VINTO!!!")) // Se abbiamo vinto mostriamo il punteggio finale ottenuto
            {
                testoPunteggio.SetActive(true); // Se l'utente  ha vinto si mostra il testo "Punteggio"
                valorePunteggio = GameObject.FindGameObjectWithTag("PunteggioFinale").GetComponent<Text>(); // Recupera il testo per inserirci il punteggio finale
                valorePunteggio.text = SharedVariables.punteggio.ToString(); // Inserisci il punteggio finale
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Metodo che carica la scena della partita
    private void IniziaPartita()
    {
        SceneManager.LoadScene("Partita", LoadSceneMode.Single);
    }

    // Metodo che termina la partita
    private void TerminaPartita()
    {
        Application.Quit();
    }

    // Metodo che torna al menu iniziale
    private void TornaAlMenu()
    {
        SceneManager.LoadScene("InizioPartita", LoadSceneMode.Single);
    }
}
