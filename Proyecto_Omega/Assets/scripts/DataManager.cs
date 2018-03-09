using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager instance;
    public TextAsset jsonData;
    public Sprite[] Cartas;

    public List<DigiCarta> ColeccionDeCartas;
    public Texture[] arrayImage;

    private void Awake()
    {
        instance = this;
        LoadCartas();
    }

   public DigiCarta GetDigicarta(int codigo)
    {
        foreach (var item in ColeccionDeCartas)
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
        ColeccionDeCartas = JsonUtility.FromJson<Cartas>(jsonData.text).DigiCartas;
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
