using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mesa : MonoBehaviour {
    public abstract void Jugar(DragTest carta);
    public abstract void Quitar(DragTest carta);
}
