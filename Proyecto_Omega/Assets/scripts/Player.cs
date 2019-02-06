using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;
using UnityEngine.Events;
using TMPro;
public class Player : MonoBehaviour {

    public string Nombre;
    public Mano _Mano;
    public Mazo Deck;
    public Image Photo;
    public TextMeshProUGUI NombreCuenta;
    public List<int> IDCartasMazo;
    public int PuntosDeVida;
    public List<ListaCartasMove> LoActions= new List<ListaCartasMove>();



    public void moveCard(Transform Padre, CartaDigimon Card, UnityAction<CartaDigimon> Action)
    {
        ListaCartasMove At = new ListaCartasMove();
            At.LoAction = Action;
            At.CartaOption = Card;
            At.Padre = Padre;
            At.ID = LoActions.Count + 1;
            LoActions.Add(At);

        _Mano.JugarCarta(Card);
        Card.Front.GetComponent<MovimientoCartas>().MoverCarta(Padre, Ajustar,At.ID);
    }
    public void moveHand(Transform Padre, CartaDigimon Card, UnityAction<CartaDigimon> Action)
    {
       // Card.transform.SetParent(Padre);
        ListaCartasMove At = new ListaCartasMove();
        At.LoAction = Action;
        At.CartaOption = Card;
        At.Padre = Padre;
        At.ID = LoActions.Count + 1;
        LoActions.Add(At);
        _Mano.RecibirCarta(Card);
        Card.Front.GetComponent<MovimientoCartas>().MoverCarta(Padre, Ajustar,At.ID);
    }
    public void Ajustar(Transform Padre, CartaDigimon LoCard, int ID)
    {
        // se llama cuando la carta a llegado a su destino :V

        ListaCartasMove At = LoActions.Find(x => x.ID ==  ID);
        At.CartaOption.transform.SetParent(At.Padre);
        if (At.LoAction != null)
        {
            At.LoAction.Invoke(At.CartaOption);
        }
        else
        {
            Debug.LogError("VAcio");
        }
    }

}

