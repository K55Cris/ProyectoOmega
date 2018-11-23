using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActivarCarta : MonoBehaviour {
    public Image DCarta;
    public CartaDigimon CartaUsada;
    public Animator Am;
    public static ActivarCarta instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    public void Activar(CartaDigimon Carta)
    {
        Debug.Log(Carta.DatosDigimon.Nombre);
        if (CartaUsada != Carta) {
            DCarta.overrideSprite= DataManager.instance.GetSprite(Carta.DatosDigimon.id);
        }
     
        Am.Play("Efectos");
    }
}
