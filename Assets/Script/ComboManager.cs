using UnityEngine;

// Classe che si occupa di caricare e gestire le sprite delle combo
public class ComboManager : MonoBehaviour
{
    private Sprite[] spritesCombos;

    // Start is called before the first frame update
    void Start()
    {
        object[] comboCaricate = Resources.LoadAll("Combo/", typeof(Sprite)); // Carica le Sprite in un array di oggetti. Nota: Resources accede alla cartella Resources salvata in Assets
        spritesCombos = new Sprite[comboCaricate.Length]; // Creiamo un array di Sprites

        // Cicliamo per fare la conversione da oggetto generico a Sprite per ogni Sprite caricata
        for (int i = 0; i < comboCaricate.Length; i++)
        {
            spritesCombos[i] = (Sprite)comboCaricate[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getter Sprite Combo
    public Sprite GetSpritesCombo(int combo)
    {
        return spritesCombos[combo-1]; // Il -1 serve per far partire le combo da 1 visto che altrimenti alla prima coppia di carte indovinate si avrebbe combo=2
    }
}
