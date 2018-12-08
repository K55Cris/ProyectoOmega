using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportBox : Slot
{
    public bool Ocupado = false;
    public CartaDigimon DCard;

    public void NowPhase()
    {
        switch (StaticRules.instance.NowPhase)
        {
            case DigiCartas.Phases.PreparationPhase:
                PreparationPhase();
                break;
            case DigiCartas.Phases.EvolutionPhase:
                EvolutionPhase();
                break;
            case DigiCartas.Phases.EvolutionRequirements:
                EvolutionRequirements();
                break;
            case DigiCartas.Phases.FusionRequirements:
                EvolutionRequirements();
                break;
            case DigiCartas.Phases.AppearanceRequirements:
                FusionRequirements();
                break;
            default:
                Debug.Log("Fase no recocida para este Slot");
                break;

        }
    }
    void OnMouseDown()
    {
        Debug.Log("Support Slot");
    }
    public void PreparationPhase()
    {

    }
    public void EvolutionPhase()
    {

    }
    public void EvolutionRequirements()
    {

    }
    public void FusionRequirements()
    {

    }
    public void AppearanceRequirements()
    {

    }
    public void SetDigimon(Transform Carta)
    {
        if (Carta.GetComponent<CartaDigimon>().DatosDigimon.IsSupport
        && MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.codigo == 
        Carta.GetComponent<CartaDigimon>().DatosDigimon.codigo)
        {
            if (!Ocupado)
            {
                PartidaManager.instance.SetMoveCard(this.transform, Carta, StaticRules.Ajustar);
                DCard= Carta.GetComponent<CartaDigimon>();
                Ocupado = true;
            }
        }
    }
}