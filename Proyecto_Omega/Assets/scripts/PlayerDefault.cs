using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;

public class PlayerDefault : MonoBehaviour {

    public SavePlayer Default;
    public static PlayerDefault instance;
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
