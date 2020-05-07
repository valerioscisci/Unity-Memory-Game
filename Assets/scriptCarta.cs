using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che definisce tutti i comportamenti e le informazioni della singola carta
public class ScriptCarta : MonoBehaviour
{
    private int numeroCarta;
    private string semeCarta;
    private Sprite immagineCarta;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log(this.numeroCarta);
        Debug.Log(this.semeCarta);
        Debug.Log(this.immagineCarta);
        GetComponent<SpriteRenderer>().sprite = this.immagineCarta;
    }

    // Setter numeroCarta
    public void setNumeroCarta(int numeroCarta)
    {
        this.numeroCarta = numeroCarta;
    }

    // Setter segnoCarta
    public void setSemeCarta(string semeCarta)
    {
        this.semeCarta = semeCarta;
    }

    // Setter immagineCarta
    public void setImmagineCarta(Sprite immagineCarta)
    {
        this.immagineCarta = immagineCarta;
    }
}
