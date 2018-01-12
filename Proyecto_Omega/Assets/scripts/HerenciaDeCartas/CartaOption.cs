using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CartaOption : CartaPadre
{
    private List<OptionSlot> slots = new List<OptionSlot>();
    private int lugar = 0;

    private void Start()
    {
        slots.Add(GameObject.Find("Option Slot 1").GetComponent<OptionSlot>());
        slots.Add(GameObject.Find("Option Slot 2").GetComponent<OptionSlot>());
        slots.Add(GameObject.Find("Option Slot 3").GetComponent<OptionSlot>());
    }
    public override void JugarCarta()
    {
        if (GetDoubleClick() && (!name.Equals("OptionSlot" + lugar)))
        {
            int i = 1;
            foreach (var item in slots)
            {
                if (!item.GetOcupado())
                {
                    name = "OptionSlot" + i;
                    item.SetOcupado(true);
                    lugar = i;
                    transform.position = new Vector3(item.x, item.y, item.z);
                    PosicionDeLasCartas.QuitarCarta2();
                    SetDoubleClick(false);
                    break;
                }
                i++;
            }
        }
    }

    public override void QuitarCarta()
    {
        if (GetDoubleClick() && name.Equals("OptionSlot" + lugar))
        {
            GameObject.Find("Option Slot " + lugar).GetComponent<OptionSlot>().SetOcupado(false);
            DarkArea.instance.Meter(/*id*/GetComponent<Renderer>().material.mainTexture.name);
            SetDoubleClick(false);
            Destroy(this.gameObject);
        }
    }
}
