using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptCarta : MonoBehaviour
{
    public int numeroCarta;
    public string segnoCarta;
    public Sprite immagineCarta;

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
        Destroy(gameObject);
    }
}
