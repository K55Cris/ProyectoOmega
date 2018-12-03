﻿using System.Collections;
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
    }
    public void Salir()
    {
        // Mandar a guardar la carta obtnida al los datos del jugador  
        SceneManager.LoadSceneAsync("Main Menu");
    }
    public void GetCarta(int DigiId)
    {
        NewDigi = DataManager.instance.GetDigicarta(DigiId);
        SetDatos();
    }

    public void SetDatos()
    {
        NombreDigimon.text = NewDigi.Nombre;
        Desbloqueado.gameObject.SetActive(true);
    }
}