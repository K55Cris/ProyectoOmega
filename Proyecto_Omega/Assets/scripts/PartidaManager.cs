using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class PartidaManager : MonoBehaviour {

    public Player Player1;
    public Player Player2;
    public GameObject CartaPrefap;
    public Transform ManoPlayer1;
    public Transform ManoPlayer2;

    public static PartidaManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (Player1.IDCartasMazo.Count > 0)
        {
            Player1.Nombre = PlayerManager.instance.Jugador.Nombre;
            Player1.IDCartasMazo = PlayerManager.instance.Jugador.IDCartasMazo;
        }
        else
        {
            Player1.IDCartasMazo = PlayerDefault.instance.Default.IDCartasMazo;
        }
        StaticRules.SelectDigimonChild();
    }


    public void CargarMazos(List<int> Deck, Transform Espacio, Player _Player)
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
        Transform netocean = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, _Player);
        netocean.GetComponent<NetOcean>().Cartas = MazoPlayer;
    }
    public void cargarManos(Transform Mano, Player jugador, Transform Deck)
    {
        StartCoroutine(CrearYColocar(Mano, jugador, Deck));
    }

    IEnumerator CrearYColocar(Transform Mano, Player jugador, Transform Deck)
    {

        for (int i = 1; i < 7; i++)
        {
            yield return new WaitForSecondsRealtime(0.3f);

            CartaDigimon Carta =  MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, jugador).GetComponent<NetOcean>().Robar();

            if (jugador == Player1)
                jugador._Mano.RecibirCarta(Carta, true);
            else
                jugador._Mano.RecibirCarta(Carta);

            PartidaManager.instance.SetMoveCard(Mano, Carta.transform);
            Carta.transform.localPosition = Vector3.zero;
            Carta.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            Carta.transform.localScale = new Vector3(25, 40, 0.015f);
        }
    }

    public void TomarCarta(Transform Mano, Player jugador, Transform Deck)
    {
        GameObject Carta = Deck.transform.GetChild(0).gameObject;
        if (jugador == Player1)
            jugador._Mano.RecibirCarta(Carta.GetComponent<CartaDigimon>(), true);
        else
            jugador._Mano.RecibirCarta(Carta.GetComponent<CartaDigimon>());

        PartidaManager.instance.SetMoveCard(Mano,Carta.transform);
        Carta.transform.localPosition = Vector3.zero;
        Carta.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Carta.transform.localScale = new Vector3(25, 40, 0.015f);
    }



    public static void Barajear(Transform Deck)
    {
        NetOcean NDeck = Deck.GetComponent<NetOcean>();

        int k = 0;
        while (k < 45)
        {
            int val = Random.Range(0, 29);
            CartaDigimon _Carta = NDeck.Cartas[val];
            NDeck.Cartas.Remove(_Carta);
            NDeck.Cartas.Add(_Carta);
            k++;
        }

    }
    public void BotonListo()
    {
        StaticRules.SiguienteFase();
    }
    public Transform GetHand()
    {
        if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
        {
            return ManoPlayer1;
        }
        else
        {
            return ManoPlayer2;
        }
    }
    public void SetMoveCard(Transform Padre,Transform Carta)
    {
        if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
        {
            Player1.moveCard(Padre, Carta.GetComponent<CartaDigimon>());
        }
        else
        {
            Player2.moveCard(Padre, Carta.GetComponent<CartaDigimon>());
        }
    }

    public void TomarOtraCarta()
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean).GetComponent<NetOcean>().RobarInteligente();
    }
}