using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventTrigger : MonoBehaviour {
    public bool mulligan = false, jugar = true, robar = false;
    public static bool inicio = true;
    public Button boton;
    //esto se pasa a rules
	void Update () {
        if (jugar && !inicio)//PosicionDeLasCartas.GetCantidadActualDeCartas() < PosicionDeLasCartas.GetCantidadTotalDeCartas() /* && (inicio de su turno || despues del mulligan)*/)
        {
            EventManager.TriggerEvent("JugarCarta"); //Ubicacion CartaPadre
        }
        if (inicio)
        {
            EventManager.TriggerEvent("RobarRookie"); //Ubicacion Rookie
        }
        if (robar)
        {
            EventManager.TriggerEvent("RobarYAcomodarEnMano"); //Ubicacion Deck
        }
        if (true/*esta en la face gamesetup*/)
        {
            EventManager.TriggerEvent("AcomodarCartas"); //Ubicacion PosicionDeLasCartas
            //EventManager.TriggerEvent("QuitarCarta"); //Ubicacion CartaPadre
        }
        if (mulligan)
        {
            EventManager.TriggerEvent("StartMulliganCarta"); //Ubicacion CartaPadre
            EventManager.TriggerEvent("SeleccionCarta"); //Ubicacion CartaPadre
        }
    }



    //Solo temporal hasta pasar todo en limpio en rules
    private UnityAction setListener;
    private UnityAction botonListener;
    private UnityAction robaListener;
    private void Awake()
    {
        setListener = new UnityAction(Set);
        botonListener = new UnityAction(Botonazo);
        robaListener = new UnityAction(Roba);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Set", setListener);
        EventManager.StartListening("Botonazo", botonListener);
        EventManager.StartListening("Roba", robaListener);
    }

    public void Set()
    {
        this.mulligan = !this.mulligan;
        this.jugar = !this.jugar;
    }
    public void Roba()
    {
        this.robar = !this.robar;
    }
    public void Botonazo()
    {
        boton.interactable = true;
    }
}
