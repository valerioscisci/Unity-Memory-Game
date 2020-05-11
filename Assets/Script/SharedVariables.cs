using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che contiene tutte le variabili che vogliamo condividere tra le varie scene
public class SharedVariables : MonoBehaviour
{
    static public string messaggio = ""; // Variabile per contenere il messaggio a fine partita
    static public int punteggio = 0; // Messaggio per contenere il punteggio accumulato in una partita
}
