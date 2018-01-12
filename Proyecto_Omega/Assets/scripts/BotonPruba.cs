using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotonPruba : MonoBehaviour, IPointerClickHandler
{
    private string a = "Listo", b = "Activar Mulligan", aux;
    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent("Set");
        //cambia el texto asi nomas
        GameObject.Find("Text").GetComponent<Text>().text = a;
        aux = a;
        a = b;
        b = aux;
        EventManager.TriggerEvent("MulliganCarta");
        EventManager.TriggerEvent("StopMulliganCarta");
        EventManager.TriggerEvent("Roba"); //Ubicacion EventTrigger
    }
}
