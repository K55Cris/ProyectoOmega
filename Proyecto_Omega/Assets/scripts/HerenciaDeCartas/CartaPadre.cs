using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class CartaPadre : MonoBehaviour {
    private int id;
    private string descripcion;
    private Image imagen;
    private UnityAction jugarLisener;
    private bool clickEnable = true;
    private bool doubleClick = false;

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
        jugarLisener = new UnityAction(JugarCarta);
    }

    private void OnEnable()
    {
        EventManager.StartListening("JugarCarta", jugarLisener);
    }

    public abstract void JugarCarta();
    public void MulliganCarta() {
    }

    public bool GetDoubleClick()
    {
        return doubleClick;
    }
}
