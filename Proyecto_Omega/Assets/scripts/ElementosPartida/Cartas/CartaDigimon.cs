using DigiCartas;
using System.Collections.Generic;
using UnityEngine;
public class CartaDigimon : Carta
{

    //Es el nivel del digimon (Nvl III, IV, adulto, etc.).
    public int cardNumber;
    public int CardNumber
    {
        get
        {
            return cardNumber;
        }

        set
        {
            cardNumber = value;
        }
    }
    //Datos de la Carta
    public DigiCarta DatosDigimon = new DigiCarta();
    public bool AddOrRemove;
    public GameObject Front;
    public bool mostrar = false;
    public GameObject Funda;
    public DarkArea DarkArea;
    public List<Habilidades> Habilidades;
    public Player _jugador;
    //Las habilidades de support del Digimon. Son las habilidades marcadas con un ■.
    private string abilitieSupport;




    public void Mostrar()
    {
        //  Front.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
        // Front.GetComponent<Renderer>().material.shader = Shader.Find("_BaseMap");
        Front.GetComponent<Renderer>().material.SetTexture("_BaseMap", DataManager.instance.GetTextureDigimon(DatosDigimon.id));

        GetHab(DatosDigimon.IDHabilidad);
    }

    public void GetHab(List<string> _Habilidad)
    {
        Habilidades.Clear();
        foreach (var item in _Habilidad)
        {
            Habilidades newH = DigiCartas.Habilidades.Sky;
            newH = 0;

            SetHabiliti(ref newH, item);
            Habilidades.Add(newH);
        }

    }

    public void SetHabiliti(ref Habilidades NHabiliti,string _Habilidad)
    {
        switch (_Habilidad)
        {
            case "Heal":
                NHabiliti = DigiCartas.Habilidades.Heal;
                break;
            case "Sky":
                NHabiliti = DigiCartas.Habilidades.Sky;
                break;
            case "Underwater":
                NHabiliti = DigiCartas.Habilidades.Underwater;
                break;
            case "Underground":
                NHabiliti = DigiCartas.Habilidades.underground;
                break;
            case "DiscardDarkOponetWin":
                NHabiliti = DigiCartas.Habilidades.DiscardDarkOponetWin;
                break;
            case "MismoDestino":
                NHabiliti = DigiCartas.Habilidades.MismoDestino;
                break;
        }
    }



    void OnMouseDown()
    {
        if (StaticRules.instance.NowPhase == DigiCartas.Phases.DiscardPhase && transform.parent.name.Contains("Option Slot"))
        {
            AddOrRemove = !AddOrRemove;
            StaticRules.instance.AddListDiscard(gameObject, AddOrRemove);
            Front.GetComponent<MovimientoCartas>().CanvasSeleted.SetActive(AddOrRemove);

        }
    }
    public void Volteo()
    {
        mostrar = true;
        transform.localRotation = Quaternion.Euler(180, 0, 0);
    }

    public void AjustarSlot()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.localScale = new Vector3(1, 1, 0.015f);

    }

    public void Destruir()
    {
        // La carta es mandada a su respectiva Dark Area ( se destruye cartas cuyo propisito es solo ir a la dark area
        DarkArea.AddListDescarte(this, 0.5f, true);
    }

}
