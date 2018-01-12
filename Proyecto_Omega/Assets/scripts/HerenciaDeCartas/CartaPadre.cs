using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class CartaPadre : MonoBehaviour {
    private int id;
    private string descripcion;
    private Image imagen;
    private UnityAction jugarListener;
    private UnityAction quitarListener;
    private UnityAction mulliganListener;
    private UnityAction seleccionListener;
    private UnityAction empiezaElMulliganListener;
    private UnityAction terminaElMulliganListener;
    private bool seleccionMulligan = false;
    private bool clickEnable = true;
    private bool doubleClick = false;
    private bool simpleClick = false;
    private bool marca = true;
    private float time1, time2;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !marca)
        {
            seleccionMulligan = !seleccionMulligan;
            this.GetComponent<Renderer>().transform.Rotate(new Vector3(90, 0, 0));//contorno o algo que diga que esta seleccionada
        }

    }
    void sad()
    {
        time2 = Time.time;
        if (clickEnable && (time1 - time2 < 0.2f))
        {
            clickEnable = false;
            doubleClick = true;
        }
        else
        {
            time1 = Time.time;
            clickEnable = true;
        }
    }
    void OnMouseUp()
    {
        if (clickEnable)
        {
            clickEnable = false;
            StartCoroutine(TrapDoubleClicks(0.5f));
        }
    }
    IEnumerator TrapDoubleClicks(float timer)
    {
        float endTime = Time.time + timer;
        while (Time.time < endTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                yield return new WaitForSeconds(0);
                clickEnable = true;
                doubleClick = true;
                simpleClick = false;
            }
            yield return 0;
        }

        if (doubleClick)
        {
            doubleClick = false;
        }

        clickEnable = true;
        yield return 0;
    }

    private void Awake()
    {
        jugarListener = new UnityAction(JugarCarta);
        quitarListener = new UnityAction(QuitarCarta);
        mulliganListener = new UnityAction(MulliganCarta);
        seleccionListener = new UnityAction(SeleccionCarta);
        empiezaElMulliganListener = new UnityAction(StartMulliganCarta);
        terminaElMulliganListener = new UnityAction(StopMulliganCarta);
    }

    private void OnEnable()
    {
        EventManager.StartListening("JugarCarta", jugarListener);
        EventManager.StartListening("QuitarCarta", quitarListener);
        EventManager.StartListening("MulliganCarta", mulliganListener);
        EventManager.StartListening("SeleccionCarta", seleccionListener);
        EventManager.StartListening("StartMulliganCarta", empiezaElMulliganListener);
        EventManager.StartListening("StopMulliganCarta", terminaElMulliganListener);
    }

    public abstract void JugarCarta();
    public abstract void QuitarCarta();
    public void MulliganCarta() {
        //desde la mano al dark
        if (seleccionMulligan && name.Substring(0,5).Equals("Carta"))
        {
            DarkArea.instance.Meter(/*id*/GetComponent<Renderer>().material.mainTexture.name);
            PosicionDeLasCartas.QuitarCarta(int.Parse(name.Substring(5)));
            Destroy(gameObject);
        }
        else if (seleccionMulligan)
        {
            DarkArea.instance.Meter(/*id*/GetComponent<Renderer>().material.mainTexture.name);
            Destroy(gameObject);
        }
    }
    public void SeleccionCarta()
    {
        if (simpleClick)
        {
            this.GetComponent<Renderer>().transform.Rotate(new Vector3(90,0,0));
            this.seleccionMulligan = true;
            simpleClick = false;
        }
    }
    public void StartMulliganCarta()
    {
        if (marca)
        {
            marca = !marca;
            this.SetSeleccionMulligan(false);
        }
    }
    public void StopMulliganCarta()
    {
        if (!marca)
        {
            marca = !marca;
        }
    }
    public void SetSeleccionMulligan(bool SeleccionMulligan)
    {
        this.seleccionMulligan = SeleccionMulligan;
    }
    public bool GetSeleccionMulligan()
    {
        return this.seleccionMulligan;
    }
    public bool GetDoubleClick()
    {
        return doubleClick;
    }
    public void SetDoubleClick(bool doubleClick)
    {
        this.doubleClick = doubleClick;
    }
}
