using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MapDuelManager : MonoBehaviour
{
    public List<ItemMenuMap> Nodos;
    public CanvasGroup MoreInfo;
    public Image IAImage;
    public Button Play;
    public TextMeshProUGUI Nombre, Wins, DeckName, Loses, Nivel, level;
    public GameObject Booss;
    public static MapDuelManager instance;
    // Use this for initialization
    void Awake()
    {

        if (instance == null)
        {
            instance = this;

        }
    }
    // Use this for initialization
    void Start()
    {
        if (PlayerManager.instance.Jugador.ALLCards)
        {
            Booss.SetActive(true);
        }
        foreach (var item in PlayerManager.instance.GetProgress())
        {
            ItemMenuMap Nodo = Nodos.Find(K => K.ID == item.ID);
            if (Nodo != null)
            {
                Nodo.Cargar(item);
            }
        }
        Nivel.text = PlayerManager.instance.Jugador.Nivel.ToString();
    }
    public void ShowInfo(ItemMenuMap nodo)
    {
        MoreInfo.alpha = 1;
        IAImage.sprite = nodo.IAImage;
        Nombre.text = nodo.Nombre;
        DeckName.text = nodo.NombreMazo;
        Wins.text = nodo.wins.ToString();
        Loses.text = nodo.Loses.ToString();
        PlayerManager.instance.IaPlaying = nodo.ID;
        PlayerManager.instance.DeckIA = nodo.Decks;
        PlayerManager.instance.MusicaDuelo = nodo.NowSound;
        level.text = nodo.NivelNecesario.ToString();

        if (PlayerManager.instance.Jugador.Nivel < nodo.NivelNecesario)
            Play.interactable = false;
        else
        {
            if (nodo.Accesible)
                Play.interactable = true;
            else
                Play.interactable = false;
        }


    }
}
