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
    private void Start()
    {
        Maincam = Camera.main;
    }

    void OnMouseDown()
    {
        Layout.ignoreLayout = true;
        particle.SetActive(true);
        this.distancia = Vector3.Distance(Maincam.transform.position, this.transform.position);
    }

    void OnMouseDrag()
    {

        Vector3 temp = Input.mousePosition;
        temp.z = this.distancia;
        print(temp.x);
        print(temp.y);
        print(temp.z);
        print(temp);
        transform.parent.position = Maincam.ScreenToWorldPoint(temp);

    }
    private void OnMouseUp()
    {
        print(transform.position);
        // Reralizar Cambio 
        StartCoroutine(TerminarDesicion());
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

}
