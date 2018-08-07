using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class NetOcean : MonoBehaviour
{
    public List<CartaDigimon> Cartas;

    public CartaDigimon Robar()
    {
        CartaDigimon Dcard = Cartas[Cartas.Count - 1];
        Cartas.RemoveAt(Cartas.Count - 1);
        return Dcard;
    }
    public void RobarInteligente()
    {
        CartaDigimon Dcard = Cartas[Cartas.Count - 1];
        Cartas.RemoveAt(Cartas.Count - 1);
        PartidaManager.instance.SetMoveHand(PartidaManager.instance.GetHand(), Dcard.transform, AjustarInterno);
      
    }

    public void AjustarInterno(CartaDigimon Dcard)
    {
        Dcard.transform.localPosition = Vector3.zero;
        Dcard.transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 180));
        Dcard.transform.localScale = new Vector3(25, 40, 0.015f);
        Dcard.Mostrar();
    }


    public CartaDigimon RobarEspesifico(string NombreDigimon)
    {
        foreach (var item in Cartas)
        {
            if (item.GetComponent<CartaDigimon>().DatosDigimon.Nombre == NombreDigimon)
            {
                Cartas.Remove(item);
               
                return item;
            }
        }
        return null;
    }

    public void Reiniciar()
    {
        
        // Dark Arena ---
        List<CartaDigimon> ListDCardsDarkArea = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>()._Cartas;
      //  Iteracion(ListDCardsDarkArea);
        sendDarkarea(ListDCardsDarkArea);
        // EvolutionBox
        List<CartaDigimon> ListDCardsEvolutionBox = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas;
       // Iteracion(ListDCardsEvolutionBox);
        sendDarkarea(ListDCardsEvolutionBox);
        // Requeriments Evolution
        List<CartaDigimon> ListDCardsRequerimientos= MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().Cartas;
       // Iteracion(ListDCardsRequerimientos);
        sendDarkarea(ListDCardsRequerimientos);
        // Support Box
        List<CartaDigimon> ListDCardSuport = MesaManager.instance.GetSlot(MesaManager.Slots.SupportBox).GetComponent<SupportBox>().Cartas;
        //  Iteracion(ListDCardsDarkArea);
        sendDarkarea(ListDCardSuport);
        // Option slot 1
        List<CartaDigimon> ListDCardOption = MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot1).GetComponent<OptionSlot>().Cartas;
       // Iteracion(ListDCardOption);
        sendDarkarea(ListDCardOption);
        // Option slot 2
        List<CartaDigimon> ListDCardOption2 = MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot2).GetComponent<OptionSlot>().Cartas;
       // Iteracion(ListDCardOption2);
        sendDarkarea(ListDCardOption2);
        // Option slot 3
        List<CartaDigimon> ListDCardOption3 = MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot3).GetComponent<OptionSlot>().Cartas;
      //  Iteracion(ListDCardOption3);
        sendDarkarea(ListDCardOption3);
        // DigimonBox Slot
        List<CartaDigimon> ListDCardDigimon = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evoluciones;
        sendDarkarea(ListDCardDigimon);
        // Mano   --------------------------
        List<CartaDigimon> ListDCardMano = StaticRules.instance.WhosPlayer._Mano.Cartas;
        Debug.Log("MAno:"+ListDCardMano.Count);
        sendDarkarea(ListDCardMano);
    }
    public void Iteracion(List<CartaDigimon> ListaCartas)
    {
        Debug.Log(ListaCartas.Count);
        foreach (var item in ListaCartas)
        {
            Debug.Log(item.DatosDigimon.Nombre);
            addNetocean(item);
        }
        ListaCartas = new List<CartaDigimon>();
    }
    public void sendDarkarea(List<CartaDigimon> ListaCartas)
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().moviendo = false;
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(addDarkArea);

        Debug.Log(ListaCartas.Count+":");
        foreach (var item3 in ListaCartas)
        {
            Debug.Log(item3.GetComponent<CartaDigimon>().DatosDigimon.Nombre);
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item3.GetComponent<CartaDigimon>(), 0.4f);
        }
        ListaCartas = new List<CartaDigimon>();
    }
    public void addDarkArea(string RESULT)
    {
        List<CartaDigimon> ListDCardsDarkArea = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>()._Cartas;
        Iteracion(ListDCardsDarkArea);
    }

    public void addNetocean(CartaDigimon Dcard)
    {
        if (!Cartas.Contains(Dcard))
        {
            Cartas.Add(Dcard);
            StartCoroutine(WhaitFrame(Dcard));
        }
    }
    public void InterAutoAjuste(CartaDigimon carta)
    {
        Vector3 Pos = new Vector3(0, 0, 0 + ((transform.childCount) * 0.010f));
        carta.transform.localScale = new Vector3(1, 1, 0.01f);
        carta.transform.localRotation = new Quaternion(0, 0, 0, 0);
        carta.transform.localPosition = Pos;
        carta.Front.GetComponent<MovimientoCartas>().Mover = false;
    }
    private void Mezclar()
    {

    }
    public IEnumerator WhaitFrame(CartaDigimon Dcard)
    {
        yield return new WaitForEndOfFrame();
        PartidaManager.instance.SetMoveCard(this.transform, Dcard.transform, InterAutoAjuste);
        yield return new WaitForEndOfFrame();
    }
}