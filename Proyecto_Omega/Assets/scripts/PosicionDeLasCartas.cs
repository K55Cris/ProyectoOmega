using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PosicionDeLasCartas : MonoBehaviour {
    public static int cantidadTotalDeCartas = 6;
    private static int cantidadActualDeCartas = 0;
    private UnityAction acomodarListener;

    private void Awake()
    {
        acomodarListener = new UnityAction(AcomodarCartas);
    }
    private void OnEnable()
    {
        EventManager.StartListening("AcomodarCartas", acomodarListener);
    }
    public static void Renombrar(int nro)
    {
        for (int i = nro; i <= GetCantidadActualDeCartas(); i++)
        {
            GameObject.Find("Carta" + (i + 1)).name = "Carta" + i;
        }
    }
    public static void AcomodarCartas()
    {
        switch (cantidadActualDeCartas)
        {
            case 1:
                GameObject.Find("Carta1").transform.position = new Vector3(250, 90, 46);
                break;
            case 2:
                GameObject.Find("Carta1").transform.position = new Vector3(220, 90, 46);
                GameObject.Find("Carta2").transform.position = new Vector3(280, 90, 46);
                break;
            case 3:
                GameObject.Find("Carta1").transform.position = new Vector3(190, 90, 46);
                GameObject.Find("Carta2").transform.position = new Vector3(250, 90, 46);
                GameObject.Find("Carta3").transform.position = new Vector3(310, 90, 46);
                break;
            case 4:
                GameObject.Find("Carta1").transform.position = new Vector3(160, 90, 46);
                GameObject.Find("Carta2").transform.position = new Vector3(220, 90, 46);
                GameObject.Find("Carta3").transform.position = new Vector3(280, 90, 46);
                GameObject.Find("Carta4").transform.position = new Vector3(340, 90, 46);
                break;
            case 5:
                GameObject.Find("Carta1").transform.position = new Vector3(130, 90, 46);
                GameObject.Find("Carta2").transform.position = new Vector3(190, 90, 46);
                GameObject.Find("Carta3").transform.position = new Vector3(250, 90, 46);
                GameObject.Find("Carta4").transform.position = new Vector3(310, 90, 46);
                GameObject.Find("Carta5").transform.position = new Vector3(370, 90, 46);
                break;
            case 6:
                GameObject.Find("Carta1").transform.position = new Vector3(100, 90, 46);
                GameObject.Find("Carta2").transform.position = new Vector3(160, 90, 46);
                GameObject.Find("Carta3").transform.position = new Vector3(220, 90, 46);
                GameObject.Find("Carta4").transform.position = new Vector3(280, 90, 46);
                GameObject.Find("Carta5").transform.position = new Vector3(340, 90, 46);
                GameObject.Find("Carta6").transform.position = new Vector3(400, 90, 46);
                break;
            default:
                break;
        }
    }

    public static void AumentarCarta()
    {
        cantidadActualDeCartas++;
        AcomodarCartas();
    }
    public static void QuitarCarta(int nro)
    {
        cantidadActualDeCartas--;
        Renombrar(nro);
    }
    public static int GetCantidadActualDeCartas()
    {
        return cantidadActualDeCartas;
    }
    public static int GetCantidadTotalDeCartas()
    {
        return cantidadTotalDeCartas;
    }
}
