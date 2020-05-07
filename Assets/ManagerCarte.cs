using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che si occupa di assegnare valori alle carte e di mischiarle randomicamente
public class ManagerCarte : MonoBehaviour
{
    private GeneratoreCarte refGeneratore;
    private int numeroCartaRandom, numeroSemeRandom;
    HashSet<int> coppieGenerate = new HashSet<int>();
    private string[] semeCarta = { "Picche", "Fiori", "Diamanti", "Cuori" };
    private Sprite[] spritesCarte;

    // Start is called before the first frame update
    void Start()
    {
        refGeneratore = GetComponent<GeneratoreCarte>();
        GameObject[,] mazzoCarte = refGeneratore.getMazzoCarte(); // Recuperiamo il mazzo di carte generato dal generatore
        object[] carteCaricate = Resources.LoadAll("Assets/Memory1/Cards/PNG-cards-1.3/", typeof(Sprite));

        spritesCarte = new Sprite[carteCaricate.Length];
        
        for (int i = 0; i < carteCaricate.Length; i++)
        {
            spritesCarte[i] = (Sprite)carteCaricate[i];
        }

        // Cicliamo 8 volte per valorizzare gli attributi delle coppie di carte
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j = j + 2)
            {
                numeroCartaRandom = Random.Range(1,14); // Generiamo il valore della carta
                numeroSemeRandom = Random.Range(0, 4); // Generiamo il seme della carta

                // Questo ciclo controlla che la coppia che stiamo generando non è già presente sulla griglia
                while (!coppieGenerate.Add(numeroCartaRandom))
                {
                    numeroCartaRandom = Random.Range(1, 14);
                }

                // Assegnamo il numero della coppia di carte
                mazzoCarte[i, j].GetComponent<ScriptCarta>().setNumeroCarta(numeroCartaRandom);
                mazzoCarte[i, j + 1].GetComponent<ScriptCarta>().setNumeroCarta(numeroCartaRandom);

                // Assegnamo il seme della coppia di carte
                mazzoCarte[i, j].GetComponent<ScriptCarta>().setSemeCarta(semeCarta[numeroSemeRandom]);
                mazzoCarte[i, j + 1].GetComponent<ScriptCarta>().setSemeCarta(semeCarta[numeroSemeRandom]);

                // Assegnamo le sprite della coppia di carte
                mazzoCarte[i, j].GetComponent<ScriptCarta>().setImmagineCarta(spritesCarte[1]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
