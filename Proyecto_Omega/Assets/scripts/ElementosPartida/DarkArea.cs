using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DigiCartas;

public class DarkArea : MonoBehaviour {

    public List<CartaDigimon> DigiCartas = new List<CartaDigimon>();
    public bool moviendo = false;
    public UnityAction<string> TermineDescarte;

    public void setAction(UnityAction<string> funcion)
    {
        this.TermineDescarte = funcion;
    }

    public void Vaciar()
    {
    DigiCartas = new List<CartaDigimon>();
    }
    public void SetCard(Transform Carta)
    {
        PartidaManager.instance.SetMoveCard(this.transform, Carta);
        Carta.GetComponent<CartaDigimon>().AjustarSlot();
        Carta.localPosition = new Vector3(0, 0, (-100 - transform.childCount));
    }
    public void AddListDescarte(CartaDigimon Dcarta, float segundos)
    {
        DigiCartas.Add(Dcarta);
        StartCoroutine(MoviendiaDarkArea(segundos));
    }

    public IEnumerator MoviendiaDarkArea(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        yield return new WaitForEndOfFrame();
        if (!moviendo)
        {
            moviendo = true;
            foreach (var item in DigiCartas)
            {
                PartidaManager.instance.SetMoveCard(this.transform,item.transform);
                item.AjustarSlot();
                SoundManager.instance.PlaySfx(Sound.SetCard);
                if (item.DatosDigimon.id!=7)
                item.Mostrar();

                item.transform.localPosition = new Vector3(0, 0, (-100 - transform.childCount));
                yield return new WaitForSeconds(segundos);
            }
            if (TermineDescarte != null)
            {
                TermineDescarte("Completado");
                moviendo = false;
                DigiCartas = new List<CartaDigimon>();
                TermineDescarte = null;
            }
        }
      
    } 
 
}
