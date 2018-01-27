using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mano : MonoBehaviour
{
    public List<Carta> Cartas;
    public int Limite;

    public void JugarCarta(Carta Index)
    {
        
    }

    public void DescartarCarta(Carta Index)
    {

    }

    public void RecibirCarta(Carta carta)
    {
        Cartas.add(carta);
    }

    private void Reordenar()
    {

    }
}
