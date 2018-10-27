using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
using UnityEngine.Events;
public class DigimonBoxSlot : MonoBehaviour {
    public bool Cambiado = false;
    public CartaDigimon _DigiCarta;
    public CartaDigimon DRoquin;
    public GameObject AuraEvolucion;
    public GameObject EnergiaEvolucion;
    public ParticleSystem DEbuff;
    public ParticleSystem AtaqueA;
    public ParticleSystem AtaqueB;
    public ParticleSystem Joggres;
    public ContadorOffencivo CanvasContador;
    public Animator Camera;
    public List<CartaDigimon> Evoluciones= new List<CartaDigimon>();

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
                FusionRequirements();
                break;
            case DigiCartas.Phases.EndPhase:
                EndPhase();
                break;
            default:
                Debug.Log("Fase no recocida para este Slot");
                break;

        }
    }

    void OnMouseDown()
    {
        if (_DigiCarta.mostrar)
        {
            VentanaMoreInfo.instance.Show(_DigiCarta.DatosDigimon);
        }
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
    public void EndPhase()
    {
        CanvasContador.Endphase();
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
        Camera.Play("BatallaphaseCam");
    }
    public void LostDigimon(UnityAction<string> Loaction)
    {
        MesaManager.instance.Campo1.DarkArea.GetComponent<DarkArea>().setAction(Loaction);
        if (Evoluciones.Count == 0)
        {
            if (Loaction != null)
            {
                Loaction("Solo esta el Roquin");
            }
        }  
        foreach (var item in Evoluciones)
        {
            // mandamos a la dark area las evoluciones del jugador perdedor
           // item.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().DestruirCarta();
            MesaManager.instance.Campo1.DarkArea.GetComponent<DarkArea>().AddListDescarte(item, 0.5f);
        }
        _DigiCarta = DRoquin;
    }
    
    public void SetDigimon(Transform Carta)
    {
        // Verificamos si hay Otro Digimon en el slot
        if (_DigiCarta)
        {
            if (_DigiCarta.DatosDigimon.Nivel== "III")
            {
                if (!Cambiado)
                {
                    if (StaticRules.NowPreparationPhase < StaticRules.PreparationPhase.ActivarOption)
                    {
                        StaticRules.NowPreparationPhase = StaticRules.PreparationPhase.ChangeDigimon;
                        PartidaManager.instance.SetMoveCard(this.transform,Carta,InterAutoAjuste2);
                        // Quitar Roquin Antiguo
                        PartidaManager.instance.SetMoveCard(MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea),DRoquin.transform,StaticRules.Ajustar);
                        _DigiCarta = Carta.GetComponent<CartaDigimon>();
                        Cambiado = true;
                        _DigiCarta.Front.GetComponent<MovimientoCartas>().Mover = false;
                        DRoquin = Carta.GetComponent<CartaDigimon>();
                        SoundManager.instance.PlaySfx(Sound.SetCard);

                        
                    }
                }
                else
                {
                    Debug.LogWarning(" El digimon ya fue cambiado");
                }
            }
            else
            {
                Debug.LogWarning(" El digimon No es Roquin");
            }
        }
        else
        {
            PartidaManager.instance.SetMoveCard(this.transform, Carta,InterAutoAjuste2);
            _DigiCarta = Carta.GetComponent<CartaDigimon>();
            DRoquin = Carta.GetComponent<CartaDigimon>();
            _DigiCarta.Front.GetComponent<MovimientoCartas>().Mover = false;
            SoundManager.instance.PlaySfx(Sound.SetCard);
          
        }
    

    }
    public void Evolution(Transform Evolucion)
    {
        Debug.Log(Evolucion.GetComponent<CartaDigimon>().DatosDigimon.Nombre);
        if (Evolucion)
        {
            PartidaManager.instance.SetMoveCard(this.transform, Evolucion, InterAutoAjuste);
            _DigiCarta = Evolucion.GetComponent<CartaDigimon>();
            MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas.Remove(Evolucion.GetComponent<CartaDigimon>());
            Evolucion.GetComponent<CartaDigimon>().Volteo();
            Evoluciones.Add(Evolucion.GetComponent<CartaDigimon>());
            Evoluciones.Add(Evolucion.GetComponent<CartaDigimon>());
            MesaManager.instance.GetSlot(MesaManager.Slots.frontSlot).GetComponent<FrontDigimon>().SetAtackNames(_DigiCarta);
        }
    }

    public void Evolucionar(string Nivel, bool _joggres=false)
    {
        SoundManager.instance.sfxSource.Stop();
        SoundManager.instance.sfxSource.Play();
        if (!_joggres)
        {
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
        else
        {
            // es joggres
            SoundManager.instance.PlaySfx(Sound.Evolucion2);
            Joggres.Play();
        }
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
    public void InterAutoAjuste2(CartaDigimon carta)
    {
        StartCoroutine(AutoAjustarRoquin(carta.transform));
    }

    public IEnumerator AutoAjustar(Transform Carta)
    {
    yield return new WaitForEndOfFrame();
    Carta.localPosition = new Vector3(0, 0, -3+ transform.childCount);
    Carta.transform.localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
    Carta.localScale = new Vector3(1, 1, 0.015f);
    }
    public IEnumerator AutoAjustarRoquin(Transform Carta)
    {
        yield return new WaitForEndOfFrame();
        Carta.localPosition = new Vector3(0, 0, 0);
        Carta.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Carta.localScale = new Vector3(1, 1, 0.015f);
    }
}
