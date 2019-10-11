using DigiCartas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ColeccionableItem : MonoBehaviour
{
    public Image Imagen;
    public TextMeshProUGUI Nombre;
    private Coleccionables _datos;
    public void CargarData(Coleccionables Datos)
    {
        _datos = Datos;
        Imagen.sprite = Datos.Image;
        Nombre.text = Datos.Nombre;
        Nombre.color = Datos.Rareza;
    }
    public void ChangeSelec()
    {
        DeckManager.instance.ChangeData(_datos);
    }
}
