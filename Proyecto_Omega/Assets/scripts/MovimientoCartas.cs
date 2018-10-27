﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MovimientoCartas : MonoBehaviour {
    public GameObject particle;
    public Camera Maincam;
    public bool Cambio=false;
    public bool Mover = false;
    public int IdMove = 0;
    public GameObject Muerte;
    public LayoutElement Layout;
    // Use this for initialization
    float distancia;
    public bool AddOrRemove=false;
    public GameObject CanvasSeleted;
    public Image BotonDiscardPhase;
    public Sprite Descar, Cancelar;
    private int shaderProperty;

    private void Start()
    {
        Maincam = Camera.main;
    }

    void OnMouseDown()
    {
       if (StaticRules.NowPhase == DigiCartas.Phases.PreparationPhase && Mover)
        { 
            Layout.ignoreLayout = true;
           
            this.distancia = Vector3.Distance(Maincam.transform.position, this.transform.position);
        }
        else
        {
            if (transform.parent.transform.parent.name.Contains("Espacio")&& StaticRules.NowPhase == DigiCartas.Phases.DiscardPhase)
            {
   
                CanvasSeleted.SetActive(true);
                if (!AddOrRemove)
                    BotonDiscardPhase.sprite = Descar;
                else
                    BotonDiscardPhase.sprite = Cancelar;
            }
        }
    }

    void OnMouseDrag()
    {
        if (Mover)
        {
            if (StaticRules.NowPhase == DigiCartas.Phases.PreparationPhase )
            {
                Vector3 temp = Input.mousePosition;
                temp.z = this.distancia;
                particle.SetActive(true);
                transform.parent.position = Maincam.ScreenToWorldPoint(new Vector3(temp.x, temp.y, 90));
            }
        }
    }
    private void OnMouseUp()
    {

        if (StaticRules.NowPhase != DigiCartas.Phases.DiscardPhase&& Mover)
        {
            // Reralizar Cambio 
            // Obtener Slot de la Carta
            Transform Slot= MesaManager.SetOptionSlot(transform.parent.gameObject);
            // Mandar a Revisar si la carta se puede colocar en el Slot
            StaticRules.CheckSetDigiCardSlot(Slot, transform.parent, true);
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

    public void MoverCarta(Transform Destino , UnityAction<Transform, CartaDigimon,int> LoAction, int ID)
    {
        IdMove = ID;
        StartCoroutine(Transicion(Destino,LoAction));
    }

    public IEnumerator Transicion(Transform Destino, UnityAction<Transform, CartaDigimon,int> LoAction)
    {
        yield return new WaitForEndOfFrame();
        if (Destino)
        {
            var heading = Destino.position - transform.parent.position;
            var distance = heading.magnitude;
            while (distance > 5)
            {
                heading = Destino.position - transform.parent.position;
                distance = heading.magnitude;
                //var direction = heading / distance; // This is now the normalized direction.
                //transform.parent.transform.Translate(direction*Time.deltaTime*30);
                float step = 500 * Time.deltaTime;
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, Destino.position, step);
                yield return new WaitForSecondsRealtime(0.01F);
            }
        }
        LoAction.Invoke(Destino, transform.parent.transform.GetComponent<CartaDigimon>(),IdMove);

    }
    public void disebleCard()
    {
        Mover = false;
        particle.SetActive(false);
        AddOrRemove = false;
        CanvasSeleted.SetActive(false);
    }
    public void DestruirCarta()
    {
        Muerte.SetActive(false);
        Muerte.SetActive(true);
        MeshRenderer Padre = transform.parent.GetComponent<MeshRenderer>();
        Padre.enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        MeshRenderer MuerteM = Muerte.GetComponent<MeshRenderer>();
        
        shaderProperty = Shader.PropertyToID("_Normal");
        MuerteM.material.SetTexture(shaderProperty, DataManager.instance.GetTextureDigimon(transform.parent.GetComponent<CartaDigimon>().DatosDigimon.id));

        Invoke("Regreso",1.2f);
    }
    public void Regreso()
    {
        Muerte.SetActive(false);
        MeshRenderer Padre = transform.parent.GetComponent<MeshRenderer>();
        Padre.enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }
    public void Descartar()
    {
        if (!AddOrRemove)
        {
      
            Renderer rend = GetComponent<Renderer>();
            //Set the main Color of the Material to green
            Debug.Log("Desactivado:" + rend.material.name);
            rend.material.color = Color.gray;
          
        }
        else
        {
            Debug.Log("Activado");
            Renderer rend = GetComponent<Renderer>();
            //Set the main Color of the Material to green
            rend.material.color = Color.white;

        }
        AddOrRemove = !AddOrRemove;
        if (!AddOrRemove)
            BotonDiscardPhase.sprite = Descar;
        else
            BotonDiscardPhase.sprite = Cancelar;
        StaticRules.instance.AddListDiscard(transform.parent.gameObject, AddOrRemove);
    }
    public void preparationPhase()
    {
        Renderer rend = GetComponent<Renderer>();
        //Set the main Color of the Material to green
        rend.material.color = Color.white;
        CanvasSeleted.SetActive(false);
    }



}
