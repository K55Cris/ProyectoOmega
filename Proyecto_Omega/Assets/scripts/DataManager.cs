using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager instance;
    public TextAsset jsonData;

    public Cartas ColeccionDeCartas;
    public List<DigiCarta> TodasLasCartas;


    public Texture[] arrayImage;
    public Sprite[] arraySprites;

    private void Awake()
    {
        instance = this;
        LoadCartas();
    }

   public DigiCarta GetDigicarta(int codigo)
    {
        foreach (var item in TodasLasCartas)
        {
            if (item.id==codigo)
            {
                return item;
            }
        }
        return new DigiCarta(); // error
    }
   
    public void LoadCartas()
    {
        ColeccionDeCartas = JsonUtility.FromJson<Cartas>(jsonData.text);
        TodasLasCartas = ColeccionDeCartas.DigiCartas;
        foreach (var item in ColeccionDeCartas.CartaOption)
        {
            TodasLasCartas.Add(item);
        }
        arrayImage = Resources.LoadAll<Texture>("Digimon");
     //   arraySprites = Resources.LoadAll<Sprite>("Digimon");

    }
    public Texture GetTextureDigimon(int IDigimon)
    {
        foreach (var item in arrayImage)
        {
            if (item.name == IDigimon.ToString())
                return item;
        }
        return null;
    }
    public Sprite GetSprite(int id)
    {
       Sprite arraySprite = Resources.Load<Sprite>("Digimon/"+id.ToString());

        return arraySprite;

      /*  foreach (Sprite item in arraySprites)
        {
            if (item.name == id.ToString())
            {
                return item;
            }
        }
        return null;
        */
    }
}
