using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;
    public Player Jugador;
    public List<DIDCarta> IDCartasDisponibles;

    private void Awake()
    {
        instance = this;
    }
}
