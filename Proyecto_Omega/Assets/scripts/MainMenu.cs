﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Deck()
    {
        SceneManager.LoadScene("Deck");
    }

    public void Tamer()
    {
        
    }

    public void IA()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}