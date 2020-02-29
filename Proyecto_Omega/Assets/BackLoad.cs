using UnityEngine;

public class BackLoad : MonoBehaviour
{
    public void Back()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "VsTamer")
        {
            MultiPlayer.instance._Lobby.RemoveAll();
        }
        LevelLoader.instance.CargarEscena("Main Menu");
    }
}
