using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSlot : Slot
{
    public bool AddOrRemove = false;
    private bool Vacio=true;
    public CartaDigimon OpCarta;
    public GameObject CanvasAction;
    public void SetCard(Transform Carta)
    {
    
        if (Vacio)
        {
            PartidaManager.instance.SetMoveCard(this.transform,Carta, StaticRules.Ajustar);
            Vacio = false;
            Cartas.Add(Carta);
            OpCarta = Carta.GetComponent<CartaDigimon>();
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
            }
               
        }
    }

    void OnMouseExit()
    {
        CanvasAction.SetActive(false);
    }
    public void ActivarOPtion()
    {
        OpCarta.Volteo();
    }
}
