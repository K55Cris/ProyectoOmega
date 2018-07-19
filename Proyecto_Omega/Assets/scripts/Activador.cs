using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Activador : MonoBehaviour {
    public UnityEvent Evento;
    public void Start()
    {
        this.enabled = false;
    }
    public void OnEnable()
    {
        Invoke("Apagar", 0.4f);
        Evento.Invoke();
    }
    public void Apagar()
    {
        SoundManager.instance.PlaySfx(Sound.AtaqueC);
    }

}
