using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratoreCarte : MonoBehaviour
{
    public GameObject refCarta; // Referenza Carta
    private GameObject[,] mazzoCarte = new GameObject[4,4];
    
    // Start is called before the first frame update
    void Start()
    {
        refCarta = GameObject.FindGameObjectWithTag("Carta"); // Prende l'oggetto con il tag carta

        // Cicliamo 16 volte per generare le carte e assegnargli la posizione
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                mazzoCarte[i, j] = GeneraCarta(i, j);
            }
        }
        refCarta.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    GameObject GeneraCarta(int i, int j)
    {
        GameObject cloneCarta = Instantiate(refCarta); // Creiamo un clone della carta
        cloneCarta.SetActive(true); // Attiviamo la carta
        cloneCarta.transform.position = new Vector2(i * 3 - 4, j * 4 - 6);

        return cloneCarta;
    }
}
