using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {
    public bool mulligan = false, robar = true;
	void Update () {
        if (robar)//PosicionDeLasCartas.GetCantidadActualDeCartas() < PosicionDeLasCartas.GetCantidadTotalDeCartas() /* && (inicio de su turno || despues del mulligan)*/)
        {
            EventManager.TriggerEvent("RobarYAcomodarEnMano");
        }
        if (!mulligan)
        {
            EventManager.TriggerEvent("RobarRookie");
            EventManager.TriggerEvent("JugarCarta");
        }
        if (true/*esta en la face gamesetup*/)
        {
            //EventManager.TriggerEvent("QuitarCarta");
        }
        if (mulligan)
        {
            EventManager.TriggerEvent("StartMulliganCarta");
            EventManager.TriggerEvent("SeleccionCarta");
            EventManager.TriggerEvent("AcomodarCartas");
        }
    }
}
