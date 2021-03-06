using System.Collections;
using UnityEngine;
public class EvolutionBox : Slot
{
    public GameObject Canvas;
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
    public void SetDigimon(Transform Carta, bool IsIgnore = false)
    {
        if (IsIgnore)
        {
            PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste);
            Cartas.Add(Carta.GetComponent<CartaDigimon>());
            Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
            SoundManager.instance.PlaySfx(Sound.SetCard);
        }
        else
        {
            if (Carta.GetComponent<CartaDigimon>().DatosDigimon.Nivel != "III" && StaticRules.CheckEvolutionList(Carta.GetComponent<CartaDigimon>(),
                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta))
            {
                if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
                {
                    if (StaticRules.instance.NowPreparationPhase < StaticRules.PreparationPhase.ActivarOption)
                    {
                        StaticRules.instance.NowPreparationPhase = StaticRules.PreparationPhase.SetEvolition;
                        PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste);
                        Cartas.Add(Carta.GetComponent<CartaDigimon>());
                        Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
                        SoundManager.instance.PlaySfx(Sound.SetCard);
                        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(null, true);
                        return;
                    }
                }
                else
                {
                    PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste);
                    Cartas.Add(Carta.GetComponent<CartaDigimon>());
                    Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
                    SoundManager.instance.PlaySfx(Sound.SetCard);
                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(null, true);
                    return;
                }

            }
            else if (Cartas.Count > 0)
            {
                if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
                {
                    if (StaticRules.instance.NowPreparationPhase < StaticRules.PreparationPhase.ActivarOption)
                    {
                        // obtener evolucion
                        CartaDigimon Evo = Cartas[Cartas.Count - 1].GetComponent<CartaDigimon>();
                        foreach (string item in Carta.transform.GetComponent<CartaDigimon>().DatosDigimon.ListaRequerimientos)
                        {
                            string nameDigi = Evo.DatosDigimon.Nombre.ToUpper().Trim().Replace(" ", "");
                            if (item.Contains(nameDigi))
                            {
                                PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste);
                                Cartas.Add(Carta.GetComponent<CartaDigimon>());
                                Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
                                SoundManager.instance.PlaySfx(Sound.SetCard);
                                MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(null, true);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    // obtener evolucion
                    CartaDigimon Evo = Cartas[Cartas.Count - 1].GetComponent<CartaDigimon>();
                    foreach (string item in Carta.transform.GetComponent<CartaDigimon>().DatosDigimon.ListaRequerimientos)
                    {
                        string nameDigi = Evo.DatosDigimon.Nombre.ToUpper().Trim().Replace(" ", "");
                        if (item.Contains(nameDigi))
                        {
                            PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste);
                            Cartas.Add(Carta.GetComponent<CartaDigimon>());
                            Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
                            SoundManager.instance.PlaySfx(Sound.SetCard);
                            MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(null, true);
                            return;
                        }
                    }
                }
            }
        }

        // El digimon no entro al tuto 

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
            {
                Tutorial.instance.Iniciar();
            }
        }
    }
    public bool IsNameCardOver(CartaDigimon DCarta)
    {
        foreach (var item in Cartas)
        {
            if (item.DatosDigimon.Nombre == DCarta.DatosDigimon.Nombre)
            {
                return true;
            }
        }
        return false;
    }
    public void InterAutoAjuste(CartaDigimon carta)
    {
        StartCoroutine(AutoAjustar(carta.transform));
    }
    public IEnumerator AutoAjustar(Transform Carta)
    {
        yield return new WaitForEndOfFrame();
        Carta.localPosition = new Vector3(0, 0, -1 + Cartas.Count / 2);
        Carta.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Carta.localScale = new Vector3(1, 1, 0.015f);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
            {

                if (Tutorial.instance.NowSelectTuto == Tutorial.TutoStates.Efectos)
                {
                    Tutorial.instance.Iniciar();
                }
                if (Tutorial.instance.NowSelectTuto == Tutorial.TutoStates.Evolucion)
                {
                    if (Tutorial.instance.NowDigiEfectos == Tutorial.DigiEfectos.None)
                    {
                        Tutorial.instance.Iniciar();
                    }
                }
            }
        }
    }
    public void OnBeforeTransformParentChanged()
    {

    }
}