using System.Collections.Generic;
using UnityEngine;

public class TripleShoot : MonoBehaviour
{

    public Animator Am;
    // Start is called before the first frame update
    public Color32 Selec;
    public Color32 Electric;
    public Color32 SelecMeta;

    public List<ParticleSystem> Energias;
    public List<ParticleSystem> Elictricidad;

    public void Atacar(string Familia)
    {
        switch (Familia)
        {
            case "Nature Spirits":
                Selec = Color.green;
                Electric = Color.yellow;
                break;
            case "Wind Guardians":
                Selec = Color.yellow;
                Electric = Color.green;
                break;
            case "Nightmare Soldiers":
                Selec = new Color32(0, 17, 100, 255);
                Electric = Color.magenta;
                break;
            case "Deep Savers":
                Selec = Color.cyan;
                Electric = Color.white;
                break;
            default:
                Selec = Color.white;
                Electric = Color.white;
                break;
        }
        byte R = Selec.r;
        byte G = Selec.g;
        byte B = Selec.b;


        foreach (var item in Energias)
        {
            var main = item.main;
            main.startColor = new Color(Selec.r / 255f, Selec.g / 255f, Selec.b / 255f, 255);
            item.Play();
        }


        foreach (var item in Elictricidad)
        {
            var main = item.main;
            main.startColor = new Color(Electric.r / 255f, Electric.g / 255f, Electric.b / 255f, 255);
            item.Play();
        }

        Am.Play("BolasEnergia");
    }
}

