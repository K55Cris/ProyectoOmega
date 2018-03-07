using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public List<Carta> Cartas;

    public void ColocarCarta(Carta carta)
    {
        Cartas.Add(carta);
    }

    public void VaciarSlot()
    {
        Cartas.Clear();
    }
}