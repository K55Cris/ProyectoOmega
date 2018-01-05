using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public static DataManager instance;
    private List<DigiCarta> ColeccionDeCartas;

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
}
