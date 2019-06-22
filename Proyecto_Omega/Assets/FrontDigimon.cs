using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FrontDigimon : MonoBehaviour {
    public Image Digimon;
    public Image Ataque;
    public List<Sprite> Ataques;
    public ContadorOffencivo Contador;
    public List<TextMeshProUGUI> NameAtaques;
    public List<TextMeshProUGUI> PoderAtaques;
    // Use this for initialization
    public void Start()
    {
        QuitarDigimon();

    }
    public void RevelarDigimon(CartaDigimon Dcard, string ataque)
    {
        Digimon.color = Color.white;
        Ataque.color = Color.white;
        Digimon.sprite = DataManager.instance.GetSprite8(Dcard.DatosDigimon.id);
        switch (ataque)
        {
            case "A":
                Ataque.overrideSprite = Ataques[0];

                break;
            case "B":
                Ataque.overrideSprite = Ataques[1];
                break;
            case "C":
                Ataque.overrideSprite = Ataques[2];
                break;
        }
        SetAtackNames(Dcard);

        if (MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta==Dcard)
        {
            PartidaManager.instance.Player1Atack = ataque;
            MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot, PartidaManager.instance.Player1).
                GetComponent<DigimonBoxSlot>().CanvasContador.OpEfectCard(StaticRules.instance.WhatAtackUse(ataque, Dcard));
        }
        else
        {
            PartidaManager.instance.Player2Atack = ataque;
            MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot, PartidaManager.instance.Player2).
              GetComponent<DigimonBoxSlot>().CanvasContador.OpEfectCard(StaticRules.instance.WhatAtackUse(ataque, Dcard));
        }


    }
    public void QuitarDigimon()
    {
        StartCoroutine(Whaith());
    }
    public IEnumerator Whaith()
    {

        Digimon.color = new Color32(255, 255, 255, 0);
        Ataque.color = new Color32(255, 255, 255, 0);
        yield return new WaitForEndOfFrame();
    }
    public void SetAtackNames(CartaDigimon Dcard)
    {
        NameAtaques[0].text = Dcard.DatosDigimon.NombreAtaqueA;
        NameAtaques[1].text = Dcard.DatosDigimon.NombreAtaqueB;
        NameAtaques[2].text = Dcard.DatosDigimon.NombreAtaqueC;
        PoderAtaques[0].text = Dcard.DatosDigimon.DanoAtaqueA.ToString();
        PoderAtaques[1].text = Dcard.DatosDigimon.DanoAtaqueB.ToString();
        PoderAtaques[2].text = Dcard.DatosDigimon.DanoAtaqueC.ToString();
    }
    public void BuffAtack(int cantidad, CartaDigimon Dcard)
    {
        int poderA = Convert.ToInt32(PoderAtaques[0].text);
        int poderB = Convert.ToInt32(PoderAtaques[1].text);
        int poderC = Convert.ToInt32(PoderAtaques[2].text);
        PoderAtaques[0].text = (poderA + cantidad).ToString();
        PoderAtaques[1].text = (poderB + cantidad).ToString();
        PoderAtaques[2].text = (poderC + cantidad).ToString();
        if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
        {
            Contador.EFECTOS(StaticRules.instance.WhatAtackUse(PartidaManager.instance.Player1Atack, Dcard) + cantidad, null);
        }
        else
        {
            Contador.EFECTOS(StaticRules.instance.WhatAtackUse(PartidaManager.instance.Player2Atack, Dcard) + cantidad, null);
        }
    }
    public void DuplicatePower(int multiplicador, CartaDigimon Dcard)
    {
        Debug.Log("Doble powa");
        int poderA = Convert.ToInt32(PoderAtaques[0].text);
        int poderB = Convert.ToInt32(PoderAtaques[1].text);
        int poderC = Convert.ToInt32(PoderAtaques[2].text);
        PoderAtaques[0].text = (poderA * multiplicador).ToString();
        PoderAtaques[1].text = (poderB * multiplicador).ToString();
        PoderAtaques[2].text = (poderC * multiplicador).ToString();
        if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
        {
            Contador.EFECTOS(StaticRules.instance.WhatAtackUse(PartidaManager.instance.Player1Atack, Dcard)*multiplicador, null);
        }
        else
        {
            Contador.EFECTOS(StaticRules.instance.WhatAtackUse(PartidaManager.instance.Player2Atack, Dcard) * multiplicador, null);
        }
    }
}
