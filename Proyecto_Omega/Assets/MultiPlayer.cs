using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MultiPlayer : MonoBehaviour
{
    public MultiplayerItem Player1;
    public MultiplayerItem Player2;
    public Button StartGame;
    public static MultiPlayer instance;
    public GameObject Inicio;
    public GameObject Room;
    public Lobby _Lobby;
    public Sprite DefaultImage;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        StartGame.interactable = false;
    }

    public void CargarPlayer1(DigiCartas.QuickPlayer Player)
    {
        Player1.Load(Player.Nombre, Player.Nivel.ToString(), DataManager.instance.GetPerfilPhoto(Player.Photo));
    }
    public void CargarPlayer2(DigiCartas.QuickPlayer Player)
    {
        Player2.Load(Player.Nombre, Player.Nivel.ToString(), DataManager.instance.GetPerfilPhoto(Player.Photo));
    }

    

    public void Reset()
    {
        Player1.Reset("", DefaultImage,0);
        Player2.Reset("...", DefaultImage,1);
    }
    public void LeaveGame()
    {
        Inicio.gameObject.SetActive(true);
        Room.gameObject.SetActive(false);
        StartGame.gameObject.SetActive(false);
    }
}
