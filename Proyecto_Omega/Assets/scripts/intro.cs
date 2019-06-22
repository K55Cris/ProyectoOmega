using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class intro : MonoBehaviour {
   // public VideoPlayer Video;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(whaithFrame());
	}
    private void Awake()
    {
    //    GetComponent<MeshRenderer>().enabled = true;
    }

    public IEnumerator whaithFrame()
    {
        yield return new WaitForEndOfFrame();
        if (DataManager.instance.IntroVisto)
        {
            Continuar();
        }
        else
        {
            //   Invoke("Continuar", (float)Video.clip.length);
            Continuar();
        }
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
        PlayerManager.instance.bienvenida = false;
        MainMenu.instance.Opciones.SetActive(true);
        MainMenu.instance.Tutorial.SetActive(true);
        PlayerManager.instance.SavePlayer();
    }
}
