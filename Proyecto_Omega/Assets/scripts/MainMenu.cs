using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    //Comentario Random

    public void Quit()
    {
        SoundManager.instance.PlaySfx(Sound.Out);
        Application.Quit();
    }
    public void vSTamer()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        LevelLoader.instance.CargarEscena("Recompensa");
    }
    public void VsIA()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        LevelLoader.instance.CargarEscena("vsIA");
    }
    public void DeckEditor()
    {
        SoundManager.instance.PlaySfx(Sound.Enter);
        LevelLoader.instance.CargarEscena("DeckEditor");
    }
}
