using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
using UnityEngine.Events;
public class Player : MonoBehaviour {

    public string Nombre;
    public Mano _Mano;
    public Mazo Deck;
    public List<int> IDCartasMazo;
    public int PuntosDeVida;
    private UnityAction<CartaDigimon> LoAction;

    public void moveCard(Transform Padre, CartaDigimon Card, UnityAction<CartaDigimon> Action)
    {
        // Card.transform.SetParent(Padre);
        LoAction = Action;
        Card.Front.GetComponent<MovimientoCartas>().MoverCarta(Padre, Ajustar);
    }
    public void Ajustar(Transform Padre, CartaDigimon LoCard)
    {
        // se llama cuando la carta a llegado a su destino :V
        LoCard.transform.SetParent(Padre);
        if (LoAction != null)
            LoAction.Invoke(LoCard);

    }

}

