using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSlot : Slot
{
    public bool AddOrRemove = false;
    private bool Vacio=true;

    public void SetCard(Transform Carta)
    {
    
        if (Vacio)
        {
            Carta.transform.parent = transform;
            Carta.GetComponent<CartaDigimon>().AjustarSlot();
            Vacio = false;
        }
    }
    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
    }
}
