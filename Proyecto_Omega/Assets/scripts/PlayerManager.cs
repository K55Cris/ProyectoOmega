using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;
    public SavePlayer Jugador;
    public List<DIDCarta> IDCartasDisponibles;

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
}
