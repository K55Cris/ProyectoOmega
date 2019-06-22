using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSlot : Slot
{
    public bool AddOrRemove = false;
    public bool Vacio=true;
    public CartaDigimon OpCarta;
    public GameObject CanvasAction;
    public GameObject ActivarCarta;
    public GameObject DescartarCarta;
    public int TurnOpcard = 0;
    public void SetCard(Transform Carta)
    {
        if (Vacio)
        {
          
            Vacio = false;
            OpCarta = Carta.GetComponent<CartaDigimon>();
            PartidaManager.instance.SetMoveCard(this.transform, Carta, StaticRules.Ajustar);
            OpCarta.Front.GetComponent<MovimientoCartas>().Mover = false;
            SoundManager.instance.PlaySfx(Sound.SetCard);
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
            {
                if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer&& Tutorial.instance.NowSelectTuto!= Tutorial.TutoStates.Evolucion) 
                {
                    Tutorial.instance.Iniciar();
                }
            }

            if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
            {
                if (StaticRules.instance.NowPhase == DigiCartas.Phases.PreparationPhase)
                {

                    StaticRules.instance.NowPreparationPhase = StaticRules.PreparationPhase.SetOptionCard;
                }
            }
        }
    }
    private void OnMouseDown()
    {
     
        if (!Vacio && PartidaManager.instance.Player1 == MesaManager.instance.WhatSlotPlayer(this.transform, MesaManager.Slots.OptionSlot1))
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
            {
                VentanaMoreInfo.instance.Show(OpCarta.DatosDigimon);
            }
                if (StaticRules.instance.NowPhase == DigiCartas.Phases.EvolutionPhase || StaticRules.instance.NowPhase == DigiCartas.Phases.BattlePhase || StaticRules.instance.NowPhase == DigiCartas.Phases.OptionBattlePhase)
                {
                    CanvasAction.SetActive(true);
                    ActivarCarta.SetActive(true);
                    DescartarCarta.SetActive(false);
                }
                if (StaticRules.instance.NowPhase == DigiCartas.Phases.PreparationPhase || StaticRules.instance.NowPhase == DigiCartas.Phases.PreparationPhase2)
                {
                    CanvasAction.SetActive(true);
                    DescartarCarta.SetActive(true);
                    ActivarCarta.SetActive(false);
                }

        }
        
    }

    public CartaDigimon GetOpCard()
    {
        return OpCarta;
    }
    void OnMouseExit()
    {
        if (CanvasAction)
        CanvasAction.SetActive(false);
    }
    public void ActivarOPtion()
    {
        if (OpCarta)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
            {
                if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
                {
                    Tutorial.instance.Iniciar();
                }
            }
            CanvasAction.SetActive(false);
            StaticRules.ActivateOptionCard(OpCarta);
            TurnOpcard = 0;
        }
    }
    public void Descartar()
    {
        if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
        {
            CanvasAction.SetActive(false);
        }
        // Descartar carta 
        if (OpCarta)
        {
          
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().SetCard(OpCarta.transform);
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
            {
                if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
                {
                    Tutorial.instance.Iniciar();
                }
            }
            Vaciar();
        }
    }
    public void Vaciar()
    {
        OpCarta = null;
        Vacio = true;
        TurnOpcard = 0;
      
    }
}
