using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TESTpUREBAMOVIMIENTOSLOTS : MonoBehaviour {

    public CartaDigimon Carta;
    public Transform Slot;


    public void Ajustar(Transform Padre, CartaDigimon LoCard)
    { 

        // se llama cuando la carta a llegado a su destino :V
        LoCard.transform.SetParent(Padre);
       
    }
}
