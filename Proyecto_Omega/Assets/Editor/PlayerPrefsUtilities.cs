using UnityEditor;
using UnityEngine;

public class PlayerPrefsUtilities : MonoBehaviour
{
    [MenuItem("PlayerPrefs/Delete All")]
    static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
