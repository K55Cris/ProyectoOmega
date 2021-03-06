﻿using System.Collections;
using UnityEngine;

public enum Sound
{
    Evolucion, SetCard, Evolucion2,Joggres, AtaqueA, AtaqueB, AtaqueC, Tornado, MainMenu, Duelo, Recompensa,
    Enter, Out, Barajear, tutorial, Wincolecionable , Heal, ButterFly,BreakUp,DarkDuel,Takecard,Changephase,Apocaly,Alpha,
    Card,NecroMagic,Neutralize
}
public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Evolucion")]
    public AudioClip EvolucionRoquin;
    public AudioClip EvolucionChampion;
    public AudioClip Joggres;

    [Header("Musica de Fondo")]
    public AudioClip MainMenu;
    public AudioClip Recompensas;
    public AudioClip Duelo;
    public AudioClip Tutorial;
    public AudioClip ButterFly;
    public AudioClip BreakUp;
    public AudioClip DarkDuel;
    public AudioClip Apocaly;
    public AudioClip Alpha;
    public AudioClip Card;

    [Header("Colocar Cartas")]
    public AudioClip SetCard1;
    public AudioClip SetCard2;
    public AudioClip SetCard3;
    public AudioClip SetCard4;
    public AudioClip TakeCard;
    public AudioClip TakeCard2;
    public AudioClip TakeCard3;
    public AudioClip TakeCard4;
    public AudioClip TakeCard5;
    public AudioClip TakeCard6;
    public AudioClip TakeCard7;
    public AudioClip Barajear;

    [Header("Ataques")]
    public AudioClip ChoqueCristal;
    public AudioClip DisparoDeFuego;
    public AudioClip DisparoElectrico;
    public AudioClip Tornado;


    [Header("Main Menu")]
    public AudioClip EnterButton;
    public AudioClip OutButton;

    [Header("Efectos")]
    public AudioClip Wincolecionable;
    public AudioClip Heal;
    public AudioClip CambioPhase;
    public AudioClip NecroMagic;
    public AudioClip Neutralize;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        GetSettings();
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
        if (musicSource.clip != GetClip(audioName))
        {
            musicSource.clip = GetClip(audioName);
            StartCoroutine(DelayMusic(musicDelay));
        }
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
            case Sound.Tornado:
                audioClip = Tornado;
                break;
            case Sound.MainMenu:
                audioClip = MainMenu;
                break;
            case Sound.Recompensa:
                audioClip = Recompensas;
                break;
            case Sound.Duelo:
                audioClip = Duelo;
                break;
            case Sound.tutorial:
                audioClip = Tutorial;
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
            case Sound.Takecard:
                int ran2 = Random.Range(1, 8);
                switch (ran2)
                {
                    case 1:
                        audioClip = TakeCard;
                        break;
                    case 2:
                        audioClip = TakeCard2;
                        break;
                    case 3:
                        audioClip = TakeCard3;
                        break;
                    case 4:
                        audioClip = TakeCard4;
                        break;
                    case 5:
                        audioClip = TakeCard3;
                        break;
                    case 6:
                        audioClip = TakeCard4;
                        break;
                    case 7:
                        audioClip = TakeCard3;
                        break;
                    case 8:
                        audioClip = TakeCard4;
                        break;
                }
                break;
            case Sound.Enter:
                audioClip = EnterButton;
                break;
            case Sound.Barajear:
                audioClip = Barajear;
                break;
            case Sound.Out:
                audioClip = OutButton;
                break;
            case Sound.Wincolecionable:
                audioClip = Wincolecionable;
                break;
            case Sound.BreakUp:
                audioClip = BreakUp;
                break;
            case Sound.ButterFly:
                audioClip = ButterFly;
                break;
            case Sound.DarkDuel:
                audioClip = DarkDuel;
                break;
            case Sound.Heal:
                audioClip = Heal;
                break;
            case Sound.Changephase:
                audioClip = CambioPhase;
                break;
            case Sound.Alpha:
                audioClip = Alpha;
                break;
            case Sound.Apocaly:
                audioClip = Apocaly;
                break;
            case Sound.Card:
                audioClip = Card;
                break;
            case Sound.NecroMagic:
                audioClip = NecroMagic;
                break;
            case Sound.Neutralize:
                audioClip = Neutralize;
                break;
            case Sound.Joggres:
                audioClip = Joggres;
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

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SaveMusic", musicSource.volume);
        PlayerPrefs.SetFloat("EfectMusic", sfxSource.volume);
    }
    public void GetSettings()
    {
        if (PlayerPrefs.HasKey("SaveMusic"))
        {
            musicSource.volume = PlayerPrefs.GetFloat("SaveMusic");
        }
        if (PlayerPrefs.HasKey("EfectMusic"))
        {
            sfxSource.volume = PlayerPrefs.GetFloat("EfectMusic");
        }
    }
    public float GetSliderVolume(int slider)
    {
        if (slider == 0)
        {
            return musicSource.volume;
        }
        else
        {
            return sfxSource.volume;
        }
    }
}
