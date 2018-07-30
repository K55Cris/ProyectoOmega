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
        List<CartaDigimon> ListDCards = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().DigiCartas;
        foreach (var item in ListDCards)
        {
            Cartas.Add(item);
            PartidaManager.instance.SetMoveCard(this.transform, item.transform,StaticRules.Ajustar);
        }
        
    }
    public void addNetocean(CartaDigimon Dcard)
    {
        Cartas.Add(Dcard);
        PartidaManager.instance.SetMoveCard(this.transform, Dcard.transform,StaticRules.Ajustar);
   
    }

    private void Mezclar()
    {

    }
}