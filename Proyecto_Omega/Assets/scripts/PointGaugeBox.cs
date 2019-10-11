using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGaugeBox : MonoBehaviour
{
    public List<Transform> PosPuntuacion;
    public List<pointItem> CanvasPuntuacion;
    public CartaDigimon Dcard;
    public Color Apagado;

    public void Start()
    {
        foreach (var item in CanvasPuntuacion)
        {
            item.Apagar(Apagado);
        }
    }
    public void SetCard(int point)
    {
        Debug.Log(transform.parent.name + point);
        Transform Padre = null;
        switch (point)
        {
            case 100:
                Padre = PosPuntuacion[9];
                break;
            case 90:
                Padre = PosPuntuacion[8];
                break;
            case 80:
                Padre = PosPuntuacion[7];
                break;
            case 70:
                Padre = PosPuntuacion[6];
                break;
            case 60:
                Padre = PosPuntuacion[5];
                break;
            case 50:
                Padre = PosPuntuacion[4];
                break;
            case 40:
                Padre = PosPuntuacion[3];
                break;
            case 30:
                Padre = PosPuntuacion[2];
                break;
            case 20:
                Padre = PosPuntuacion[1];
                break;
            case 10:
                Padre = PosPuntuacion[0];
                break;
        }

        foreach (var item in CanvasPuntuacion)
        {
            int k = Convert.ToInt32(item.name);
            if (k > point)
            {
                item.Apagar(Apagado);
            }
        }

        if (point <= 0)
        {
            StaticRules.Victori();
        }
        else
        {
            PartidaManager.instance.SetMoveCard(Padre, Dcard.transform, StaticRules.Ajustar);
        }
    }
    public void iniciar()
    {
        StartCoroutine(Progresivo(CanvasPuntuacion));
    }
    public IEnumerator Progresivo(List<pointItem> lista)
    {
        foreach (var item in lista)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.2f);
            item.Prender();
        }
    }
}
