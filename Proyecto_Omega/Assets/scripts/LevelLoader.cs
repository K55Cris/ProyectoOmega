using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DigiCartas;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

    public static LevelLoader instance;
    public ParticleSystem ps;
    public CanvasGroup Fondo, NewItem;
    public Image Guilmon;
    public TextMeshProUGUI Carga;
    public Animator AMColeccionable;
    public List<GameObject> Contenedores;
    public Button SalirColecionable;
    public Transform PanelColecion;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;

        }

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
        ps.Stop();
    }


    /// <summary>
    /// Carga la escena deseada
    /// </summary>
    /// <param name="nombreDeLaEscena">
    /// String con el nombre de la escena en el proyecto
    /// </param>
    public void CargarEscena(string nombreDeLaEscena)
    {
        ps.Play();
        Guilmon.gameObject.SetActive(true);
        Carga.text = "";
        StartCoroutine(LoadNewScene(nombreDeLaEscena));
    }
    public IEnumerator LoadNewScene(string nombreDeLaEscena)
    {
        yield return null;
        // Abrimos pantalla de carga 
        Fondo.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        DataManager.instance.FadeCanvas(Fondo, true);

        yield return new WaitForSeconds(0.5f);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(nombreDeLaEscena);
        async.allowSceneActivation = true;
        Carga.text = async.progress.ToString("N0") + "%";
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            Carga.text = async.progress.ToString("N0") + "%";
            if (async.progress>=.9F)
            {
                Carga.text = "100%";
                ps.Pause();
                async.allowSceneActivation = true;
                yield return new WaitForSeconds(0.5f);
                
            }
        }
        yield return new WaitForEndOfFrame();
        Guilmon.gameObject.SetActive(false);
        DataManager.instance.FadeCanvas(Fondo, false);
        yield return new WaitForSeconds(0.5f);
    }

    public void GetNewItem(List<int> ID)
    {
        SalirColecionable.interactable = false;
        PanelColecion.transform.localScale = new Vector3(0,0,1);
        foreach (var item in Contenedores)
        {
            item.SetActive(false);
        }
        DataManager.instance.FadeCanvas(NewItem,true);
        List<Coleccionables> TempColec = new List<Coleccionables>(); 
        foreach (var item2 in ID)
        {
            foreach (var item in DataManager.instance.ListaColeccionables)
            {
                if (item.ID == item2)
                {
                    //Coleccionable encontrado
                    TempColec.Add(item);
                    PlayerManager.instance.SetNewColeccionable(item);
                }
            }
        }
        //Cargar Datos
        SoundManager.instance.PlaySfx(Sound.Wincolecionable);
        for (int i = 0; i < TempColec.Count; i++)
        {
            Contenedores[i].SetActive(true);
            Contenedores[i].GetComponent<ColeccionableItem>().CargarData(TempColec[i]);
        }
        Invoke("ShowItems", 0.5f);
}
    
    public void ShowItems()
    {
        AMColeccionable.Play("OpenNewItem");
        SalirColecionable.interactable = true;
    }
    
    public void OutColecionable()
    {
        DataManager.instance.FadeCanvas(NewItem);
    }
    
}
