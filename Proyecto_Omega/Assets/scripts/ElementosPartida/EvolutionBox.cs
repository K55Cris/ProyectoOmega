using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EvolutionBox : Slot
{
    public GameObject Canvas;
    public void NowPhase()
    {
        switch (StaticRules.NowPhase)
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
        Debug.Log("Evolution slot");
        NowPhase();
    }
    void OnMouseExit()
    {
        Canvas.SetActive(false);
    }
    public void PreparationPhase()
    {

    }
    public void EvolutionPhase()
    {
     //   Canvas.SetActive(true);
     //   Cartas.Remove(Cartas[0]);
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

        if (Carta.GetComponent<CartaDigimon>().DatosDigimon.Nivel != "III"&& StaticRules.CheckEvolutionList(Carta.GetComponent<CartaDigimon>()))
        {
                if (StaticRules.NowPreparationPhase < StaticRules.PreparationPhase.ActivarOption)
                {
                    StaticRules.NowPreparationPhase = StaticRules.PreparationPhase.SetEvolition;
                    PartidaManager.instance.SetMoveCard(this.transform, Carta, StaticRules.Ajustar);
                    Cartas.Add(Carta.GetComponent<CartaDigimon>());
                    Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
                    SoundManager.instance.PlaySfx(Sound.SetCard);
                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(true);
                }   
        }
        else if (Cartas.Count>0)
        {
            if (StaticRules.NowPreparationPhase < StaticRules.PreparationPhase.ActivarOption)
            {
                // obtener evolucion
                CartaDigimon Evo = Cartas[Cartas.Count-1].GetComponent<CartaDigimon>();
                foreach (string item in Carta.transform.GetComponent<CartaDigimon>().DatosDigimon.ListaRequerimientos)
                {
                    string nameDigi = Evo.DatosDigimon.Nombre.ToUpper().Trim().Replace(" ", "");
                    if (item.Contains(nameDigi))
                    {
                        PartidaManager.instance.SetMoveCard(this.transform, Carta,StaticRules.Ajustar);
                        Cartas.Add(Carta.GetComponent<CartaDigimon>());
                        Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
                        SoundManager.instance.PlaySfx(Sound.SetCard);
                        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(true);

                    }
                    Debug.Log(nameDigi);
                }
            }
        }  
    }
}