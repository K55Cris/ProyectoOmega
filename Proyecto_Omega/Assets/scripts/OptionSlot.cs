using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSlot : Mesa
{
    private bool ocupado = false;
    public override void Jugar(DragTest carta)
    {
            this.ocupado = true;
            carta.SetEstoyEnMesa(true);
            carta.transform.SetParent(this.transform.parent.parent);
            carta.transform.position = new Vector3(x, y, z);
    }

    public override void Quitar(DragTest carta)
    {
        Destroy(carta.gameObject);
        this.SetOcupado(false);
    }

    //Getters and Setters
    public void SetOcupado(bool ocupado)
    {
        this.ocupado = ocupado;
    }
    public bool GetOcupado()
    {
        return this.ocupado;
    }

}
