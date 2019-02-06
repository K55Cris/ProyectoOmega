using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DigiCartas;

public class DarkArea : MonoBehaviour
{

    public List<CartaDigimon> DigiCartas = new List<CartaDigimon>();
    public List<CartaDigimon> _Cartas = new List<CartaDigimon>();
    public bool moviendo = false;
    public UnityAction<string> TermineDescarte;
    public ViewCards VistaDarkArea;

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
        TermineDescarte = null;
        Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().DestruirCarta();
        SoundManager.instance.PlaySfx(Sound.SetCard);
        _Cartas.Add(Carta.GetComponent<CartaDigimon>());
        StartCoroutine(WhaitFrame(Carta.GetComponent<CartaDigimon>()));
    }


    public void AddListDescarte(CartaDigimon Dcarta, float segundos, bool SinAction = false)
    {
        if (Dcarta)
            DigiCartas.Add(Dcarta);

        if (SinAction)
        {
            TermineDescarte = null;
        }
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
                    item.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().DestruirCarta();
                    _Cartas.Add(item);
                    yield return new WaitForSeconds(segundos);
                    StartCoroutine(WhaitFrame(item));
                    SoundManager.instance.PlaySfx(Sound.SetCard);
                    if (item.DatosDigimon.id != 7)
                        item.Mostrar();
                    item.transform.localPosition = new Vector3(0, 0, (-100 - transform.childCount));

                }
            }
            yield return new WaitForSeconds(segundos);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(segundos);
            if (TermineDescarte != null)
            {
                moviendo = false;
                DigiCartas = new List<CartaDigimon>();
                TermineDescarte("Completado Dark area");
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
        Vector3 Pos = new Vector3(-transform.childCount * .007F, 0, -((transform.childCount) * 0.06f));
        carta.transform.localRotation = new Quaternion(0, 0, 0, 0);
        carta.transform.localPosition = Pos;
        carta.transform.localScale = new Vector3(1, 1, 0.15f);
        carta.Front.GetComponent<MovimientoCartas>().disebleCard();
        carta.Front.GetComponent<MovimientoCartas>().preparationPhase();
    }

    public IEnumerator WhaitFrame(CartaDigimon Dcard)
    {
        yield return new WaitForSeconds(0.3f);
        PartidaManager.instance.SetMoveCard(this.transform, Dcard.transform, InterAutoAjuste);
        yield return new WaitForEndOfFrame();
    }
    private void OnMouseDown()
    {
        VistaDarkArea.Activar(_Cartas);
    }
}
