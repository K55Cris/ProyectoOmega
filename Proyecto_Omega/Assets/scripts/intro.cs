using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class intro : MonoBehaviour {
    public VideoPlayer Video;
	// Use this for initialization
	void Start () {
        if (DataManager.instance.IntroVisto)
        {
            Continuar();
        }else
        Invoke("Continuar", (float)Video.clip.length);
	}
    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void Continuar()
    {
        if (PlayerManager.instance.bienvenida)
        {
            MainMenu.instance.PanelNewUser.SetActive(true);
            MainMenu.instance.Opciones.SetActive(false);
            MainMenu.instance.Tutorial.SetActive(false);
        }
        else
        {
            MainMenu.instance.Opciones.SetActive(true);
            MainMenu.instance.Tutorial.SetActive(true);
        }

        if (gameObject)
        {
            DataManager.instance.IntroVisto = true;
            gameObject.SetActive(false);
          
        }

      
        SoundManager.instance.PlayMusic(Sound.MainMenu);
    }
    public void OnMouseDown()
    {
        Continuar();
       
    }
    public void CloseNewUser()
    {
        MainMenu.instance.Opciones.SetActive(true);
        MainMenu.instance.Tutorial.SetActive(true);
    }
}
