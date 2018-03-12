using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager instance;
    public TextAsset jsonData;
    public Sprite[] Cartas;

    public Cartas ColeccionDeCartas;
    public List<DigiCarta> TodasLasCartas;


    public Texture[] arrayImage;

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
}
