using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGauge : Mesa
{
    //public AnimationClip pierdeVida = new AnimationClip();
    private void OnMouseDown()
    {
        Jugar();
    }
    public override void Jugar(DragTest carta)
    {

    }
    public void Jugar()
    {
        //pierdeVida.SetCurve("", typeof(Transform), "localPosition.y", new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.1f, 0 + 0.2f), new Keyframe(0.2f, 0)));

    }

    public override void Quitar(DragTest carta)
    {
        throw new System.NotImplementedException();
    }
}
