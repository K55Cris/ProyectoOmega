using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionRequerimentBox : Slot
{
    public bool Activado = true;
    public GameObject Canvas;
    public Transform X, O;
    public List<string> ListaRequerimientos;
    public List<CartaDigimon> ListaRequerimientosAdicionales = new List<CartaDigimon>();
    public List<CartaDigimon> ListaXO = new List<CartaDigimon>();
    public bool Colocar = false;

    public void NowPhase()
    {
        switch (StaticRules.instance.NowPhase)
        {

            case DigiCartas.Phases.PreparationPhase:
                PreparationPhase();
                break;
            case DigiCartas.Phases.PreparationPhase2:
                PreparationPhase();
                break;
            default:
                Activado = true;
                break;

        }
    }

    private void PreparationPhase()
    {
        Activado = false;
    }
    public void EndEvolution()
    {
        ListaRequerimientos.Clear();
        ListaXO.Clear();
        ListaRequerimientosAdicionales.Clear();
    }


    public void Requerimientos()
    {
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
                    SetRequerimientos(null);
                    if (Tutorial.instance.NowDigiEfectos == Tutorial.DigiEfectos.None)
                    {
                        Tutorial.instance.Iniciar();
                    }

                }

            }
        }
        else
        {
            PartidaManager.instance.CambioDePhase(false);
            SetRequerimientos(null);
        }
    }

    public void SetRequerimientos(CartaDigimon Carta, bool condicion = false)
    {
        if (!Carta && condicion == true)
            EndEvolution();

        if (condicion)
            EndEvolution();

        Cartas = new List<CartaDigimon>();
        Cartas = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas;


        for (int i = 0; i < Cartas.Count; i++)
        {
            if (i == 0)
            {
                CodeRequest(Cartas[i], MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta, Carta, condicion);
            }
            else
            {

                CodeRequest(Cartas[i], Cartas[i - 1], Carta, condicion);
            }

        }

    }


    public void CodeRequest(CartaDigimon DEvo, CartaDigimon Base, CartaDigimon SetCard, bool condicion = false)
    {

        List<string> requerimientos = StaticRules.GetListRequerimentsDigimon(DEvo.DatosDigimon, Base.DatosDigimon);


        if (requerimientos.Count > 0)
        {
            bool Adds = false;
            if (SetCard)
            {
                Adds = true;
            }
            foreach (var item in requerimientos)
            {

                if (item == "O")
                {
                    if (!condicion)
                    {
                        Canvas.SetActive(false);
                        if (!SetCard)
                        {
                            Adds = true;
                            JalarCartaNetOcean(O, null);

                        }

                        else
                        {
                            Adds = true;
                            JalarCartaNetOcean(O, SetCard);
                            break;
                        }

                    }
                    else
                    {
                        if (!Adds)
                            ListaRequerimientos.Add("O");
                    }

                }
                else if (item == ("X"))
                {
                    if (!condicion)
                    {
                        Canvas.SetActive(false);
                        if (!SetCard)
                        {
                            Adds = true;
                            JalarCartaNetOcean(X, null);
                        }

                        else
                        {
                            Adds = true;
                            JalarCartaNetOcean(X, SetCard);
                            break;

                        }

                    }
                    else
                    {
                        if (!Adds)
                            ListaRequerimientos.Add("X");
                    }
                }
                else
                {
                    if (condicion)
                    {
                        if (!Adds)
                        {
                            ListaRequerimientos.Add(item);
                        }
                    }
                    else
                    {
                        if (Adds)
                            CheckRequeriment(SetCard, item);
                    }
                }
            }
            Activado = true;

        }
    }
    public void CheckRequeriment(CartaDigimon RCard, string item)
    {
        if (!MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().IsNameCardOver(RCard))
        {
            if (StaticRules.isDigimonOrChip(RCard.DatosDigimon))
            {
                string name = RCard.DatosDigimon.Nombre;
                string ShapeName = name.Substring(name.Length - 4, 3);
                if (item.Contains(ShapeName))
                {
                    PartidaManager.instance.SetMoveCard(this.transform, RCard.transform, StaticRules.Ajustar);
                    ListaRequerimientosAdicionales.Add(RCard);
                    StartCoroutine(QuitarDeListaRequerimientos(item));
                    SoundManager.instance.PlaySfx(Sound.SetCard);

                }
            }
            else
            {
                string name = RCard.DatosDigimon.Nombre.Replace(" ", "");
                if (item == name.ToUpper())
                {
                    PartidaManager.instance.SetMoveCard(this.transform, RCard.transform, StaticRules.Ajustar);
                    ListaRequerimientosAdicionales.Add(RCard);
                    StartCoroutine(QuitarDeListaRequerimientos(item));
                    SoundManager.instance.PlaySfx(Sound.SetCard);
                }
            }
        }
    }

    public void SetAdicionalRequiriment(Transform Carta)
    {
        foreach (var item in ListaRequerimientos)
        {
            Debug.Log(item);
            bool saltar = false;
            if (Carta.GetComponent<CartaDigimon>().DatosDigimon.Nombre.ToUpper().Contains(item.ToUpper()))
            {

                foreach (var item2 in MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox, StaticRules.instance.WhosPlayer).GetComponent<EvolutionBox>().Cartas)
                {
                    if (item2.DatosDigimon.Nombre == item)
                        saltar = true;
                    break;
                }
                if (!saltar)
                {
                    PartidaManager.instance.SetMoveCard(O, Carta, StaticRules.Ajustar);
                    ListaRequerimientosAdicionales.Add(Carta.GetComponent<CartaDigimon>());
                    StartCoroutine(QuitarDeListaRequerimientos(item));
                    SoundManager.instance.PlaySfx(Sound.SetCard);
                    break;
                }
            }
        }
        /*foreach (var item in ListaRequerimientos)
        {
        
            if (item.Contains("X"))
            {
                PartidaManager.instance.SetMoveCard(X, Carta, StaticRules.Ajustar);
                ListaXO.Add(Carta);
                StartCoroutine(QuitarDeListaRequerimientos(item));
                SoundManager.instance.PlaySfx(Sound.SetCard);
                ListaRequerimientos.Remove(item);
                break;
            }
            else if (item.Contains("O"))
            {
                PartidaManager.instance.SetMoveCard(O, Carta, StaticRules.Ajustar);
                ListaXO.Add(Carta);
                StartCoroutine(QuitarDeListaRequerimientos(item));
                ListaRequerimientos.Remove(item);
                SoundManager.instance.PlaySfx(Sound.SetCard);
                break;
            }
        }
        */

    }
    public IEnumerator QuitarDeListaRequerimientos(string request)
    {
        yield return new WaitForEndOfFrame();
        //  int index = ListaRequerimientos.FindIndex(x => x==request);
        ListaRequerimientos.Remove(request);
    }

    void OnMouseDown()
    {
        NowPhase();

        if (!Activado)
            Canvas.SetActive(true);
    }
    void OnMouseExit()
    {
        Canvas.SetActive(false);
    }
    public void JalarCartaNetOcean(Transform Slot, CartaDigimon DCard)
    {
        if (ListaRequerimientos.Contains(Slot.name))
        {
            if (!DCard)
            {
                CartaDigimon _carta = new CartaDigimon();
                if (MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean).GetComponent<NetOcean>().Cartas.Count > 0)
                {
                    _carta = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean).GetComponent<NetOcean>().Robar();
                }
                if (_carta)
                {
                    ListaXO.Add(_carta);
                    _carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = false;
                    SoundManager.instance.PlaySfx(Sound.SetCard);
                    PartidaManager.instance.SetMoveCard(Slot, _carta.transform, AjusteIntermedio);
                    StartCoroutine(QuitarDeListaRequerimientos(Slot.name));
                }
            }
            else
            {
                CartaDigimon _carta = new CartaDigimon();
                _carta = DCard;
                ListaXO.Add(_carta);
                _carta.Front.GetComponent<MovimientoCartas>().Mover = false;
                SoundManager.instance.PlaySfx(Sound.SetCard);
                PartidaManager.instance.SetMoveCard(Slot, DCard.transform, AjusteIntermedio);
                StartCoroutine(QuitarDeListaRequerimientos(Slot.name));
            }
        }
    }
    public void AjusteIntermedio(CartaDigimon _DCard)
    {
        _DCard.AjustarSlot();
        if (StaticRules.instance.WhosPlayer != IA.instance.IAPlayer)
            PartidaManager.instance.CambioDePhase(true);
    }

}