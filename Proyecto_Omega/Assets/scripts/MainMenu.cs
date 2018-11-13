using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //Comentario Random

    public void Quit()
    {
        Application.Quit();
    }
    public void vSTamer()
    {
        LevelLoader.instance.CargarEscena("Recompensa");
    }
    public void VsIA()
    {
        LevelLoader.instance.CargarEscena("vsIA");
    }
    public void DeckEditor()
    {

        LevelLoader.instance.CargarEscena("DeckEditor");
    }
}
