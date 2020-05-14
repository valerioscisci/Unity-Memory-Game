using System.Collections;
using System.Collections.Generic;
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

    // Getter per la victory
    public AudioSource GetVictory()
    {
        return victory;
    }

    // Getter per la lost
    public AudioSource GetLost()
    {
        return lost;
    }
}
