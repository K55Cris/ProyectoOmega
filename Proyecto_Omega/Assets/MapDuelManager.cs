using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MapDuelManager : MonoBehaviour {
    public List<ItemMenuMap> Nodos;
    public CanvasGroup MoreInfo;
    public Image IAImage;
    public Button Play;
    public TextMeshProUGUI Nombre, Wins, DeckName,Loses;

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
    void Start ()
    {

        foreach (var item in PlayerManager.instance.GetProgress())
        {
            ItemMenuMap Nodo = Nodos.Find(K => K.ID == item.ID);
            if (Nodo!=null)
            {
                Nodo.Cargar(item);
            }
        }
	}
    public void ShowInfo(ItemMenuMap nodo)
    {
        MoreInfo.alpha = 1;
        Play.interactable = true;
        IAImage.sprite = nodo.IAImage;
        Nombre.text = nodo.Nombre;
        DeckName.text = nodo.NombreMazo;
        Wins.text = nodo.wins.ToString();
        Loses.text = nodo.Loses.ToString();
    }
}
