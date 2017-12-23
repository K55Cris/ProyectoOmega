using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragTest : MonoBehaviour{
    /*tipos
     * tipo -1 tipo no definido
     * OptionSlot = 0
     * DigimonBox = 1 -- Rookie = 1
     * 
     * Point gauge = 7
     */
    //variables
    [TooltipAttribute("Ingresa la mainCamera.")]
    public Camera camara;
    [TooltipAttribute("Tipo de la carta (-1 ningun tipo), El tipo debe ser el mismo que el de AdmiteDrag.tipo.")]
    public int tipo = -1;
    [TooltipAttribute("Ingresa un gameObject para remplazar la carta.")]
    public GameObject iconoClon;
    [TooltipAttribute("Es el tamaño de la carta (realmente es la profundidad a la que se ve la carta).")]
    public float distProfundidadIcono = 458.0f;
    [TooltipAttribute("Color del efecto al clikear la carta.")]
    public Color color1 = Color.red;
    [TooltipAttribute("Color del efecto al clikear la carta.")]
    public Color color2 = Color.yellow;
    [TooltipAttribute("Es el material que va a tener el clon de la carta.")]
    public Material materialClon;
    public float duration = 1.0f;
    public float velocidad = 4.0f;
    [TooltipAttribute("Si esta carta puede tener otra en cima.")]
    public bool necesitaMesa = false;
    [TooltipAttribute("Delay del doble Click.")]
    public float delayDelDobleClick = 1;

    private Vector3 pos;
    private Vector2 pos2;
    private GameObject clon;
    private Vector3 posClon;
    private bool bol = false;
    private float distancia;
    //|distancia de la camara - altura de las cartas|
    private Ray rayo;
    private Material materialOriginal;
    private Vector3 p;
    private Matrix4x4 m;
    private Vector3 vec;
    private float xClon;// = pos2.x;
    private float yClon;// = pos2.y;
    private float zClon;// = distProfundidadIcono;
    private bool actualizado = false;
    private int w = Screen.width;
    private int h = Screen.height;
    private bool estoyEnMesa = false;
    private bool clikeado = false;
    private bool unClick = false;
    private bool enTiempo;
    private float tiempoParaElDobleClick;
    private string estoyEn = "";

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            pos2 = e.mousePosition;
        }
    }

    private void OnMouseUp()
    {
        //renderer.material
        GetComponent<Renderer>().material = materialOriginal;
        Destroy(clon);
        clon = null;
        bol = false;
        if (!necesitaMesa)
        {
            if (CNTdrag.objetoQuieto != null && CNTdrag.tipo == tipo && !CNTdrag.estoyEnMesa)
            {
                transform.position = CNTdrag.objetoQuieto.transform.position;
                CNTdrag.clikeado = false;
                estoyEnMesa = true;
            }
            
        }
        else
        {
            if (!CNTdrag.estoyEnMesa)
            {
                if (CNTdrag.objetoQuieto != null && CNTdrag.tipo == tipo)
                {
                    transform.position = CNTdrag.objetoQuieto.transform.position;
                    CNTdrag.clikeado = false;
                    this.clikeado = false;
                    estoyEnMesa = true;
                }
                
            }
        }
        CNTdrag.objetoQuieto = null;

    }
    private void OnMouseDrag()
    {
        if (!estoyEnMesa)
        {
            Test();
            actualizado = true;
        }
    }

    private void OnMouseDown()
    {
        if (!unClick)
        {
            unClick = true;
            tiempoParaElDobleClick = Time.time;
        }
        else
        {
            unClick = false;
            //movimiento de carta
            if (!estoyEnMesa)
            {
                if (tipo == CNTdrag.OPTION_SLOT)
                {
                    if (!GameObject.Find("Option Slot 1").GetComponent<OptionSlot>().GetOcupado())
                    {
                        GameObject.Find("Option Slot 1").GetComponent<OptionSlot>().Jugar(this);
                        estoyEn = "Option Slot 1";
                    }
                    else if (!GameObject.Find("Option Slot 2").GetComponent<OptionSlot>().GetOcupado())
                    {
                        GameObject.Find("Option Slot 2").GetComponent<OptionSlot>().Jugar(this);
                        estoyEn = "Option Slot 2";
                    }
                    else if (!GameObject.Find("Option Slot 3").GetComponent<OptionSlot>().GetOcupado())
                    {
                        GameObject.Find("Option Slot 3").GetComponent<OptionSlot>().Jugar(this);
                        estoyEn = "Option Slot 3";
                    }
                }
            }
            else //if(face de mulligan)
            {
                GameObject.Find(estoyEn).GetComponent<OptionSlot>().Quitar(this);
            }
        }
        CNTdrag.clikeado = true;
        this.clikeado = true;
    }

    // Use this for initialization
    void Start () {
        Inicializa();
        pos = transform.position;
        posClon = pos;
        //renderer.material
        materialOriginal = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
        if (unClick)
        {
            if ((Time.time - tiempoParaElDobleClick) > delayDelDobleClick)
            {
                unClick = false;
            }
        }
        pos = transform.position;
        if (clon)
        {
            //Rigidbody
            if (clon.GetComponent<Rigidbody>() != null)
            {
                //clon.rigidbody
                Destroy(clon.GetComponent<Rigidbody>());
            }
            //Collider
            if (clon.GetComponent<Collider>() != null)
            {
                //clon.collider
                Destroy(clon.GetComponent<Collider>());
            }
            clon.transform.position = posClon;
            Colores();

            vec = new Vector3(xClon, yClon, camara.nearClipPlane + (zClon));
            DibujaClon();
            if (actualizado)
            {
                Inicializa();
            }
            xClon += Input.GetAxis("Mouse X") * velocidad * 100 * Time.deltaTime;
            yClon += Input.GetAxis("Mouse Y") * velocidad * 100 * Time.deltaTime;
        }
        Rayo();
        w = Screen.width;
        h = Screen.height;
    }
    private void Test()
    {
        if (Input.GetButton("Fire1"))
        {
            m = camara.cameraToWorldMatrix;
            p = m.MultiplyPoint(new Vector3(0, 0, distancia));
            posClon =  camara.WorldToScreenPoint(p);
            if (!bol)
            {
                if (iconoClon)
                {
                    clon = Instantiate(iconoClon, posClon, Quaternion.identity);
                    clon.transform.Rotate(90, 0, 0);
                }
                else
                {
                    clon = Instantiate(gameObject, posClon, Quaternion.identity);
                    clon.transform.Rotate(90, 0, 0);
                }
                bol = true;
            }
        }
    }
    private void Rayo()
    {
        rayo.origin = camara.transform.position;
        rayo.direction = transform.position;
        distancia = Vector3.Distance(rayo.origin, rayo.direction);
    }
    private void Colores()
    {

        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        if (!materialClon)
        {
            //clon.renderer.material
            clon.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
            clon.GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, lerp);
        }
        else
        {
            //clon.renderer.material
            clon.GetComponent<Renderer>().material  = materialClon;
        }
        //renderer.material
        GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
        color1.a = 0.5f;
        color2.a = 0.5f;
        //renderer.material
        GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, lerp);

    }
    private void DibujaClon()
    {
        Vector3 p1 = camara.ScreenToWorldPoint(vec);
        clon.transform.position = p1;

    }
    private void Inicializa()
    {
        xClon = pos2.x;
        yClon = h - (pos2.y);
        zClon = distProfundidadIcono;
        actualizado = false;
    }
    private void OnMouseOver()
    {
        CNTdrag.estoyEnMesa = this.estoyEnMesa;
    }
    public void SetEstoyEnMesa(bool estoyEnMesa)
    {
        this.estoyEnMesa = estoyEnMesa;
    }
}
