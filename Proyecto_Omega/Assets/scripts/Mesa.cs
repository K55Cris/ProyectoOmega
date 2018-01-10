using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mesa : MonoBehaviour {
    public float x;
    public float y;
    public float z;
    public abstract void Jugar(DragTest carta);
    public abstract void Quitar(DragTest carta);
}
