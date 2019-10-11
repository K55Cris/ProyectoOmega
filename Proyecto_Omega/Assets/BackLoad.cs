using UnityEngine;

public class BackLoad : MonoBehaviour
{
    public void Back()
    {
        LevelLoader.instance.CargarEscena("Main Menu");
    }
}
