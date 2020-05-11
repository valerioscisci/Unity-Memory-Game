using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che definisce tutti i comportamenti e le informazioni della singola carta
public class ScriptCarta : MonoBehaviour
{
    private bool cliccata = false;
    private GameManager refGameManager;

    // Start is called before the first frame update
    void Start()
    {
        refGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); // Prendiamo la referenza del game manager
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Metodo che memorizza il fatto che la carta è stata cliccata, la disattiva e diminuisce di uno il counter del Game Manager
    private void OnMouseDown()
    {
        cliccata = true;
        refGameManager.SottraiClick(gameObject);
    }

    // Setter cliccata
    public void SetCliccata()
    {
        cliccata = false;
    }

    // Setter immagineCarta
    public void SetImmagineCarta(Sprite immagineCarta)
    {
        GetComponent<SpriteRenderer>().sprite = immagineCarta;
    }
}
