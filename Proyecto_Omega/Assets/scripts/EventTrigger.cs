using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {
    
	void Update () {
        if (PosicionDeLasCartas.GetCantidadActualDeCartas() < PosicionDeLasCartas.GetCantidadTotalDeCartas() /* && (inicio de su turno || despues del mulligan)*/)
        {
            EventManager.TriggerEvent("RobarYAcomodarEnMano");
        }
	}
}
