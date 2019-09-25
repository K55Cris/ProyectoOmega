using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DigiCartas;
public class RecomnesaManager : MonoBehaviour {
    public bool PresOne = false;
    public Button BtnAll;
    public List<GameObject> Particulas;
    public Animator Am;
    public TextMeshProUGUI NombreDigimon;
    public GameObject Desbloqueado;
    public DigiCarta NewDigi;
    public AudioClip ClipNewCard;
    public AudioSource As;
    public GameObject Pucha;
    public ParticleSystem PosOpen;
    public void Start()
    {
        SoundManager.instance.PlayMusic(Sound.Recompensa);
    }
    public void GetCarta()
    {
        if (!PresOne)
        {
            Pucha.SetActive(false);
            BtnAll.interactable = false;
            As.clip = ClipNewCard;
            As.Play();
            PosOpen.gameObject.SetActive(false);
            Am.Play("WinCard");
            foreach (var item in Particulas)
            {
                item.SetActive(true);
            }
            Invoke("WhaitCard", 2f);
            PresOne = !PresOne;
        }
        else
        {
            Salir();
        }
    }
    public void WhaitCard()
    {
        BtnAll.interactable = true;
        PosOpen.gameObject.SetActive(true);
        PosOpen.Play();
    }
    public void Salir()
    {
        // Mandar a guardar la carta obtnida al los datos del jugador  
        LevelLoader.instance.CargarEscena("Main Menu");
    }
    public void GetCarta(int DigiId,bool nuevo)
    {
        NewDigi = DataManager.instance.GetDigicarta(DigiId);
        NombreDigimon.text = NewDigi.Nombre;
        Desbloqueado.gameObject.SetActive(nuevo);
    }

 
}
