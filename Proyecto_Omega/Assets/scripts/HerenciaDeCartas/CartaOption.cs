using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CartaOption : CartaPadre
{
    private List<OptionSlot> slots = new List<OptionSlot>();

    private void Start()
    {
        slots.Add(GameObject.Find("Option Slot 1").GetComponent<OptionSlot>());
        slots.Add(GameObject.Find("Option Slot 2").GetComponent<OptionSlot>());
        slots.Add(GameObject.Find("Option Slot 3").GetComponent<OptionSlot>());
    }
    public override void JugarCarta()
    {
        if (GetDoubleClick() && !name.Equals("fuera"))
        {
            foreach (var item in slots)
            {
                if (!item.GetOcupado())
                {
                    name = "fuera";
                    item.SetOcupado(true);
                    transform.position = new Vector3(item.x, item.y, item.z);
                    PosicionDeLasCartas.QuitarCarta();
                    break;
                }
            }
        }
    }
}
