using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class CartaDigimon : Carta {
    //Son los requisitos que deben cumplirse antes de poder evolucionar al digimon.
    private List<string> evolutionRequirement;
    //Es el nivel del digimon (Nvl III, IV, adulto, etc.).

    //Datos de la Carta
    public DigiCarta DatosDigimon;

    public GameObject Front;
    //Las habilidades de support del Digimon. Son las habilidades marcadas con un ■.
    private string abilitieSupport;

    //GaS
    public List<string> EvolutionRequirement
    {
        get
        {
            return evolutionRequirement;
        }

        set
        {
            evolutionRequirement = value;
        }
    }



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
        transform.localRotation= Quaternion.Euler(90,0,0);
      //  Front.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
        Front.GetComponent<Renderer>().material.mainTexture=DataManager.instance.GetTextureDigimon(DatosDigimon.id);
    }
}
