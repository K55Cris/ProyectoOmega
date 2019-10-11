using DigiCartas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DeckItem : MonoBehaviour
{
    public TextMeshProUGUI Nombre;
    public Image Tipo;
    public DigiCarta _Datos;
    public bool lleno = false;
    private Color32 Trasparente = new Color32(255, 225, 255, 0);

    public void Llenar(DigiCarta Datos)
    {
        Nombre.text = Datos.Nombre;
        Tipo.overrideSprite = DataManager.instance.GetSpriteForType(Datos);
        Tipo.color = Color.white;
        lleno = true;
        _Datos = Datos;
    }
    public void Resetear()
    {
        if (lleno)
        {
            Nombre.text = "Empty Slot";
            Tipo.color = Trasparente;
            lleno = false;
            DeckManager.instance.QuitDeck(_Datos);
        }
    }

}
