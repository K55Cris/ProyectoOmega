using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class DigimonBoxSlot : MonoBehaviour {
    public bool Cambiado = false;
    public CartaDigimon _DigiCarta;
    public GameObject AuraEvolucion;
    public GameObject EnergiaEvolucion;
    public void Start()
    {
    
    }

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
        if (_DigiCarta)
        {
            DigiCarta Roquin= Carta.GetComponent<CartaDigimon>().DatosDigimon;
            if (Roquin.Nivel== "III")
            {
                if (!Cambiado)
                {
                    if (StaticRules.NowPreparationPhase < StaticRules.PreparationPhase.ActivarOption)
                    {
                        StaticRules.NowPreparationPhase = StaticRules.PreparationPhase.ChangeDigimon;

                        Carta.SetParent(transform);
                        Carta.GetComponent<CartaDigimon>().AjustarSlot();
                        StartCoroutine(AutoAjustar(Carta));
                        StaticRules.CheckSetDigiCardSlot(MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea), Carta);
                        _DigiCarta = Carta.GetComponent<CartaDigimon>();
                        Cambiado = true;
                        SoundManager.instance.PlaySfx(Sound.SetCard);
                    }
                }
            }
        }
        else
        {
            Carta.transform.parent = transform;
            Carta.GetComponent<CartaDigimon>().AjustarSlot();
            _DigiCarta = Carta.GetComponent<CartaDigimon>();
            StartCoroutine(AutoAjustar(Carta));
            SoundManager.instance.PlaySfx(Sound.SetCard);
        }
     

    }
    public void Evolution(Transform Evolucion)
    {
        Evolucion.SetParent(transform);
        Evolucion.GetComponent<CartaDigimon>().AjustarSlot();
        _DigiCarta = Evolucion.GetComponent<CartaDigimon>();
        Evolucion.localPosition = new Vector3(0, 0, 0 + transform.childCount);
        Evolucion.GetComponent<CartaDigimon>().Volteo();
    }

    public void Evolucionar(string Nivel)
    {
        SoundManager.instance.sfxSource.Stop();
        SoundManager.instance.sfxSource.Play();
        switch (Nivel)
        {
            case "IV":
                SoundManager.instance.PlaySfx(Sound.Evolucion);
                break;
            case "Perfect":
                SoundManager.instance.PlaySfx(Sound.Evolucion2);
                break;
            case "Ultimate":
                SoundManager.instance.PlaySfx(Sound.Evolucion2);
                break;
        }
        AuraEvolucion.SetActive(true);
        EnergiaEvolucion.SetActive(true);
    }
    public void TerminarEvolucionar()
    {
        AuraEvolucion.SetActive(false);
        EnergiaEvolucion.SetActive(false);
    }

    public IEnumerator AutoAjustar(Transform Carta)
{
    yield return new WaitForEndOfFrame();
    Carta.localPosition = new Vector3(0, 0, -100 - transform.childCount);
}
}
