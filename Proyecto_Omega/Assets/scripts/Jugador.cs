using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {

    public string Apodo;
    public int PointGauge;
    public List<string> Mano;
    public Deck Deck;
    public DarkArea DarkArea;

    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Roba una carta y la coloca en la mano.
    /// </summary>
    public void Robar()
    {
        //Mano.Add(Deck.Robar());
    }

    /// <summary>
    /// Descarta las cartas seleccionadas.
    /// </summary>
    public void Descartar(string idCartaDescartada)
    {
        Mano.Remove(idCartaDescartada);
        DarkArea.Meter(idCartaDescartada);
    }

    /// <summary>
    /// Juega la carta indicada por parametro.
    /// </summary>
    public void JugarCarta(string idCartaJugada)
    {
        Mano.Remove(idCartaJugada);
        //cartaJugada.JugarCarta();
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
        Mano.ForEach(carta => {
            //if(carta.getTipo() == "OptionCard") then carta.JugarCarta();
        });
    }

    /// <summary>
    /// Colocara una o mas cartas de digimon comprobando el nivel de las que hay colocadas.
    /// </summary>
    public void ColocarEvolucion()
    {
        Mano.ForEach(carta => {
            //if(carta.getTipo() == "DigiCard" & esSiguienteDigievolucion) => carta.JugarCarta();
        });
    }

    /// <summary>
    /// Comprobará la mano y descartara digievoluciones no utiles en ese momento.
    /// </summary>
    public void BalancearMano()
    {
        int cantidadCartasDigimon = 0;
        int cantidadCartasOpcion = 0;

        Mano.ForEach(carta => {
            //Contar cuantas cartas de cada hay 

        });

        //Mano.Descartar(Las que correspondan);
    }

    /// <summary>
    /// Jugará si se requere y se puede un digimon support.
    /// </summary>
    public void JugarDigimonSupport()
    {
        
    }

    public void activarOptionCard()
    {

    }
}
