using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe che definisce tutti i comportamenti e le informazioni della singola carta
public class ScriptCarta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Setter immagineCarta
    public void setImmagineCarta(Sprite immagineCarta)
    {
        GetComponent<SpriteRenderer>().sprite = immagineCarta;
    }
}
