using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionDeLasCartas : MonoBehaviour {
    public int cantidadTotalDeCartas = 6;
    private static int cantidadActualDeCartas = 0;

    public static void Renombrar()
    {
        string nombre = "Carta1";
        bool marca = false;
        for (int i = 1; i <= cantidadActualDeCartas+1; i++)
        {
            if (marca)
            {
                GameObject.Find("Carta" + i).name = nombre;
                marca = false;
            }
            if (GameObject.Find("Carta" + i) == null)
            {
                nombre = "Carta" + i;
                marca = true;
            }
        }
    }
    public static void Acomodar()
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
        Acomodar();
    }
    public static void QuitarCarta()
    {
        cantidadActualDeCartas--;
        Renombrar();
        Acomodar();
    }
    public static int GetCantidadActualDeCartas()
    {
        return cantidadActualDeCartas;
    }
}
