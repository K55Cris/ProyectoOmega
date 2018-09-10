using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DigiCartas;

public class DarkArea : MonoBehaviour {

    public List<CartaDigimon> DigiCartas = new List<CartaDigimon>();
    public List<CartaDigimon> _Cartas = new List<CartaDigimon>();
    public bool moviendo = false;
    public UnityAction<string> TermineDescarte;

    public void setAction(UnityAction<string> funcion)
    {
        this.TermineDescarte = funcion;
    }

    public void Vaciar()
    {
        DigiCartas = new List<CartaDigimon>();
        _Cartas = new List<CartaDigimon>();
    }
    public void SetCard(Transform Carta)
    {
        SoundManager.instance.PlaySfx(Sound.SetCard);
        _Cartas.Add(Carta.GetComponent<CartaDigimon>());
        PartidaManager.instance.SetMoveCard(this.transform, Carta, InterAutoAjuste);
    }


    public void AddListDescarte(CartaDigimon Dcarta, float segundos)
    {
        if(Dcarta)
        DigiCartas.Add(Dcarta);

        StartCoroutine(MoviendiaDarkArea(segundos));
    }

    public IEnumerator MoviendiaDarkArea(float segundos)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(segundos);
        if (!moviendo)
        {
            moviendo = true;
            foreach (var item in DigiCartas)
            {
                if (!_Cartas.Contains(item))
                {
                    _Cartas.Add(item);
                    PartidaManager.instance.SetMoveCard(this.transform, item.transform, InterAutoAjuste);
                    SoundManager.instance.PlaySfx(Sound.SetCard);
                    if (item.DatosDigimon.id != 7)
                        item.Mostrar();
                    item.transform.localPosition = new Vector3(0, 0, (-100 - transform.childCount));
                    yield return new WaitForSeconds(segundos);
                }
            }
            yield return new WaitForSeconds(segundos);
            yield return new WaitForEndOfFrame();
            if (TermineDescarte != null)
            {
                TermineDescarte("Completado");
                moviendo = false;
                DigiCartas = new List<CartaDigimon>();
                TermineDescarte = null;
            }
            else
            {
                moviendo = false;
                DigiCartas = new List<CartaDigimon>();
                TermineDescarte = null;
            }
        }
      
    }

    public void InterAutoAjuste(CartaDigimon carta)
    {
        Vector3 Pos = new Vector3(0, 0, 0 - ((transform.childCount - 4) * 50));
        carta.transform.localRotation = new Quaternion(0, 0, 0,0);
        carta.transform.localPosition = Pos;
        carta.transform.localScale= new Vector3(1, 1, 0.01f);
        carta.Front.GetComponent<MovimientoCartas>().disebleCard();
    }
 
}
