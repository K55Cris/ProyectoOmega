using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class Player : MonoBehaviour {

    public string Nombre;
    public Mano _Mano;
    public Mazo Deck;
    public List<int> IDCartasMazo;
    public int PuntosDeVida;

    public void moveCard(Transform Padre, CartaDigimon Card)
    {
        Card.transform.SetParent(Padre);
    }
}

