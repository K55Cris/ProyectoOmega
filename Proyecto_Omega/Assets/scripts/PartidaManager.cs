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

    private void Start()
    {
        StaticRules.SelectDigimonChild();
    }


    public void CargarMazos(List<int> Deck,Transform Espacio,Player _Player)
    {
        List<DigiCarta> DatosDigi = DataManager.instance.TodasLasCartas;
        int contador = 1;
        List<CartaDigimon> MazoPlayer = new List<CartaDigimon>();
        foreach (var carta in Deck)
        {
            GameObject DigiCarta = Instantiate(CartaPrefap, Espacio);
            DigiCarta.GetComponent<CartaDigimon>().AjustarSlot();
           
            DigiCarta.GetComponent<CartaDigimon>().cardNumber = contador;
            DigiCarta.GetComponent<CartaDigimon>().DatosDigimon = DatosDigi.Find(x => x.id == carta);
            MazoPlayer.Add(DigiCarta.GetComponent<CartaDigimon>());
            contador++;
        }
        _Player.Deck.cartas = MazoPlayer;
    }
    public void cargarManos(Transform Mano, Player jugador, Transform Deck)
    {
        for (int i = 1; i < 7; i++)
        {
    
            GameObject Carta = Deck.transform.GetChild(jugador.Deck.cartas.Count-i-1).gameObject;
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


    public static void Barajear(Transform Deck)
    {
        int k = 0;
        while (k < 25)
        {
            int val = Random.Range(0, 29);
            Transform _Carta = Deck.transform.GetChild(val);
            _Carta.transform.parent = null;
            _Carta.transform.parent = Deck;
            k++;
        }

    }


}
