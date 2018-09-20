using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DigiCartas;
public class NetOcean : MonoBehaviour
{
    public List<CartaDigimon> Cartas;
    public UnityAction<string> Loaction;
    public CartaDigimon Robar()
    {
        if (Cartas.Count == 0)
            return null;
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

    public void Reiniciar(UnityAction<string> LoAction)
    {
        this.Loaction = LoAction;
        // Dark Arena ---
        List<CartaDigimon> ListDCardsDarkArea = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>()._Cartas;
      //  Iteracion(ListDCardsDarkArea);
        sendDarkarea(ListDCardsDarkArea);
     
        // DigimonBox Slot
        List<CartaDigimon> ListDCardDigimon = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evoluciones;
        sendDarkarea(ListDCardDigimon);
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().LostDigimon(null);

    }

    public void ListaCartasOptionSlots(List<CartaDigimon> ListDCards, Transform Slot)
    {
        CartaDigimon OpCard = Slot.GetComponent<OptionSlot>().GetOpCard();
        if (OpCard)
        {
            ListDCards.Add(OpCard);
            Slot.GetComponent<OptionSlot>().Vacio = true;
        }
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
        Vector3 Pos = new Vector3(0, 0, 0 + ((transform.childCount) * 0.030f));
        carta.transform.localScale = new Vector3(1, 1, 0.01f);
        carta.transform.localRotation = new Quaternion(0, 0, 0, 0);
        carta.transform.localPosition = Pos;
        carta.Front.GetComponent<MovimientoCartas>().Mover = false;
        if (Loaction != null)
        {
            Loaction("Seguir");
            Loaction = null;
        }
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().Vaciar();
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