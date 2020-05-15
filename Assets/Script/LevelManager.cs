using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Classe che si occupa di gestire gli spostamenti all'interno del menu di gioco e di mostrare i messaggi a fine partita
public class LevelManager : MonoBehaviour
{
    private Button bottonePlay, bottoneQuit, bottoneMenu; // Crea le variabili dei bottoi del menu
    private Text testoMessaggio, valorePunteggio; // Crea le variabili di appoggio dei testi mostrati a fine partita
    private GameObject testoPunteggio;
    private AudioManager refAudioManager; // Referenza Audio Manager
    private bool checkSoundtrack = false; // 

    // Start is called before the first frame update
    void Start()
    {
        refAudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>(); // Prende l'oggetto con il tag AudioManager

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
            testoPunteggio = GameObject.FindGameObjectWithTag("Punteggio");
            testoPunteggio.SetActive(false);
            bottoneMenu.onClick.AddListener(TornaAlMenu);
            testoMessaggio = GameObject.FindGameObjectWithTag("Messaggio").GetComponent<Text>(); // Recuperiamo l'oggetto messaggio
            testoMessaggio.text = SharedVariables.messaggio; // Lo valorizziamo con la variabile condivisa messaggio
            if (SharedVariables.messaggio.Equals("HAI VINTO!!!")) // Se abbiamo vinto mostriamo il punteggio finale ottenuto
            {
                refAudioManager.GetVictory().Play(); // Lanciamo il souno della vittoria
                StartCoroutine("AttesaSoundtrack", "Vittoria"); // Lanciamo la coroutine che attende che il suono della vittoria finisca
                testoPunteggio.SetActive(true); // Se l'utente  ha vinto si mostra il testo "Punteggio"
                valorePunteggio = GameObject.FindGameObjectWithTag("PunteggioFinale").GetComponent<Text>(); // Recupera il testo per inserirci il punteggio finale
                valorePunteggio.text = SharedVariables.punteggio.ToString(); // Inserisci il punteggio finale
                GetComponentsInChildren<ParticleSystem>()[0].Play(); // Lancia l'animazione della vittoria    
                testoMessaggio.color = Color.green; // Mettiamo il colore della scritta in verde
            }
            else // Se abbiamo perso
            {
                refAudioManager.GetLost().Play();  // Lanciamo il souno della sconfitta
                StartCoroutine("AttesaSoundtrack", "Sconfitta"); // Lanciamo la coroutine che attende che il suono della sconfitta finisca
                GetComponentsInChildren<ParticleSystem>()[1].Play(); // Lancia l'animazione della sconfitta
                testoMessaggio.color = Color.red; // Mettiamo il colore della scritta in rosso
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

    // Coroutine che attennde che il suono di vittoria o sconfitta sia terminato
    IEnumerator AttesaSoundtrack(string messaggio)
    {
        if (messaggio.Equals("Vittoria"))
        {
            yield return new WaitForSeconds(refAudioManager.GetVictory().clip.length);
        }
        else if (messaggio.Equals("Sconfitta"))
        {
            yield return new WaitForSeconds(refAudioManager.GetLost().clip.length);
        }
        refAudioManager.GetSoundtrack_1().Play(); // Lancia la soundtrack quando è finito il suono di vittoria/sconfitta
    }
}