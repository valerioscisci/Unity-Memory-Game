using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che si occupa di generare il mazzo di carte e di assegnare una posizione sulla griglia a tutte le carte

public class GeneratoreCarte : MonoBehaviour
{
    private GameObject refCarta; // Referenza Carta
    private GameObject[,] mazzoCarte = new GameObject[4,4]; // Crea il mazzo di carte vuoto

    private void Awake()
    {
        refCarta = GameObject.FindGameObjectWithTag("Carta"); // Prende l'oggetto con il tag Carta

        // Cicliamo 16 volte per generare le carte e assegnargli la posizione
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                mazzoCarte[i, j] = GeneraCarta(i, j);
            }
        }
        refCarta.SetActive(false); // Disattiva dal gioco la nostra carta "Blueprint" che ci permette di creare le altre
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Metodo che instanzai una carta e le assegna una posizione
    private GameObject GeneraCarta(int i, int j)
    {
        GameObject cloneCarta = Instantiate(refCarta); // Creiamo un clone della carta
        cloneCarta.SetActive(true); // Attiviamo la carta
        cloneCarta.transform.position = new Vector2(i * 3 - 4, j * 4 - 6);

        return cloneCarta;
    }

    // Setter Mazzo Carte
    public void setMazzoCarte(GameObject[,] mazzo)
    {
        this.mazzoCarte = mazzo;
    }

    // Getter Mazzo Carte
    public GameObject[,] getMazzoCarte()
    {
        return this.mazzoCarte;
    }
}
