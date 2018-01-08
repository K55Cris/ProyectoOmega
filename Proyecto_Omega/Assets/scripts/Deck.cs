using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour {
    [TooltipAttribute("Modelado de la carta.")]
    public GameObject modeladoDeLaCarta;
    [TooltipAttribute("Ingresa la mainCamera.")]
    public Camera camara = new Camera();
    [TooltipAttribute("Deck del jugador/Bot/cualquier deck")]
    public static Deck instance;
    private List<Sprite> arrayImage;
    private List<string> deck = new List<string>();
    private UnityAction roboLisener;
    private void Awake()
    {
        roboLisener = new UnityAction(RobarYAcomodarEnMano);
        instance = this;
    }

    private void OnEnable()
    {
        EventManager.StartListening("RobarYAcomodarEnMano", roboLisener);
    }

    private void RobarYAcomodarEnMano()
    {
        //revista si el deck esta bacio
        if (!(deck == null))
        {
            //mira la mano y revisa si esta llena
            if (!(PosicionDeLasCartas.GetCantidadActualDeCartas() == 6))
            {
                //roba el codigo
                string robada = Robar();
                //carga los datos (en este caso solo las imagenes, deberia llamar a DataManager y pedir la carta y su info o cargar los datos al deck y solo robar hmm)
                //Y compara todas las imagenes en busca de la necesaria (Si es un asco pero era para probar)
                Texture imagenRobada = new Texture();
                foreach (var item in arrayImage)
                {
                    if (item.name.ToLower().Equals(robada.ToLower()))
                    {
                        imagenRobada = item.texture;
                        break;
                    }
                }
                //instancia la carta con su modelado y se le dan sus valores
                GameObject carta = Instantiate(modeladoDeLaCarta);
                carta.GetComponent<DragTest>().SetCamera(camara);
                carta.GetComponent<Renderer>().material.mainTexture = imagenRobada;
                carta.name = ("Carta" + (PosicionDeLasCartas.GetCantidadActualDeCartas() + 1));
                //posiciona la carta
                PosicionDeLasCartas.AumentarCarta();
            }
        }
        else
        {
            //si esta vacio en vez de robar carga el deck, y lo mezcla
            deck = GameObject.Find("DarkArea").GetComponent<DarArea>().Devolver();
            FisherYates(deck);
        }
    }

    private void Swap(List<string> deck, int primero, int segundo)
    {
        string temp = deck[primero];
        deck[primero] = deck[segundo];
        deck[segundo] = temp;
    }
    //mezcla el deck
    public void FisherYates(List<string> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int index = Random.Range(0, i);
            //lo asigna
            Swap(deck, i, index);
        }
    }
    //devuelve el codigo de una carta
    string Robar()
    {
        try
        {
            //devuelve la primera carta
            string x = deck[0];
            deck.RemoveAt(0);
            return x;
        }
        catch (System.ArgumentOutOfRangeException)
        {
            //en caso de error por estar vacio devuelve se le asigna null para compararlo mas tarde
            deck = null;
            return null;
        }
        /*catch (System.ArgumentOutOfRangeException)
        {
            deck = GameObject.Find("DarkArea").GetComponent<DarArea>().Devolver();
            FisherYates(deck);
            return ("");
        }*/

    }
    // si hay cartas que roben una carta en espesifico y para sacar del deck el rookie inicial
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
    // Getters and Setters
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
