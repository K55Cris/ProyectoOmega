﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenu : MonoBehaviour {

    //Comentario Random
    public TMP_InputField Nombre;
    public TextMeshProUGUI NombreCuenta;
    public Button Aceptar;
    public static MainMenu instance;
    public GameObject PanelNewUser, Opciones, Tutorial, PanelCloseOpcion;
    public RectTransform Contendor;
    public Dropdown Dropdown; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }

        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
  
   
    }

    public void Quit()
    {
        SoundManager.instance.PlaySfx(Sound.Out);
        Application.Quit();
    }
    public void vSTamer()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        LevelLoader.instance.CargarEscena("Recompensa");
    }
    public void VsIA()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        LevelLoader.instance.CargarEscena("MapaDuelos");
    }
    public void DeckEditor()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        LevelLoader.instance.CargarEscena("DeckEditor");
    }

    public void OpenTutorial()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        LevelLoader.instance.CargarEscena("Tutorial");
    }
    public void OpenOpciones()
    {
        
        SoundManager.instance.PlaySfx(Sound.Enter);
        StartCoroutine(MovePanel(Contendor, 1));
    }
    public void closeOpciones()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        StartCoroutine(MovePanel(Contendor, -1));
        SoundManager.instance.SaveSettings();
    }

    public void ChooseName()
    {
        if (!string.IsNullOrEmpty(Nombre.text))
        {
            Aceptar.interactable = true;
        }
        else
        {
            Aceptar.interactable = false;
        }
    }
    public void SaveName()
    {
        PlayerManager.instance.SaveName(Nombre.text);
        LoadName();
    }
    public void LoadName()
    {
        if (!string.IsNullOrEmpty(PlayerManager.instance.Jugador.Nombre))
        {
            NombreCuenta.text = PlayerManager.instance.Jugador.Nombre;
        }
    }
    public IEnumerator MovePanel(RectTransform Panel,int Signo)
    {
        if (Signo > 0)
        {
            while (Panel.transform.localPosition.x < 430)
            {
                yield return new WaitForSeconds(0.005f);
                float Incremento =600f*Time.deltaTime * Signo;
                Vector3 NewPos = new Vector3(0, 0, 0);
                if (Signo > 0)
                {
                    if (Panel.transform.localPosition.x + Incremento > 430)
                    {
                        NewPos = new Vector3(430, 0, 0);
                        Panel.transform.localPosition = NewPos;
                        break;
                    }
                }
                else
                {
                    if (Panel.transform.localPosition.x + Incremento < 0)
                    {
                        NewPos = new Vector3(0, 0, 0);
                        Panel.transform.localPosition = NewPos;
                        break;
                    }
                }

                NewPos = new Vector3(Panel.transform.localPosition.x + Incremento, 0, 0);

                Panel.transform.localPosition = NewPos;
            }
            PanelCloseOpcion.SetActive(true);
        }
        else
        {
            while (Panel.transform.localPosition.x >0)
            {
                yield return new WaitForSeconds(0.01f);
                int Incremento = 10 * Signo;
                Vector3 NewPos = new Vector3(0, 0, 0);
                if (Signo > 0)
                {
                    if (Panel.transform.localPosition.x + Incremento > 430)
                    {
                        NewPos = new Vector3(430, 0, 0);
                        Panel.transform.localPosition = NewPos;
                        break;
                    }
                }
                else
                {
                    if (Panel.transform.localPosition.x + Incremento < 0)
                    {
                        NewPos = new Vector3(0, 0, 0);
                        Panel.transform.localPosition = NewPos;
                        break;
                    }
                }

                NewPos = new Vector3(Panel.transform.localPosition.x + Incremento, 0, 0);

                Panel.transform.localPosition = NewPos;
            }
            PanelCloseOpcion.SetActive(false);

        }

    }
    public void ChangeGraficos()
    {
        QualitySettings.SetQualityLevel(Dropdown.value);
    }
}
