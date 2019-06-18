using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DigiCartas;
public class NetOcean : MonoBehaviour
{
    public List<CartaDigimon> Cartas;
    public UnityAction<string> Loaction;
    private Player _jugador= new Player();
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
        Dcard.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Dcard.Mostrar();
        Dcard.transform.localScale = new Vector3(25, 40, 0.015f);
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

    public void Reiniciar(UnityAction<string> LoAction,Player Jugador )
    {
        this.Loaction = LoAction;
        // DigimonBox Slot
        //    List<CartaDigimon> ListDCardDigimon = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evoluciones;
        //    sendDarkarea(ListDCardDigimon);
        _jugador = Jugador;
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot, Jugador).GetComponent<DigimonBoxSlot>().LostDigimon(addDarkArea);

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

    public IEnumerator  Iteracion(List<CartaDigimon> ListaCartas)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);
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
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item3, 0.4f);
        }
        ListaCartas = new List<CartaDigimon>();
    }
    public void addDarkArea(string RESULT)
    {
        Debug.Log(RESULT);
        List<CartaDigimon> ListDCardsDarkArea = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, _jugador).GetComponent<DarkArea>()._Cartas;
        StartCoroutine(Iteracion(ListDCardsDarkArea));
    }

    public void addNetocean(CartaDigimon Dcard)
    {

        if (!Cartas.Contains(Dcard))
        {
            Cartas.Add(Dcard);
            Dcard.mostrar = false;
            StartCoroutine(WhaitFrame(Dcard));
        }

    }
    public void InterAutoAjuste(CartaDigimon carta)
    {
        Vector3 Pos = new Vector3(0, 0, 0 + ((transform.childCount) * 0.06f));
        carta.transform.localScale = new Vector3(1, 1, 0.01f);
        carta.transform.localRotation = new Quaternion(0, 0, 0, 0);
        carta.transform.localPosition = Pos;
        carta.Front.GetComponent<MovimientoCartas>().Mover = false;
        if (Esperar)
        {
            Esperar = false;
            StartCoroutine(WhaitReinicio());
        }
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().Vaciar();
    }
    private bool Esperar = true;
    private void Mezclar()
    {
        
    }
    public IEnumerator WhaitFrame(CartaDigimon Dcard)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        PartidaManager.instance.SetMoveCard(this.transform, Dcard.transform, InterAutoAjuste); 
    }
    public IEnumerator WhaitReinicio()
    {
        yield return new WaitForSeconds(0.5f);
        // Mezclamos Mazo


        if (Loaction != null)
        {
            // barajear mazo 
            SoundManager.instance.PlaySfx(Sound.Barajear);
            int k = 0;
            while (k < 45)
            {
                int val = Random.Range(0, Cartas.Count);
                CartaDigimon _Carta = Cartas[val];
                Cartas.Remove(_Carta);
                Cartas.Add(_Carta);
                k++;
            }
            yield return new WaitForSeconds(1f);
            Loaction("Seguir");
        }
       

        Loaction = null;
        Esperar = true;
    }
}