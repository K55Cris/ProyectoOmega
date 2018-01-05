using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class Deck : MonoBehaviour {
    public GameObject modeladoDeLaCarta;
    public Camera camara = new Camera();
    public static Deck instance;
    private List<Sprite> arrayImage;
    private List<string> deck = new List<string>();

    private void Awake()
    {
        instance = this;
    }
    private void OnMouseUp()
    {
        if (!(PosicionDeLasCartas.GetCantidadActualDeCartas() == 6))
        {
            string robada = Robar();
            Texture imagenRobada = new Texture();
            foreach (var item in arrayImage)
            {
                if (item.name.ToLower().Equals(robada.ToLower()))
                {
                    imagenRobada = item.texture;
                    break;
                }
            }
            GameObject carta = Instantiate(modeladoDeLaCarta);
            carta.GetComponent<DragTest>().SetCamera(camara);
            carta.GetComponent<Renderer>().material.mainTexture = imagenRobada;
            carta.name = ("Carta" + (PosicionDeLasCartas.GetCantidadActualDeCartas() + 1));
            PosicionDeLasCartas.AumentarCarta();
        }
    }
    private void Swap(List<string> deck, int primero, int segundo)
    {
        string temp = deck[primero];
        deck[primero] = deck[segundo];
        deck[segundo] = temp;
    }
    public void FisherYates(List<string> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int index = Random.Range(0, i);
            Swap(deck, i, index);
        }
    }
    string Robar()
    {
        try
        {
            string x = deck[0];
            deck.RemoveAt(0);
            return x;
        }
        catch (System.ArgumentOutOfRangeException)
        {
            deck = GameObject.Find("DarkArea").GetComponent<DarArea>().Devolver();
            FisherYates(deck);
            return ("");
        }

    }
    public string RobarEspecifico(string codCarta)
    {
        int i = 0;
        foreach (var item in deck)
        {
            if (item.Equals(codCarta))
            {
                string x = deck[i];
                deck.RemoveAt(i);
                return x;
            }
            i++;
        }
        throw new System.Exception();
    }

    public List<string> GetDeck()
    {
        return deck;
    }
    public void SetDeck(List<string> deck)
    {
        this.deck = deck;
    }
    public List<Sprite> GetArrayImage()
    {
        return arrayImage;
    }
    public void SetArrayImage(List<Sprite> arrayImage)
    {
        this.arrayImage = arrayImage;
    }
}
