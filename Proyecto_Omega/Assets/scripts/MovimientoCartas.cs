using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MovimientoCartas : MonoBehaviour {
    public GameObject particle;
    public Camera Maincam;
    public bool Cambio=false;
    public bool Mover = true;
    public LayoutElement Layout;
    // Use this for initialization
    float distancia;
    public bool AddOrRemove=false;
    public GameObject CanvasSeleted;
    private void Start()
    {
        Maincam = Camera.main;
    }

    void OnMouseDown()
    {
        if (StaticRules.NowPhase != StaticRules.Phases.DiscardPhase&&Mover)
        { 
            Layout.ignoreLayout = true;
            particle.SetActive(true);
            this.distancia = Vector3.Distance(Maincam.transform.position, this.transform.position);
        }
        else
        {
            if (transform.parent.transform.parent.name.Contains("Option Slot") | transform.parent.transform.parent.name.Contains("Espacio"))
            {
                AddOrRemove = !AddOrRemove;
                StaticRules.instance.AddListDiscard(transform.parent.gameObject, AddOrRemove);
                CanvasSeleted.SetActive(AddOrRemove);
            }
        }
    }

    void OnMouseDrag()
    {
        if (Mover)
        {
            if (StaticRules.NowPhase != StaticRules.Phases.DiscardPhase &&
                StaticRules.NowPhase != StaticRules.Phases.EndPhase && StaticRules.NowPhase
                != StaticRules.Phases.GameSetup)
            {
                Vector3 temp = Input.mousePosition;
                temp.z = this.distancia;
                transform.parent.position = Maincam.ScreenToWorldPoint(new Vector3(temp.x, temp.y, 90));
            }
        }
    }
    private void OnMouseUp()
    {

        if (StaticRules.NowPhase != StaticRules.Phases.DiscardPhase&& Mover)
        {
            // Reralizar Cambio 
            // Obtener Slot de la Carta
            Transform Slot= MesaManager.SetOptionSlot(transform.parent.gameObject);
            // Mandar a Revisar si la carta se puede colocar en el Slot
            StaticRules.CheckSetDigiCardSlot(Slot, transform.parent);
            StartCoroutine(TerminarDesicion());
        }
    }
    IEnumerator TerminarDesicion()
    {
        yield return new WaitForEndOfFrame();
        if (!Cambio)
        {
            transform.parent.localPosition = Vector3.zero;
            Layout.ignoreLayout = false;
            particle.SetActive(false);
           
        }
        else
        {
            Debug.LogError("Movido");
            Mover = false;
            StaticRules.instance.WhosPlayer._Mano.JugarCarta(transform.parent.transform.GetComponent<CartaDigimon>());
        }
    }
    public void OnMouseOver()
    {
        VentanaMoreInfo.instance.Show(transform.parent.GetComponent<CartaDigimon>().DatosDigimon);
    }

    public void MoverCarta(Transform Destino , UnityAction<Transform, CartaDigimon> LoAction)
    {
        StartCoroutine(Transicion(Destino,LoAction));
    }

    public IEnumerator Transicion(Transform Destino, UnityAction<Transform, CartaDigimon> LoAction)
    {
        yield return new WaitForEndOfFrame();
        if (Destino)
        {
            while (transform.parent.position != Destino.position)
            {

                var heading = Destino.position - transform.parent.position;
                var distance = heading.magnitude;
                if (distance < 5)
                {
                    break;
                }

                //var direction = heading / distance; // This is now the normalized direction.
                //transform.parent.transform.Translate(direction*Time.deltaTime*30);
                float step = 500 * Time.deltaTime;
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, Destino.position, step);
                yield return new WaitForSecondsRealtime(0.01F);
            }
        }
        LoAction.Invoke(Destino, transform.parent.transform.GetComponent<CartaDigimon>());

    }
}
