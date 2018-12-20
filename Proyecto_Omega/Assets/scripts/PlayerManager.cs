using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;
    public SavePlayer Default;
    public SavePlayer Jugador;
    public int Nivel = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
           
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    public void Start()
    {
        // conseguimos los datos
        GetData();
    }

    public void GetData()
    {
        // cargar Deck EN EL JUGADOR
        string cartas = PlayerPrefs.GetString("SavePlayer");
        if (cartas!="")
        {
            Jugador = JsonUtility.FromJson<SavePlayer>(cartas);
            // Datos cargados
        }
        else
        {
            Jugador = Default;
            // menu de nuevo jugador 
        }
    }
    public List<int> GetDeck()
    {
        return Jugador.IDCartasMazo;
    }
    public List<Progreso> GetProgress()
    {
        return Jugador.Progresos;
    }
    public List<DIDCarta> GetCards()
    {
        return Jugador.IDCartasDisponibles;
    }
    public void SaveDeck(List<int> IDCartas)
    {
        // Guatdar Mazo
        Jugador.IDCartasMazo = IDCartas;
        SavePlayer();
    }
    public void SaveName(string Nombre)
    {
        // Guatdar Mazo
        Jugador.Nombre = Nombre;
        SavePlayer();
    }
    public void SaveCards(List<DIDCarta> IDCartasDisponibles)
    {
        // Guatdar Mazo
        Jugador.IDCartasDisponibles = IDCartasDisponibles;
        SavePlayer();
    }
    public void SavePlayer()
    {
        string JSON = JsonUtility.ToJson(Jugador);
        PlayerPrefs.SetString("SavePlayer", JSON);
    }

    public void NewCard(int ID)
    {
        bool bandera = false;
        foreach (var item in Jugador.IDCartasDisponibles)
        {
            if (item.ID == ID)
            {
                item.Cantidad++;
                bandera = true;
                break;
            }
        }
        if (!bandera)
        {
            DIDCarta NewID = new  DIDCarta();
            NewID.ID = ID;
            NewID.Cantidad = 1;
            Jugador.IDCartasDisponibles.Add(NewID);
        }
        SavePlayer();
    }
}
