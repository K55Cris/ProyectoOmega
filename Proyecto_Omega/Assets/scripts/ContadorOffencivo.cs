using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class ContadorOffencivo : MonoBehaviour
{
    public TextMeshProUGUI Ataque;
    public int AtaqueBase = 0;
    public int PoderDeAtaque=0;

    // Use this for initialization
    public void Empezar(int Daño,UnityAction<string> LoAction=null)
    {
        AtaqueBase = Daño;
        PartidaManager.instance.CambioDePhase(false);
        StartCoroutine(EstablecerCantidad(Daño, LoAction));
    }
    public void Endphase()
    {
        PoderDeAtaque = 0;
        Ataque.color = Color.white;
        this.gameObject.SetActive(false);
    }

    public void EFECTOS(int cantidad, UnityAction<string> LoAction)
    {
        Debug.Log(cantidad + ":" + PoderDeAtaque);
        StartCoroutine(EstablecerCantidad(cantidad,LoAction));
    }

    public void OpEfectCard(int cantidad)
    {
        PoderDeAtaque = cantidad;
    }

    public void EsperarEfectos()
    {
      
    }

    private IEnumerator EstablecerCantidad(int cantidad, UnityAction<string> loaction=null)
    {
        yield return new WaitForEndOfFrame();
        if (cantidad>PoderDeAtaque)
        {
            // Buffo
            Debug.Log("bUFFO");
            int DIVISOR = cantidad / 50;
            for (int i = PoderDeAtaque; i < cantidad; i+=DIVISOR)
            {
                Ataque.text = i.ToString();
                yield return new WaitForSecondsRealtime(0.008f);
            }
            PoderDeAtaque = cantidad;
            Ataque.text = cantidad.ToString();

            if (loaction != null)
            {
                loaction.Invoke("Aumento de Poder");
            }
        }
        else
        {
            // De-duff
            Debug.Log(PoderDeAtaque+":"+transform.parent.transform.parent.transform.parent.name);
            int DIVISOR = PoderDeAtaque / 60;
            for (int i = PoderDeAtaque; i > cantidad; i-=DIVISOR)
            {
                Ataque.text = i.ToString();
                yield return new WaitForSecondsRealtime(0.01f);
            }
            PoderDeAtaque = cantidad;
            Ataque.text = cantidad.ToString();

            if (loaction != null)
            {
                loaction.Invoke("De-Buff a habilidad");
            }
        }
     
           
    }
}
