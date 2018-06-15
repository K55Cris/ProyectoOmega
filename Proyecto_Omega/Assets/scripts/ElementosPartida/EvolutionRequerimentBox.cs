using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolutionRequerimentBox : Slot
{
    public bool Activado = false;
    public GameObject Canvas;
    public Transform X, O;
    public List<string> ListaRequerimientos;
    public List<Transform> ListaRequerimientosAdicionales = new List<Transform>();
    public List<Transform> ListaXO = new List<Transform>();
    public UnityEvent lol;

    public void GetX()
    {
        Debug.Log(StaticRules.NowPhase);
        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase)
        {
            JalarCartaNetOcean(X);
        }

    }
    public void GetO()
    {
        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase)
        {
            JalarCartaNetOcean(O);
        }
    }
    public void Requerimientos()
    {
        StaticRules.CheckSetDigiCardSlot(MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox) ,null);
    }

    public void SetRequerimientos()
    {
        Cartas = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas;


        for (int i = 0;i < Cartas.Count; i++)
        {
            if (i == 0)
            {
                CodeRequest(Cartas[i],MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta.transform);
                Debug.Log(Cartas[i] + ""+i);
            }
            else
            {
                Debug.Log(Cartas[i] + ""+i);
                CodeRequest(Cartas[i], Cartas[i-1]);
            }
            
        }
        StaticRules.NowEvolutionPhase = StaticRules.EvolutionPhase.SecondRequerimient;

    }

    public void CodeRequest(Transform DEvo, Transform Base)
    {
        List<string> requerimientos = StaticRules.GetListRequerimentsDigimon(DEvo.GetComponent<CartaDigimon>().DatosDigimon,
            Base.GetComponent<CartaDigimon>().DatosDigimon);

        if (requerimientos.Count > 0)
        {
            foreach (var item in requerimientos)
            {
                if (item == "O")
                {
                    JalarCartaNetOcean(O);
                    Canvas.SetActive(false);

                }
                else if (item == ("X"))
                {
                    JalarCartaNetOcean(X);
                    Canvas.SetActive(false);
                }
                else
                {
                    ListaRequerimientos.Add(item);

                }
            }
            Activado = true;
        }
    }
    public void PlayAnimationEvolution(int phase)
    {
        
    }

    public void SetAdicionalRequiriment(Transform Carta)
    {
        foreach (var item in ListaRequerimientos)
        {
            if (Carta.GetComponent<CartaDigimon>().DatosDigimon.Nombre.Contains(item))
            {
                PartidaManager.instance.SetMoveCard(this.transform, Carta);
                ListaRequerimientosAdicionales.Add(Carta);
                Carta.GetComponent<CartaDigimon>().AjustarSlot();
                StartCoroutine(QuitarDeListaRequerimientos(item));
                SoundManager.instance.PlaySfx(Sound.SetCard);
            }
        }
    }
   public IEnumerator QuitarDeListaRequerimientos(string request)
    {
        yield return new WaitForEndOfFrame();
        ListaRequerimientos.Remove(request);
    }

    void OnMouseDown()
    {
        if(!Activado)
        Canvas.SetActive(true);
    }
    void OnMouseExit()
    {
        Canvas.SetActive(false);
    }
    public void JalarCartaNetOcean(Transform Slot)
    {
        if (MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean).childCount > 0)
        {
            Transform _carta = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean).GetChild(0);
            ListaXO.Add(_carta);
            PartidaManager.instance.SetMoveCard(Slot, _carta);
            _carta.GetComponent<CartaDigimon>().AjustarSlot();
            SoundManager.instance.PlaySfx(Sound.SetCard);
        }

    }
   
}