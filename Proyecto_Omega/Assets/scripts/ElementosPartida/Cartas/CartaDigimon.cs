using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class CartaDigimon : Carta {
    
    //Es el nivel del digimon (Nvl III, IV, adulto, etc.).

    //Datos de la Carta
    public DigiCarta DatosDigimon;

    public GameObject Front;
    //Las habilidades de support del Digimon. Son las habilidades marcadas con un ■.
    private string abilitieSupport;

    //GaS

    public string AbilitieSupport
    {
        get
        {
            return abilitieSupport;
        }

        set
        {
            abilitieSupport = value;
        }
    }
    public void Hola(string lol)
    {
        Debug.Log("Holi" + lol);
    }
    public void Mostrar()
    {
      // transform.localRotation= Quaternion.Euler(90,0,0);
      //  Front.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));

        Front.GetComponent<Renderer>().material.mainTexture=DataManager.instance.GetTextureDigimon(DatosDigimon.id);
    }
    public void AjustarSlot()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.localScale = new Vector3(1, 1, 0.015f);
    }
}
