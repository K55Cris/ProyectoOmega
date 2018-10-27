using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    Evolucion, SetCard, Evolucion2 , AtaqueA,AtaqueB,AtaqueC
}
public class SoundManager : MonoBehaviour {

    public static SoundManager instance;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Evolucion")]
    public AudioClip EvolucionRoquin;
    public AudioClip EvolucionChampion;

    [Header("Colocar Cartas")]
    public AudioClip SetCard1;
    public AudioClip SetCard2;
    public AudioClip SetCard3;
    public AudioClip SetCard4;

    [Header("Ataques")]
    public AudioClip ChoqueCristal;
    public AudioClip DisparoDeFuego;
    public AudioClip DisparoElectrico;

    public AudioClip recompesas;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public void PlaySfx(Sound audioName, bool playOneShot = true, bool loops = false, float startTime = 0f)
    {
        if (playOneShot)
            sfxSource.PlayOneShot(GetClip(audioName));
        else
        {
            sfxSource.clip = GetClip(audioName);
            sfxSource.time = startTime;
            sfxSource.Play();
        }

        if (loops)
        {
            sfxSource.loop = true;
        }
        else
            sfxSource.loop = false;
    }

    public void PlayMusic(Sound audioName, float musicDelay = 0)
    {
        musicSource.clip = GetClip(audioName);



        StartCoroutine(DelayMusic(musicDelay));
    }
    IEnumerator DelayMusic(float musicDelay)
    {
        yield return new WaitForSeconds(musicDelay);
        musicSource.Play();
    }

    /// <summary>
    /// Funcion para encontrar el clip de audio basandose en el nombre del enum solicitado
    /// </summary>
    /// <param name="audioName"></param>
    /// <returns></returns>
    public AudioClip GetClip(Sound audioName)
    {
        AudioClip audioClip = null;
        switch (audioName)
        {
            case Sound.Evolucion:
                audioClip = EvolucionRoquin;
                break;
            case Sound.Evolucion2:
                audioClip = EvolucionChampion;
                break;
            case Sound.AtaqueA:
                audioClip = DisparoDeFuego;
                break;
            case Sound.AtaqueB:
                audioClip = DisparoElectrico;
                break;
            case Sound.AtaqueC:
                audioClip = ChoqueCristal;
                break;
            case Sound.SetCard:
                int ran = Random.Range(1, 4);
                switch (ran)
                {
                    case 1:
                        audioClip = SetCard1;
                        break;
                    case 2:
                        audioClip = SetCard2;
                        break;
                    case 3:
                        audioClip = SetCard3;
                        break;
                    case 4:
                        audioClip = SetCard4;
                        break;
                }
                break;
        }
        return audioClip;
    }
    /// <summary>
    /// Ajusta el volumen del audiosource de musica
    /// </summary>
    /// <param name="newVol"></param>
    public void AdjustMusicVolume(float newVol)
    {
        musicSource.volume = newVol;
    }

}
