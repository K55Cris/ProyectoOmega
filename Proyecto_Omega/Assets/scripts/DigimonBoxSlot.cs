using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class DigimonBoxSlot : MonoBehaviour {
    public bool Cambiado = false;
    
    public void NowPhase()
    {
        switch (StaticRules.NowPhase)
        {
            case StaticRules.Phases.PreparationPhase:
                PreparationPhase();
                break;
            case StaticRules.Phases.EvolutionPhase:
                EvolutionPhase();
                break;
            case StaticRules.Phases.EvolutionRequirements:
                EvolutionRequirements();
                break;
            case StaticRules.Phases.FusionRequirements:
                EvolutionRequirements();
                break;
            case StaticRules.Phases.AppearanceRequirements:
                FusionRequirements();
                break;
            default:
                Debug.Log("Fase no recocida para este Slot");
                break;

        }
    }
    void OnMouseDown()
    {
        Debug.Log("Digimon Slot");
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
        // Verificamos si hay Otro Digimon en el slot
        if (transform.childCount > 0)
        {
            
            Transform Child = transform.GetChild(transform.childCount-1);
            if (Child.GetComponent<CartaDigimon>().DatosDigimon.Nivel== "III")
            {
                if (!Cambiado)
                { 
                    Carta.transform.parent = transform;
                    Carta.GetComponent<CartaDigimon>().AjustarSlot();
                    StartCoroutine(AutoAjustar(Carta));
                    StaticRules.CheckSetDigiCardSlot(MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea), Child);
                    Cambiado = true;
                }
            }
        }
        else
        {
            Carta.transform.parent = transform;
            Carta.GetComponent<CartaDigimon>().AjustarSlot();
            StartCoroutine(AutoAjustar(Carta));
        }
     

    }
      

public IEnumerator AutoAjustar(Transform Carta)
{
    yield return new WaitForEndOfFrame();
    Carta.localPosition = new Vector3(0, 0, -100 - transform.childCount);
}
}
