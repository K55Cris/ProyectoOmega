using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSlot : Slot
{
    public bool AddOrRemove = false;
    private bool Vacio=true;

    public void SetCard(Transform Carta)
    {
    
        if (Vacio)
        {
            Carta.transform.parent = transform;
            Carta.GetComponent<CartaDigimon>().AjustarSlot();
            Vacio = false;
            Cartas.Add(Carta);
            SoundManager.instance.PlaySfx(Sound.SetCard);
            if (StaticRules.NowPhase==StaticRules.Phases.PreparationPhase)
               StaticRules.NowPreparationPhase=StaticRules.PreparationPhase.SetOptionCard;
        }
    }
    private void OnMouseDown()
    {
        if (!Vacio)
        {
      
            if(StaticRules.NowPreparationPhase < StaticRules.PreparationPhase.SetOptionCard)
            StaticRules.NowPreparationPhase = StaticRules.PreparationPhase.ActivarOption;
        }
    }
}
