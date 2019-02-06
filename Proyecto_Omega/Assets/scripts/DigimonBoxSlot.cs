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
    public TornadoAtaque Tornado;
    public ContadorOffencivo CanvasContador;
    public Animator Camera;
    public List<CartaDigimon> Evoluciones= new List<CartaDigimon>();

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
        if (_DigiCarta)
        {
            if (_DigiCarta.mostrar)
            {
                VentanaMoreInfo.instance.Show(_DigiCarta.DatosDigimon);
            }

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
                if (DataManager.GetDesicion())
                {
                  AtaqueA.Play();
                  SoundManager.instance.PlaySfx(Sound.AtaqueA);
                  
                }
                else
                {
                    Tornado.Atacar(_DigiCarta.DatosDigimon.Familia);
                    SoundManager.instance.PlaySfx(Sound.Tornado);
                }
               
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

        if (Evoluciones.Count == 0)
        {
            if (Loaction != null)
            {
                Loaction("Solo esta el Roquin");
                return;
            }
        }
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, MesaManager.instance.
        WhatSlotPlayer(this.transform, MesaManager.Slots.DigimonSlot)).GetComponent<DarkArea>().GetComponent<DarkArea>().setAction(Loaction);

        foreach (var item in Evoluciones)
        {
            // mandamos a la dark area las evoluciones del jugador perdedor
             item.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().DestruirCarta();


            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea,MesaManager.instance.
                WhatSlotPlayer(this.transform,MesaManager.Slots.DigimonSlot)).GetComponent<DarkArea>().AddListDescarte(item, 0.5f);
        }
        Evoluciones.Clear();
        _DigiCarta = DRoquin;
    }
    public void DeEvolution(UnityAction<string> Loaction, string Condicion)
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, MesaManager.instance.
        WhatSlotPlayer(this.transform, MesaManager.Slots.DigimonSlot)).GetComponent<DarkArea>().setAction(Loaction);

        if (Evoluciones.Count == 0)
        {
            if (Loaction != null)
            {
                Loaction("Solo esta el Roquin");
            }
            else
            {
                return;
            }
          
        }
        // aplicamos condicion
        string[] sub = Condicion.Split(' ');
        string NivelRequerido="";
        string NivelObjetivo="";

        if (sub[1] == "to")
        {
            NivelRequerido = sub[0];
            NivelObjetivo = sub[2];
        }


        if (_DigiCarta.DatosDigimon.Nivel == NivelRequerido)
        {
            List<CartaDigimon> ReturnEvo = new List<CartaDigimon>();
            // EL DIGIMON en posicion si es del nivel requerido
            foreach (var item in Evoluciones)
            {
                if (item.DatosDigimon.Nivel != NivelObjetivo)
                {
                    if (StaticRules.instance.WhatNivelMayor(item.DatosDigimon.Nivel, NivelObjetivo))
                    {
                        // mandamos a la dark area las cartas con nivel superior 
                        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, MesaManager.instance.WhatSlotPlayer(this.transform, MesaManager.Slots.DigimonSlot)
                            ).GetComponent<DarkArea>().AddListDescarte(item, 0.5f);
                        ReturnEvo.Add(item);
                    }
                }
                else
                {
                    _DigiCarta = item;
                }
            }
            foreach (var item in ReturnEvo)
            {
                if (Evoluciones.Contains(item))
                    Evoluciones.Remove(item);
            }
            ReturnEvo.Clear();
            _DigiCarta.Volteo();
            // revelamos la carta
            MesaManager.instance.GetSlot(MesaManager.Slots.frontSlot, MesaManager.instance.
            WhatSlotPlayer(this.transform, MesaManager.Slots.DigimonSlot)).GetComponent<FrontDigimon>().RevelarDigimon(_DigiCarta, PartidaManager.
            instance.GetAtackUse(MesaManager.instance.WhatSlotPlayer(this.transform, MesaManager.Slots.DigimonSlot)));
            }
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
                    if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
                    {
                        if (StaticRules.instance.NowPreparationPhase < StaticRules.PreparationPhase.ActivarOption)
                        {
                            StaticRules.instance.NowPreparationPhase = StaticRules.PreparationPhase.ChangeDigimon;
                            PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste2);
                            // Quitar Roquin Antiguo
                            PartidaManager.instance.SetMoveCard(MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea), DRoquin.transform, StaticRules.Ajustar);
                            _DigiCarta = Carta.GetComponent<CartaDigimon>();
                            Cambiado = true;
                            _DigiCarta.Front.GetComponent<MovimientoCartas>().Mover = false;
                            DRoquin = Carta.GetComponent<CartaDigimon>();
                            SoundManager.instance.PlaySfx(Sound.SetCard);
                        }
                    }
                    else
                    {
                        PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste2);
                        // Quitar Roquin Antiguo
                        PartidaManager.instance.SetMoveCard(MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea), DRoquin.transform, StaticRules.Ajustar);
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
        if (Evolucion)
        {
            PartidaManager.instance.SetMoveCard(this.transform, Evolucion, InterAutoAjuste);
            _DigiCarta = Evolucion.GetComponent<CartaDigimon>();
            Evolucion.GetComponent<CartaDigimon>().Volteo();
            Evoluciones.Add(Evolucion.GetComponent<CartaDigimon>());
            MesaManager.instance.GetSlot(MesaManager.Slots.frontSlot).GetComponent<FrontDigimon>().SetAtackNames(_DigiCarta);
            StartCoroutine(Quitar(Evolucion));
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
    public IEnumerator Quitar(Transform Carta)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas.Remove(Carta.GetComponent<CartaDigimon>());
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
