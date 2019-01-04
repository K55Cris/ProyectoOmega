using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoAtaque : MonoBehaviour {
    public Transform Padre;
    public Color32 Selec;
    public Color32 SelecMeta;
    public List<ParticleSystem> TornadoPize;
    public ParticleSystem TornadoInterior;
    private Vector3 PosOriginal;
    // Use this for initialization
    public void Start()
    {
        PosOriginal = Padre.position;
    }

    public void Atacar(string Familia)
    {
        switch (Familia)
        {
            case "Nature Spirits":
                Selec = Color.green;
                break;
            case "Wind Guardians":
                Selec = Color.yellow;
                break;
            case "Nightmare Soldiers":
                Selec = new Color32(0, 17, 100, 255);
                break;
            case "Deep Savers":
                Selec = Color.cyan;
                break;
            default:
                Selec = Color.white;
                break;
        }
        byte R = Selec.r;
        byte G = Selec.g;
        byte B = Selec.b;
        for (int i = 0; i < 1; i++)
        {
            R--;
            G--;
            B--;
        }
        SelecMeta = new Color32(R,G,B, 200);

        foreach (var item in TornadoPize)
        {
            var main = item.main;
            main.startColor = new Color(Selec.r/255f,Selec.g/255f,Selec.b/255f,255);
            item.Play();
        }
        var main2 = TornadoInterior.main;
        main2.startColor = new Color(SelecMeta.r/255F, SelecMeta.g/255F, SelecMeta.b/255F, 255);
        TornadoInterior.Play();
        StartCoroutine(Transicion());
    }
    public IEnumerator Transicion()
    {
        yield return new WaitForEndOfFrame();
        // Movimiento
        Vector3 Mover = this.transform.position;
        var heading = Mover - Padre.position;
        var distance = heading.magnitude;
        while (distance > 5)
        {
            heading = Mover - Padre.position;
            distance = heading.magnitude;
            float step = 25*Time.deltaTime;
            Padre.position = Vector3.MoveTowards(Padre.position, Mover, step);
            yield return new WaitForSecondsRealtime(0.01F);
        }
        Invoke("ResetZero", 3.5F);

    }
    public void ResetZero()
    {
        Padre.transform.position = PosOriginal;
    }
}
