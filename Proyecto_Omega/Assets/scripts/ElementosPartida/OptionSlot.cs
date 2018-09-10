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
    public void SetCard(Transform Carta)
    {
    
        if (Vacio)
        {
          
            Vacio = false;
            OpCarta = Carta.GetComponent<CartaDigimon>();
            PartidaManager.instance.SetMoveCard(this.transform, OpCarta.transform, StaticRules.Ajustar);
            OpCarta.Front.GetComponent<MovimientoCartas>().Mover = false;
            SoundManager.instance.PlaySfx(Sound.SetCard);
            if (StaticRules.NowPhase==StaticRules.Phases.PreparationPhase)
               StaticRules.NowPreparationPhase=StaticRules.PreparationPhase.SetOptionCard;
        }
    }
    private void OnMouseDown()
    {
        if (!Vacio)
        {
            if (StaticRules.NowPhase == StaticRules.Phases.EvolutionPhase)
            {
                CanvasAction.SetActive(true);
                ActivarCarta.SetActive(true);
                DescartarCarta.SetActive(false);
            }
            if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase)
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
        CanvasAction.SetActive(false);
    }
    public void ActivarOPtion()
    {
        OpCarta.Volteo();
    }
    public void Descartar()
    {
        CanvasAction.SetActive(false);
        // Descartar carta 
        if (OpCarta)
        {
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().SetCard(OpCarta.transform);
            Vaciar();
        }
    }
    public void Vaciar()
    {
        OpCarta = null;
        Vacio = true;
    }
}
