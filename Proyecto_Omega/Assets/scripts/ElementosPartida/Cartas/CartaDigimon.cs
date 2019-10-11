using DigiCartas;
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
        //  Front.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
        // Front.GetComponent<Renderer>().material.shader = Shader.Find("_BaseMap");
        Front.GetComponent<Renderer>().material.SetTexture("_BaseMap", DataManager.instance.GetTextureDigimon(DatosDigimon.id));
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
