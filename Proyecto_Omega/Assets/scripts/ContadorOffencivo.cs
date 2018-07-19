using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ContadorOffencivo : MonoBehaviour
{
    public Text Ataque;
    public int AtaqueBase = 0;
    public int PoderDeAtaque=0;
    // Use this for initialization
    public void Empezar(int Daño,UnityAction<string> LoAction=null)
    {
        AtaqueBase = Daño;
        StartCoroutine(EstablecerCantidad(Daño, LoAction));
    }

    public void EFECTOS(int cantidad, UnityAction<string> LoAction)
    {
        StartCoroutine(EstablecerCantidad(cantidad,LoAction));
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
            Ataque.color = Color.green;
            for (int i = PoderDeAtaque; i < cantidad; i+=5)
            {
                Ataque.text = i.ToString();
                yield return new WaitForSecondsRealtime(0.05f);
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
            Ataque.color = Color.red;
         
            Debug.Log(PoderDeAtaque+":"+transform.parent.transform.parent.transform.parent.name);
            int DIVISOR = PoderDeAtaque / 60;
            for (int i = PoderDeAtaque; i > cantidad; i-=DIVISOR)
            {
                Ataque.text = i.ToString();
                yield return new WaitForSecondsRealtime(0.05f);
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
