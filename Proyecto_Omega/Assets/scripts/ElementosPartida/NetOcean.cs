using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class NetOcean : MonoBehaviour
{
    public List<Carta> Cartas;
    public Carta Robar()
    {
        Cartas.RemoveAt(Cartas.Count - 1);
        return Cartas[Cartas.Count - 1];
    }

    public Carta RobarEspesifico(int idCarta)
    {
        foreach (var item in Cartas)
        {
            if (item.cardNumber == idCarta)
            {
                return item;
            }
        }
        return null;
    }

    public void Reiniciar(List<Carta> darkArea)
    {
        
    }

    private void Mezclar()
    {

    }
}