using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderValude : MonoBehaviour
{
    public Image Handler;
    public List<Sprite> Digis;
    public Slider Barra;
    public int Slider;
    // Use this for initialization

    public void Cambio()
    {

        if (Barra.value >= .75f)
        {
            Handler.sprite = Digis[3];
        }
        else if (Barra.value >= .50f)
        {
            Handler.sprite = Digis[2];
        }
        else if (Barra.value >= .25f)
        {
            Handler.sprite = Digis[1];
        }
        else if (Barra.value < .25f)
        {
            Handler.sprite = Digis[0];
        }
    }
    public void ChangeMusic()
    {
        SoundManager.instance.musicSource.volume = Barra.value;
    }
    public void ChangeEfects()
    {
        SoundManager.instance.sfxSource.volume = Barra.value;
    }
    public void Open()
    {
        Barra.value = SoundManager.instance.GetSliderVolume(Slider);
    }

}
