using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
using UnityEngine.Events;
public class DigimonBoxSlot : MonoBehaviour {
    public bool Cambiado = false;
    public CartaDigimon _DigiCarta;
    public GameObject AuraEvolucion;
    public GameObject EnergiaEvolucion;
    public ParticleSystem DEbuff;
    public ParticleSystem AtaqueA;
    public ParticleSystem AtaqueB;
    public ContadorOffencivo CanvasContador;

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
                FusionRequirements();
                break;
            case StaticRules.Phases.BattlePhase:

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
    public void BattelPhase(int Ataque, UnityAction<string> LoUnityAction = null)
    {
        CanvasContador.gameObject.SetActive(true);
        CanvasContador.Empezar(Ataque, LoUnityAction);
    }
    public void IniciarAtaque()
    {
        switch (PartidaManager.instance.WhoAtackUse(this.transform))
        {
            case "A":
                AtaqueA.Play();
                SoundManager.instance.PlaySfx(Sound.AtaqueA);
                break;

            case "B":
                AtaqueB.Play();
                SoundManager.instance.PlaySfx(Sound.AtaqueB);
                break;
            case "C":
                Animator Am = GetComponent<Animator>();
                Am.Play("AtacarDigimonBoxSlot1");
                break;
            default:
                break;
        }
      // avisar que el point calculation a terminado

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
                        PartidaManager.instance.SetMoveCard(this.transform,Carta,InterAutoAjuste);
                      
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
            PartidaManager.instance.SetMoveCard(this.transform, Carta,InterAutoAjuste);
            _DigiCarta = Carta.GetComponent<CartaDigimon>();
            SoundManager.instance.PlaySfx(Sound.SetCard);
        }
     

    }
    public void Evolution(Transform Evolucion)
    {
        PartidaManager.instance.SetMoveCard(this.transform,Evolucion,InterAutoAjuste);
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

    public void InterAutoAjuste(CartaDigimon carta)
    {
        StartCoroutine(AutoAjustar(carta.transform));
    }

    public IEnumerator AutoAjustar(Transform Carta)
{
    yield return new WaitForEndOfFrame();
    Carta.localPosition = new Vector3(0, 0, -100 + ((transform.childCount-4)*500));
}
}
