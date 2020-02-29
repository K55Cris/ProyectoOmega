using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MultiplayerItem : MonoBehaviour
{
    public Image photo;
    public TextMeshProUGUI Nombre, Nivel;

    // Start is called before the first frame update
    public void Load(string user, string _nivel, Sprite Foto,bool reset2=false)
    {
        photo.sprite = Foto;
        photo.color = Color.white;
        Nombre.text = user;
        Nivel.text = _nivel;
    }

    public void Reset(string user, Sprite Foto, int bandera)
    {
        photo.sprite = Foto;

        Nombre.text = user;
        Nivel.text = "";
        switch (bandera)
        {
            case 0:
                photo.color = new Color32(0,0,0,0);
                break;
            case 1:
                photo.color = Color.black;
                break;
            default:
                break;
        }
           
    }
}
