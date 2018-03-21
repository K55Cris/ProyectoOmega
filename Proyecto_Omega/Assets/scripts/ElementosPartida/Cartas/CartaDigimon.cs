using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class CartaDigimon : Carta {
    
    //Es el nivel del digimon (Nvl III, IV, adulto, etc.).

    //Datos de la Carta
    public DigiCarta DatosDigimon;
    public bool AddOrRemove;
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

   
    public void Mostrar()
    {
      // 
      //  Front.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));

        Front.GetComponent<Renderer>().material.mainTexture=DataManager.instance.GetTextureDigimon(DatosDigimon.id);
    }
    void OnMouseDown()
    {
        if (StaticRules.NowPhase == StaticRules.Phases.DiscardPhase && transform.parent.name.Contains("Option Slot"))
        {
            AddOrRemove = !AddOrRemove;
            StaticRules.instance.AddListDiscard(gameObject, AddOrRemove);
            Front.GetComponent<MovimientoCartas>().CanvasSeleted.SetActive(AddOrRemove);
                
        }
    }
    public void Volteo()
    {
        transform.localRotation = Quaternion.Euler(180, 0, 0);
    }
    public void AjustarSlot()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.localScale = new Vector3(1, 1, 0.015f);
    }
   
}
