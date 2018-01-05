using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBaryCantidadDeCartas : MonoBehaviour {
    public static ScrollBaryCantidadDeCartas instance;
    public GameObject prefCarta;
    public Transform contenedor;
    private List<Sprite> arrayRookie;

    private void Awake()
    {
        instance = this;
    }
    
    public void CargarDatos()
    {
        foreach (var item in this.arrayRookie)
        {
            GameObject cartas = Instantiate(prefCarta, contenedor);
            cartas.GetComponent<DetalleCarta>().Cargar(item);
        }
    }
	
    public void SetArrayRookie(List<Sprite> arrayRookie)
    {
        this.arrayRookie = arrayRookie;
    }

    public List<Sprite> GetArrayRookie()
    {
        return this.arrayRookie;
    }
    // Update is called once per frame
    void Update () {

    }
}
