using System.Collections;
using UnityEngine;

// Classe che gestisce tutti i suoni del gioco
public class AudioManager : MonoBehaviour
{
    private AudioSource buttonClick, soundtrack_1, soundtrack_2, cardFlip, cardDestroy, victory, lost;

    private void Awake()
    {
        buttonClick = GetComponents<AudioSource>()[0]; // Carica il suono del bottone che viene cliccato
        soundtrack_1 = GetComponents<AudioSource>()[1]; // Carica il suono della colonna sonora 1
        soundtrack_2 = GetComponents<AudioSource>()[2]; // Carica il suono della colonna sonora 2
        cardFlip = GetComponents<AudioSource>()[3]; // Carica il suono della carta che si rigira
        cardDestroy = GetComponents<AudioSource>()[4]; // Carica il suono della carta che si distrugge
        victory = GetComponents<AudioSource>()[5]; // Carica il suono della vittoria
        lost = GetComponents<AudioSource>()[6]; // Carica il suono della sconfitta
    }

    // Getter per il buttonClick
    public AudioSource GetButtonClick()
    {
        return buttonClick; 
    }

    // Getter per la soundtrack_1
    public AudioSource GetSoundtrack_1()
    {
        return soundtrack_1;
    }

    // Getter per la soundtrack_2
    public AudioSource GetSoundtrack_2()
    {
        return soundtrack_2;
    }

    // Getter per il cardFlip
    public AudioSource GetCardFlip()
    {
        return cardFlip;
    }
    
    // Getter per il cardDestroy
    public AudioSource GetCardDestroy()
    {
        return cardDestroy;
    }

    // Metodo che lancia il suono di fine partita in caso di vittoria o sconfitta
    public void PlaySoundrackFineLivello()
    {
        if (GameManager.messaggio.Equals("HAI VINTO!!!")) // Se abbiamo vinto mostriamo il punteggio finale ottenuto
        {
            victory.Play(); // Lanciamo il souno della vittoria
            StartCoroutine("AttesaSoundtrack", "Vittoria"); // Lanciamo la coroutine che attende che il suono della vittoria finisca
        }
        else
        {
            lost.Play();  // Lanciamo il souno della sconfitta
            StartCoroutine("AttesaSoundtrack", "Sconfitta"); // Lanciamo la coroutine che attende che il suono della sconfitta finisca
        }
    }

    // Coroutine che attennde che il suono di vittoria o sconfitta sia terminato
    private IEnumerator AttesaSoundtrack(string messaggio)
    {
        if (messaggio.Equals("Vittoria"))
        {
            yield return new WaitForSeconds(victory.clip.length);
        }
        else if (messaggio.Equals("Sconfitta"))
        {
            yield return new WaitForSeconds(lost.clip.length);
        }
        soundtrack_1.Play(); // Lancia la soundtrack quando è finito il suono di vittoria/sconfitta
    }
 }