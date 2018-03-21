using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;

public class DarkArea : MonoBehaviour {

    public List<Carta> Cartas;
    public void Vaciar()
    {
        Cartas = new List<Carta>();
    }
    public void SetCard(Transform Carta)
    {
        Carta.transform.parent = transform;
        Carta.GetComponent<CartaDigimon>().AjustarSlot();
        Carta.localPosition = new Vector3(0, 0, (-100 - transform.childCount));
    }
 
}
