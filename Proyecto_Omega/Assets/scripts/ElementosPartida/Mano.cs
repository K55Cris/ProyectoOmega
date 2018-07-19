using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mano : MonoBehaviour
{
    public List<CartaDigimon> Cartas;
    public int Limite;

    public void JugarCarta(CartaDigimon carta)
    {
    Cartas.Remove(carta); 
    }

    public void DescartarCarta(CartaDigimon carta)
    {
    Cartas.Remove(carta);
    }

    public void RecibirCarta(CartaDigimon carta, bool PlayerOrIA=false)
    {
        Cartas.Add(carta);
        if (PlayerOrIA)
        {
            carta.Mostrar();
            carta.transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 180));
        }
       
    }

    private void Reordenar()
    {

    }
}
