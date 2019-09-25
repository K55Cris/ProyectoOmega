using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public DeckCartas Cartas = new DeckCartas();
    public List<Players> Jugadores = new List<Players>();
    public void repartir()
    {
        foreach (var item in Jugadores)
        {
            item.GetCarta(Cartas.Robar());
            item.GetCarta(Cartas.Robar());
        }
    }
}

public class DeckCartas
{
    public List<Carta> Cartas;
    public Carta Robar()
    {
        if (Cartas.Count == 0)
            return null;
        Carta Dcard = Cartas[0];
        Cartas.RemoveAt(0);
        return Dcard;
    }
}
public class Players
{
    public Vector3 Posicion = new Vector3();
    public List<Carta> Hand;
    public void GetCarta(Carta Card)
    {
        // aqui pones el metodo para mover la carta al jugador el Move
    }
}
