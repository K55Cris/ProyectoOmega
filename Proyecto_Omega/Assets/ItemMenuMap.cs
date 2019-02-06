using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public int NivelNecesario;
    public List<ParticleSystem> Conexiones;
    public List<ItemMenuMap> NodosVecinos= new List<ItemMenuMap>();
    public bool Accesible=false;
    public IADecks Decks;
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
            Accesible = true;
            wins = Pros.Victorias;
            Loses = Pros.Derrotas;
            if (Pros.Completo)
            {  

                // Cargar Nivel completo pero sin El nivel necesario
                if (PlayerManager.instance.Jugador.Nivel < NivelNecesario)
                {
                    var main = General.main;
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    main.startColor = Color.red;
                    var main2 = Nodo.main;
                    main2.startColor = Color.red;
                    foreach (var item in Conexiones)
                    {
                        var mainitem = item.main;
                        mainitem.startColor = Color.red;
                        item.Play();
                    }
                }
                else
                {
                    foreach (var item in Conexiones)
                    {
                        item.Play();
                    }
                }
                General.Play();
                Nodo.Play();
     
                foreach (var item in NodosVecinos)
                {
                    item.Accesible = true;
                    PlayerManager.instance.NodoCerca(item.ID);
                }   
            }
            else
            {
                // Cargar Nivel no completo pero cerca
                if (PlayerManager.instance.Jugador.Nivel < NivelNecesario)
                {
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    var main = General.main;
                    main.startColor = Color.red;
                }
            }
            General.Play();
        }
        else
        {
            Accesible = false;
            // Cargar Nivel no completo no cerca y sin nivel necesario
            if(PlayerManager.instance.Jugador.Nivel< NivelNecesario)
            {
                this.gameObject.GetComponent<Image>().color = Color.red;
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
