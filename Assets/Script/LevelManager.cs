using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Classe che si occupa di gestire gli spostamenti all'interno del menu di gioco e di mostrare i messaggi a fine partita
public class LevelManager : MonoBehaviour
{
    private Button bottonePlay, bottoneQuit, bottoneMenu; // Crea le variabili dei bottoi del menu
    private AudioManager refAudioManager; // Referenza Audio Manager
    private ViewManager refViewManager; // Referenza Audio Manager

    // Start is called before the first frame update
    void Start()
    {
        refAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); // Prende l'oggetto con il tag AudioManager
        refViewManager = GameObject.FindGameObjectWithTag("ViewManager").GetComponent<ViewManager>(); // Prende l'oggetto con il tag ViewManager

        // In base alla scena dove ci troviamo in questo momento, lanciamo una soundtrack diversa 
        if (SceneManager.GetActiveScene().buildIndex == 0) // Il build index ci dice il numero per ciascuna scena in fase di build
        {
            refAudioManager.GetSoundtrack_1().Play(); // Play soundtrack 1
        } else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            refAudioManager.GetSoundtrack_2().Play(); // Play soundtrack 2
        }

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
            bottoneMenu.onClick.AddListener(TornaAlMenu);
            refViewManager.AnimazioneFineLivello(gameObject);
            refAudioManager.PlaySoundrackFineLivello();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Metodo che carica la scena della partita
    private void IniziaPartita()
    {
        refAudioManager.GetButtonClick().Play(); // Lanciamo il suono del click del pulsante
        SceneManager.LoadScene("Partita", LoadSceneMode.Single);
    }

    // Metodo che termina la partita
    private void TerminaPartita()
    {
        refAudioManager.GetButtonClick().Play(); // Lanciamo il suono del click del pulsante
        Application.Quit();
    }

    // Metodo che torna al menu iniziale
    private void TornaAlMenu()
    {
        refAudioManager.GetButtonClick().Play(); // Lanciamo il suono del click del pulsante
        SceneManager.LoadScene("InizioPartita", LoadSceneMode.Single);
    }

    // Metodo che si occupa di terminare la partita
    public void TerminaPartita(string finePartita, ViewManager refViewManager)
    {
        refViewManager.CambiaMessaggio(finePartita); // Lancia il metodo che aggiorna il messaggio a schermo di fine partita
        SceneManager.LoadScene("FinePartita", LoadSceneMode.Single); // Cambio di scena che mostra il risultato della partita, 
    }
}