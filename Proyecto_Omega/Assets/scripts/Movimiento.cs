using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour {
    public float y = 0;
    private Ray ray;
    private Canvas a = new Canvas();
    public GameObject particle;
    private void Click()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            print(ray);
            if (!Physics.Raycast(ray))
            {
                //Si no hay, creas un objeto particle, en la posicion del mouse
                Instantiate(particle, Input.mousePosition, transform.rotation);
            }
        }
    }
    float distancia;

    void OnMouseDown()
    {

        this.distancia = Vector3.Distance(Camera.main.transform.position, this.transform.position);
    }

    void OnMouseDrag()
    {

        Vector3 temp = Input.mousePosition;
        temp.z = this.distancia;
        print(temp.x);
        print(temp.y);
        print(temp.z);
        print(temp);
        this.transform.position = Camera.main.ScreenToWorldPoint(temp);

    }
    private void OnMouseUp()
    {
        print(transform.position);
    }
    /*private void OnMouseDown()
    {
        Click();
        print(ray.origin);
        transform.position = ray.origin;
        transform.Rotate(90,0,0);
        //transform.Translate(0, y, 0);
    }*/
    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update() {
    }
}
