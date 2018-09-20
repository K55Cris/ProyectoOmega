using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    /// <summary>
    /// Carga la escena deseada
    /// </summary>
    /// <param name="nombreDeLaEscena">
    /// String con el nombre de la escena en el proyecto
    /// </param>
    public void CargarEscena(string nombreDeLaEscena)
    {
        SceneManager.LoadSceneAsync(nombreDeLaEscena);
    }
}
