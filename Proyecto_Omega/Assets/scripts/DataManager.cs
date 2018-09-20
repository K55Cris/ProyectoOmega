using DigiCartas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour {

    public static DataManager instance;
    public TextAsset jsonData;

    public Cartas ColeccionDeCartas;
    public List<DigiCarta> TodasLasCartas;


    public Texture[] arrayImage;
    public Sprite[] arraySprites8;

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

    public void FadeCanvas(CanvasGroup _Canvas)
    {
        StartCoroutine(_FadeCanvas(_Canvas));
    }
    public IEnumerator _FadeCanvas(CanvasGroup _Canvas)
    {
        while (_Canvas.alpha>0)
        {
            yield return new WaitForSeconds(0.01f);
            _Canvas.alpha-=0.08f;
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
}
