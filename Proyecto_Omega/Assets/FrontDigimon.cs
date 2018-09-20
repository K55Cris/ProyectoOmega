using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontDigimon : MonoBehaviour {
    public Image Digimon;
    public Image Ataque;
    public List<Sprite> Ataques;
    // Use this for initialization
    public void Start()
    {
        QuitarDigimon();
    }
    public void RevelarDigimon(int id, string ataque)
    {
        Digimon.color = Color.white;
        Ataque.color = Color.white;
        Digimon.sprite = DataManager.instance.GetSprite8(id);
        switch (ataque)
        {
            case "A":
                Ataque.overrideSprite = Ataques[0];
                break;
            case "B":
                Ataque.overrideSprite = Ataques[1];
                break;
            case "C":
                Ataque.overrideSprite = Ataques[2];
                break;
        }
    }
    public void QuitarDigimon()
    {
        StartCoroutine(Whaith());
    }
    public IEnumerator Whaith()
    {

        Digimon.color = new Color32(255, 255, 255, 0);
        Ataque.color = new Color32(255, 255, 255, 0);
        yield return new WaitForEndOfFrame();
    }
}
