using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mano : MonoBehaviour
{
    public List<CartaDigimon> Cartas;
    public int Limite;

    public void JugarCarta(Carta Index)
    {
        
    }

    public void DescartarCarta(Carta Index)
    {

    }

    public void RecibirCarta(CartaDigimon carta, bool PlayerOrIA=false)
    {
        Cartas.Add(carta);
        if(PlayerOrIA)
        carta.Mostrar();
    }

    private void Reordenar()
    {

    }
}
