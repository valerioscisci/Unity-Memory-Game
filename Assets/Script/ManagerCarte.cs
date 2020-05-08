using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che si occupa di assegnare valori alle carte e di mischiarle randomicamente
public class ManagerCarte : MonoBehaviour
{
    private GeneratoreCarte refGeneratore;
    private int numeroSpriteRandom;
    HashSet<int> checkCartaGenerata = new HashSet<int>();
    private Sprite[] spritesCarte;
    private Vector3[] posizioniCarte;

    // Start is called before the first frame update
    void Start()
    {
        refGeneratore = GetComponent<GeneratoreCarte>(); // Prendiamo la referenza del generatore di carte
        GameObject[,] mazzoCarte = refGeneratore.getMazzoCarte(); // Recuperiamo il mazzo di carte generato dal generatore
        object[] carteCaricate = Resources.LoadAll("Memory1/Cards/PNG-cards-1.3/", typeof(Sprite)); // Carica le Sprite in un array di oggetti. Nota: Resources accede alla cartella Resources salvata in Assets

        spritesCarte = new Sprite[carteCaricate.Length]; // Creiamo un array di Sprites
        
        // Cicliamo per fare la conversione da oggetto generico a Sprite per ogni Sprite caricata
        for (int i = 0; i < carteCaricate.Length; i++)
        {
            spritesCarte[i] = (Sprite)carteCaricate[i];
        }

        posizioniCarte = refGeneratore.getPosizioniCarte(); // Recuperiamo il vettore di posizioni iniziali dalla referenza al generatore
        // Chiamiamo il metodo che mischia le carte
        shuffle(posizioniCarte);

        // Cicliamo 8 volte per valorizzare gli attributi delle coppie di carte
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j = j + 2)
            {
                numeroSpriteRandom = Random.Range(0, 67); // Generiamo il valore della sprite carta assicurandoci con l'HashSet che non sia già stato generato in precedenza
                while (!checkCartaGenerata.Add(numeroSpriteRandom))
                {
                    numeroSpriteRandom = Random.Range(1, 14);
                }

                // Assegnamo le sprite della coppia di carte
                mazzoCarte[i, j].GetComponent<ScriptCarta>().setImmagineCarta(spritesCarte[numeroSpriteRandom]);
                mazzoCarte[i, j + 1].GetComponent<ScriptCarta>().setImmagineCarta(spritesCarte[numeroSpriteRandom]);

                // Assegnamo la posizione alla coppia di carte
                mazzoCarte[i, j].transform.position = posizioniCarte[i * 4 + j];
                mazzoCarte[i, j + 1].transform.position = posizioniCarte[i * 4 + j + 1];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Metodo che mischia l'array delle posizioni (Knuth shuffle algorithm)
    void shuffle(Vector3[] posizioniCarte)
    {
        for (int i = 0; i < posizioniCarte.Length; i++)
        {
            Vector3 posizioneTemporanea = posizioniCarte[i]; // Memorizziamo la posizione identificata dal valore corrente dell'indice i
            int posizioneRandom = Random.Range(0, posizioniCarte.Length); // Genera l'indice che ci permetterà di fare lo swap con la nuova posizione
            posizioniCarte[i] = posizioniCarte[posizioneRandom]; // Salviamo la nuova posizione random nella posizione corrente
            posizioniCarte[posizioneRandom] = posizioneTemporanea; // Salviamo la vecchia posizione nella posizione random
        }
    }
}
