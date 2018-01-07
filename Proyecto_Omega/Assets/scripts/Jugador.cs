using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {

    public string Apodo;
    public int PointGauge;
    public List<string> Mano;
    public Deck Deck;
    
    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Roba una carta y la coloca en la mano.
    /// </summary>
    public void Robar()
    {
        
    }

    /// <summary>
    /// Descarta las cartas seleccionadas.
    /// </summary>
    public void Descartar()
    {
        
    }


    /*----------METODOS PARA LA IA----------*/

    /// <summary>
    /// Cambiará el Digimon Nivel III activo.
    /// </summary>
    public void CambiarRookie()
    {

    }
    
    /// <summary>
    /// Colocara Option Cards de la mano a la mesa.
    /// </summary>
    public void ColocarOptionCards()
    {
        
    }

    /// <summary>
    /// Colocara una o mas cartas de digimon comprobando el nivel de las que hay colocadas.
    /// </summary>
    public void ColocarEvolucion()
    {
        
    }

    /// <summary>
    /// Comprobará la mano y descartara digievoluciones no utiles en ese momento.
    /// </summary>
    public void BalancearMano()
    {
        
    }

    /// <summary>
    /// Jugará si se requere y se puede un digimon support.
    /// </summary>
    public void JugarDigimonSupport()
    {
        
    }
}
