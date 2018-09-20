using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour {

    private void OnEnable()
    {
        TerminarGirar();
    }

    public void TerminarGirar()
    {
        WhoIsPlayer1.instance.Desicion();
    }
}
