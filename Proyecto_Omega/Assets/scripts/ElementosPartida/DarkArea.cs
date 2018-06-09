using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DigiCartas;

public class DarkArea : MonoBehaviour {

    public List<Carta> Cartas;
    public List<Transform> DigiCartas = new List<Transform>();
    public bool moviendo = false;
    public UnityAction<string> TermineDescarte;

    public void setAction(UnityAction<string> funcion)
    {
        this.TermineDescarte = funcion;
    }

    public void Vaciar()
    {
        Cartas = new List<Carta>();
    }
    public void SetCard(Transform Carta)
    {
        Carta.transform.parent = transform;
        Carta.GetComponent<CartaDigimon>().AjustarSlot();
        Carta.localPosition = new Vector3(0, 0, (-100 - transform.childCount));
    }
    public void AddListDescarte(Transform Dcarta, float segundos)
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
                item.transform.parent = transform;
                item.GetComponent<CartaDigimon>().AjustarSlot();
                SoundManager.instance.PlaySfx(Sound.SetCard);
                if (item.GetComponent<CartaDigimon>().DatosDigimon.id!=7)
                item.GetComponent<CartaDigimon>().Mostrar();

                item.localPosition = new Vector3(0, 0, (-100 - transform.childCount));
                yield return new WaitForSeconds(segundos);
            }
            if (TermineDescarte != null)
            {
                TermineDescarte("Completado");
                moviendo = false;
                DigiCartas = new List<Transform>();
                TermineDescarte = null;
            }
        }
      
    } 
 
}
