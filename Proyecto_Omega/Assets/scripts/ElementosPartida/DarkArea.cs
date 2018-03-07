using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;

public class DarkArea : MonoBehaviour {

    public List<Carta> Cartas;
    public static DarkArea instance;
    public void Vaciar()
    {
        Cartas = new List<Carta>();
    }

    
}
