using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour {

    public static LevelLoader instance;
    public ParticleSystem ps;
    public CanvasGroup Fondo;
    public GameObject Guilmon;
    public TextMeshProUGUI Carga;
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

        // Abrimos pantalla de carga 
        Fondo.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        DataManager.instance.FadeCanvas(Fondo, true);

        yield return new WaitForSeconds(0.5f);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(nombreDeLaEscena);
        async.allowSceneActivation = false;
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            Carga.text = async.progress.ToString("N0") + "%";
            if (async.progress==.9F)
            {
                Carga.text = "100%";
                ps.Pause();
                async.allowSceneActivation = true;
                yield return new WaitForSeconds(0.5f);
                Guilmon.gameObject.SetActive(false);
                DataManager.instance.FadeCanvas(Fondo, false);
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return new WaitForEndOfFrame();

    }

}
