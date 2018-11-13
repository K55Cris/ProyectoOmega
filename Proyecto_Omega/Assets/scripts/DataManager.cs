using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DataManager : MonoBehaviour {

    public static DataManager instance;
    public TextAsset jsonData;
    public bool IntroVisto = false;
    public Cartas ColeccionDeCartas;
    public List<DigiCarta> TodasLasCartas;
    private UnityAction<int> _Loaction;
    public List<Sprite> ImagesType;
    public Texture[] arrayImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        LoadCartas();
    }
    public DigiCarta GetDigicarta(int codigo)
    {
        foreach (var item in TodasLasCartas)
        {
            if (item.id==codigo)
            {
                return item;
            }
        }
        return new DigiCarta(); // error
    }
   
    public void LoadCartas()
    {
        ColeccionDeCartas = JsonUtility.FromJson<Cartas>(jsonData.text);
        TodasLasCartas = ColeccionDeCartas.DigiCartas;
        foreach (var item in ColeccionDeCartas.CartaOption)
        {
            TodasLasCartas.Add(item);
        }
        arrayImage = Resources.LoadAll<Texture>("Digimon");
     //   arraySprites = Resources.LoadAll<Sprite>("Digimon");

    }
    public Texture GetTextureDigimon(int IDigimon)
    {
        foreach (var item in arrayImage)
        {
            if (item.name == IDigimon.ToString())
                return item;
        }
        return null;
    }
    public Sprite GetSprite(int id)
    {
       Sprite arraySprite = Resources.Load<Sprite>("Digimon/"+id.ToString());

        return arraySprite;
    }
    public Sprite GetSprite8(int id)
    {
        Sprite arraySprite = Resources.Load<Sprite>("Digimon8/" + id.ToString());

        return arraySprite;
    }

    public void FadeCanvas(CanvasGroup _Canvas,bool Sw=false)
    {
        if (!Sw)
        {
            StartCoroutine(_FadeCanvas(_Canvas));
        }
        else
        {
            StartCoroutine(InFadeCanvas(_Canvas));
        }
      
    }
    public IEnumerator _FadeCanvas(CanvasGroup _Canvas)
    {
        _Canvas.blocksRaycasts = true;
        while (_Canvas.alpha>0)
        {
            yield return new WaitForSeconds(0.01f);
            _Canvas.alpha-=0.08f;
        }
        _Canvas.blocksRaycasts = false;
    }
    public IEnumerator InFadeCanvas(CanvasGroup _Canvas)
    {
        _Canvas.blocksRaycasts = true;
        while (_Canvas.alpha < 1)
        {
            yield return new WaitForSeconds(0.01f);
            _Canvas.alpha += 0.08f;
        }
        _Canvas.blocksRaycasts = false;
    }

    public void EndFrame(UnityAction<string> funcion)
    {
        StartCoroutine(_EndFrame(funcion));
    }
    public IEnumerator _EndFrame(UnityAction<string> funcion)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        funcion("");
    }
    public bool bandera = true;

    public void WinCard(UnityAction<int> Loaction)
    {
        _Loaction = Loaction;
        // revisamos si de casualidad ya tiene todas las cartas
        if (PlayerManager.instance.Jugador.IDCartasDisponibles.Count == 60)
        {
            foreach (var item in PlayerManager.instance.Jugador.IDCartasDisponibles)
            {
                if (item.Cantidad < 9)
                {
                    JuegoCompleto();
                    break; 
                }
            }
        }
        else
        {
            GetCard();
        }
        
       
    }
    public void GetCard()
    {
        int  IDCard = Random.Range(1, TodasLasCartas.Count + 1);
        bool Encontrado = false;
        //Revisamos si la carta es nueva 
        foreach (var item in PlayerManager.instance.Jugador.IDCartasDisponibles)
        {
            if (item.ID == IDCard)
            {
                if (item.Cantidad < 9)
                {
                    Encontrado = true;
                    // agregamos y salimos
                    PlayerManager.instance.NewCard(IDCard);
                    _Loaction.Invoke(IDCard);
                    break;
                }
                else
                    GetCard();
            }
        }
        if (!Encontrado)
        {
            // agregamos y salimos
            PlayerManager.instance.NewCard(IDCard);
            _Loaction.Invoke(IDCard);
        } 
    } 

    public void JuegoCompleto()
    {
        // mensaje de Ganador
        PlayerManager.instance.Jugador.ALLCards = true;
        PlayerManager.instance.SavePlayer();
        _Loaction.Invoke(61);
    }

    public Sprite GetSpriteForType(DigiCarta Datos)
    {
        Sprite Tipo=null;
        if (Datos.IsSupport)
        {
            switch (Datos.ListaCatrgoria[0])
            {
                case "Item":
                    Tipo = ImagesType[3];
                    break;
                case "Program":
                    Tipo = ImagesType[4];
                    break;
            }
        }
        else
        {
            switch (Datos.TipoBatalla)
            {
                case "A":
                    Tipo = ImagesType[0];
                    break;
                case "B":
                    Tipo = ImagesType[1];
                    break;
                case "C":
                    Tipo= ImagesType[2];
                    break;
            }
        }
        return Tipo;
    }
}
