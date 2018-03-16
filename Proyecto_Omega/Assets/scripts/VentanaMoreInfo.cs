using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigiCartas;
public class VentanaMoreInfo : MonoBehaviour {

    public static VentanaMoreInfo instance;
    public Image DigimonImage, Type;
    public DigiCarta Datos;
    public Text NombreAtaqueA, NombreAtaqueB, NombreAtaqueC, Habilidad, Evolucion, TypeText, NombreDigimon, Capacidad;
    public Text DañoA, DañoB, DañoC;
    public CanvasGroup Panel;
    public GameObject DigimonCard, OptionCard;
    private void Awake()
    {
        instance = this;
    }
    public void Show(DigiCarta DatosDigimon)
    {
        if (Datos != DatosDigimon)
        {
            CancelInvoke("Cerrando");
            Panel.alpha = 1;
            DigimonImage.sprite = DataManager.instance.GetSprite(DatosDigimon.id);
            Type.gameObject.SetActive(true);
            Datos = DatosDigimon;
            if (Datos.Capasidad == 0)
            {
                DigimonCard.SetActive(true);
                OptionCard.SetActive(false);
                switch (Datos.TipoBatalla)
                {
                    case "A":
                        Type.color = Color.red;
                        TypeText.text = "A";
                        break;
                    case "B":
                        Type.color = Color.green;
                        TypeText.text = "B";
                        break;
                    case "C":
                        Type.color = Color.yellow;
                        TypeText.text = "C";
                        break;
                }
                NombreAtaqueA.text = Datos.NombreAtaqueA;
                NombreAtaqueB.text = Datos.NombreAtaqueB;
                NombreAtaqueC.text = Datos.NombreAtaqueC;
                DañoA.text = Datos.DanoAtaqueA.ToString();
                DañoB.text = Datos.DanoAtaqueB.ToString();
                DañoC.text = Datos.DanoAtaqueC.ToString();
            }
            else
            {
                DigimonCard.SetActive(false);
                OptionCard.SetActive(true);
                Capacidad.text = Datos.Capasidad.ToString();
            }
            NombreDigimon.text = Datos.Nombre;
            Invoke("Cerrando", 5f);
        }
     }
    public void Cerrando()
    {
       StartCoroutine(WhoIsPlayer1.ReduceAlpha(Panel));
    }

    
}
