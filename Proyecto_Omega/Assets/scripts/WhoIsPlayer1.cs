using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class WhoIsPlayer1 : MonoBehaviour {

    public Animator Am;
    public GameObject Moneda;
    public static WhoIsPlayer1 instance;
    public Text Titulo;
    public Image Cara, Cruz;
    public CanvasGroup _CGroup;
    private UnityAction<Player> Evento;
    private Player Player1, Player2;
    private bool val;
    private bool PlayerValor;
    
    private void Awake()
    {
        instance = this;
    }

    public void Activar(Player player1, Player player2, UnityAction<Player> WhoIs)
    {
        _CGroup.alpha = 1;
        _CGroup.interactable = true;
        _CGroup.blocksRaycasts = true;
        Evento = WhoIs;
        Player1 = player1;
        Player2 = player2;
    }


    public void SelectedPlayerOne(bool moneda)
    {
        // Cara o Cruz
        int ran = Random.Range(1, 3);
        Cara.CrossFadeAlpha(0, 0.5f,true);
        Cruz.CrossFadeAlpha(0, 0.5f,true);
        Moneda.SetActive(true);
        GetComponent<Image>().CrossFadeAlpha(0, 1.5f, true);
        if (ran==1)
        {
            Am.SetBool("Cara", true);
            val = true;
  
        }
        else
        {
            Am.SetBool("Cruz", true);
            val = false;

        }
        PlayerValor = moneda;
    }

    public void Desicion()
    {
        if (val == PlayerValor)
        {
            Evento(Player1);
            Titulo.text = Player1.Nombre + " juega primero";
        }
        else
        {
            Evento(Player2);
            Titulo.text = Player2.Nombre + " juega primero";
        }
        
   
        Invoke("CerrarPanel", 1F);
    }
   public void CerrarPanel()
    {
        Moneda.SetActive(false);
        StartCoroutine(ReduceAlpha(_CGroup));
    }
    
   public static IEnumerator ReduceAlpha(CanvasGroup cv)
    {
        while (cv.alpha > 0)
        {
            yield return new WaitForSeconds(0.1f);
            cv.alpha -= 0.25f;
        }
        cv.blocksRaycasts = false;
        cv.interactable = false;
    }
	
}
