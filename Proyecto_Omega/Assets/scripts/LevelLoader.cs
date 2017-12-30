﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {
    public GameObject loadingScreen;
    public Slider slider;
    public void loadLevel(int sceneIndex) 
    {
        StartCoroutine(LoadAsynchronouly(sceneIndex));
    }

    IEnumerator LoadAsynchronouly(int sceneIndex) 
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);
        while (!operation.isDone) 
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
    }
}