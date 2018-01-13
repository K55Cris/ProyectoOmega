using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager instance;

    public List<DigiCarta> ColeccionDeCartas;

    private void Awake()
    {
        instance = this;
    }

   public DigiCarta GetDigicarta(string codigo)
    {
        foreach (var item in ColeccionDeCartas)
        {
            if (item.CompararCodigo(codigo))
            {
                return item;
            }
        }
        return new DigiCarta(); // error
    }
    
    public void LoadCartas(string jsonData)
    {
        ColeccionDeCartas = JsonUtility.FromJson<Cartas>(jsonData).DigiCartas;
    }
}
