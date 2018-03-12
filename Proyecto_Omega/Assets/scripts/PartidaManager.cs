using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class PartidaManager : MonoBehaviour {

    public Player Player1;
    public Player Player2;
    public GameObject CartaPrefap;
    public Transform DeckPlayer1;
    public Transform DeckPlayer2;
    public Transform ManoPlayer1;
    public Transform ManoPlayer2;

    public static PartidaManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        CargarMazos(Player1.IDCartasMazo,DeckPlayer1);
        CargarMazos(Player2.IDCartasMazo, DeckPlayer2);
        cargarManos(ManoPlayer1, Player1, DeckPlayer1);
        cargarManos(ManoPlayer2, Player2, DeckPlayer2);
    }
    


    public void CargarMazos(List<int> Deck,Transform Espacio)
    {
        List<DigiCarta> DatosDigi = DataManager.instance.TodasLasCartas;
        int contador = 1;
        foreach (var carta in Deck)
        {
            GameObject DigiCarta = Instantiate(CartaPrefap, Espacio);
            DigiCarta.GetComponent<CartaDigimon>().AjustarSlot();
            DigiCarta.GetComponent<CartaDigimon>().cardNumber = contador;
            DigiCarta.GetComponent<CartaDigimon>().DatosDigimon = DatosDigi.Find(x => x.id == carta);
            contador++;
        }
    }
    public void cargarManos(Transform Mano, Player jugador, Transform Deck)
    {
        for (int i = 1; i < 7; i++)
        {
    
            GameObject Carta = Deck.transform.GetChild(jugador.Deck.cartas.Count-i).gameObject;
            if(jugador==Player1)
            jugador._Mano.RecibirCarta(Carta.GetComponent<CartaDigimon>(),true);
            else
            jugador._Mano.RecibirCarta(Carta.GetComponent<CartaDigimon>());

            Carta.transform.parent = Mano;
            Carta.transform.localPosition = Vector3.zero;
            Carta.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Carta.transform.localScale = new Vector3(26, 45, 0.015f);
        }
    }
    

}
