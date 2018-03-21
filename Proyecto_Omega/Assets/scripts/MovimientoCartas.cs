using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MovimientoCartas : MonoBehaviour {
    public GameObject particle;
    public Camera Maincam;
    public bool Cambio=false;
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
        if (StaticRules.NowPhase != StaticRules.Phases.DiscardPhase)
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
        if (StaticRules.NowPhase != StaticRules.Phases.DiscardPhase )
        {
            Vector3 temp = Input.mousePosition;
            temp.z = this.distancia;
            transform.parent.position = Maincam.ScreenToWorldPoint(new Vector3(temp.x, temp.y, 90));
        }

    }
    private void OnMouseUp()
    {

        if (StaticRules.NowPhase != StaticRules.Phases.DiscardPhase)
        {
            // Reralizar Cambio 
            // Obtener Slot de la Carta
            Transform Slot= MesaManager.GetOptionSlot(transform.parent.gameObject);
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
    }
    public void OnMouseOver()
    {
        VentanaMoreInfo.instance.Show(transform.parent.GetComponent<CartaDigimon>().DatosDigimon);
    }

}
