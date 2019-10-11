using TMPro;
using UnityEngine;
public class ListaEvolucionesItem : MonoBehaviour
{
    public TextMeshProUGUI Texto;
    // Use this for initialization
    public void Crear(string Request)
    {
        Texto.text = Request;
    }
}
