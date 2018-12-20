using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class ItemMenuMap : MonoBehaviour {
    public ParticleSystem Nodo;
    public ParticleSystem General;
    public int ID;
    public Sprite IAImage;
    public string Nombre;
    public string NombreMazo;
    public int wins;
    public int Loses;
    public List<ParticleSystem> Conexiones;
    public bool Completado=false;
	// Use this for initialization
	void Awake ()
    {
        // Apagamos Todas las Particulas
        foreach (var item in Conexiones)
        {
            item.Stop();
        }
        Nodo.Stop();
        General.Stop();

	}
	public void Cargar(Progreso Pros)
    {
        if (Pros.Cerca)
        {
            General.Play();
            wins = Pros.Victorias;
            Loses = Pros.Derrotas;
            if (Pros.Completo)
            {
                Nodo.Play();
                foreach (var item in Conexiones)
                {
                    item.Play();
                }
            }
        }
    }
    public void Centrar()
    {
        Vector3 NewPos = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position = NewPos;
        MapDuelManager.instance.ShowInfo(this);
    }

}
