using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che gestisce il gioco nella sua interezza
public class GameManager : MonoBehaviour
{
    private GeneratoreCarte refGeneratore;
    private GameObject cartaCliccata_1, cartaCliccata_2; // Variabili per memorizzare le carte che sono state cliccate
    private int contatoreClick = 2; // Contatore che memorizza il numero di click disponibili

    // Start is called before the first frame update
    void Start()
    {
        refGeneratore = GameObject.FindGameObjectWithTag("GeneratoreCarte").GetComponent<GeneratoreCarte>(); // Prendiamo la referenza del generatore di carte
        GameObject[,] mazzoCarte = refGeneratore.getMazzoCarte(); // Recuperiamo il mazzo di carte generato dal generatore
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Metodo per sottrarre un click disponibile e verificare se il contatore è a 0 per poi lanciare il metodo che verifica se le carte cliccate sono uguali
    public void SottraiClick(GameObject cartaCliccata)
    {
        contatoreClick -= 1;
        if (contatoreClick == 1) {
            cartaCliccata_1 = cartaCliccata;
        } else {
            cartaCliccata_2 = cartaCliccata;
            CheckNomi();
        }
    }

    // Metodo che controlla se le carte cliccate sono identiche per poi andarle a distruggere
    private void CheckNomi()
    {
        if (cartaCliccata_1.GetComponent<SpriteRenderer>().sprite.name.Equals(cartaCliccata_2.GetComponent<SpriteRenderer>().sprite.name))
        { // Se sono uguali disattivale
            cartaCliccata_1.SetActive(false);
            cartaCliccata_2.SetActive(false);
        } else { // Altrimenti reset carte 
            cartaCliccata_1.GetComponent<ScriptCarta>().setCliccata();
            cartaCliccata_2.GetComponent<ScriptCarta>().setCliccata();
        }
        contatoreClick = 2; // Reset contatore
    }
}
