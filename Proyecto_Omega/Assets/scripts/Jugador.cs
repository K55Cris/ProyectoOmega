using System;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour {

    public string Apodo;
    public int PointGauge;
    public List<string> Mano;
    public Deck Deck;
    public DarkArea DarkArea;
    //public DigimonBox DigimonBox;
    private bool optionSlotLibre;

    private enum EtapaDigimon
    {
        III, IV, PERFECT, ULTIMATE
    };

    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Roba una carta y la coloca en la mano.
    /// </summary>
    public void Robar()
    {
        Mano.Add(Deck.Robar());
    }

    /// <summary>
    /// Descarta las cartas seleccionadas.
    /// </summary>
    /// <param name="idCartaDescartada">ID de la carta a descartar</param>
    public void Descartar(string idCartaDescartada)
    {
        Mano.Remove(idCartaDescartada);
        DarkArea.Meter(idCartaDescartada);
    }

    /// <summary>
    /// Juega la carta indicada por parametro.
    /// </summary>
    /// <param name="idCartaJugada">ID de la carta a jugar</param>
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
        //Por ahora que la ia no cambia el rookie
    }

    /// <summary>
    /// Colocara Option Cards de la mano a la mesa.
    /// </summary>
    public void ColocarOptionCards()
    {
        Mano.ForEach(carta => {
            if (optionSlotLibre)
            {
                /*
                if (carta.getTipo() == "OptionCard")
                {
                    carta.JugarCarta();
                }
                */
            }
        });
    }

    /// <summary>
    /// Colocara una o mas cartas de digimon comprobando el nivel de las que hay colocadas.
    /// </summary>
    public void ColocarEvolucion()
    {
        /*
        bool cartaJugada = false;
        Mano.ForEach(carta => {
            if (carta.getTipo() == "DigiCard" & carta.getNivel() == nivelDigimonActivo + 1 & !cartaJugada)
            {
                carta.JugarCarta();
                cartaJugada = true;
            }
        });
        */
    }

    /// <summary>
    /// Comprobará la mano y descartara digievoluciones no utiles en ese momento.
    /// </summary>
    public void BalancearMano()
    {
        //Requiere mucha optimizacion


        int cantidadCartasDigimon = 0;
        int cantidadCartasOpcion = 0;


        Mano.ForEach(carta => {
            /*
            if (carta.getTipo() == "OptionCard")
            {
                cantidadCartasOpcion++;
            }
            else
            {

                cantidadCartasDigimon++;
            }
            */
        });

        int cartasSobrantes = cantidadCartasDigimon - cantidadCartasOpcion;

        if (cartasSobrantes != 0)
        {
            string tipoCartaSobrante = (cartasSobrantes > 0) ? "DigimonCard" : "OptionCard";
            cartasSobrantes = Math.Abs(cartasSobrantes);




            //Contar cartas digimon de cada nivel.
            //Quedarse al menos con una de cada.





            //Pendiente comprobar digievoluciones para descartar las peores
            Mano.ForEach(carta => {
                /*
                if (carta.getTipo() == tipoCartaSobrante & cartasSobrantes!=0)
                {
                    Descartar(carta);
                    cartasSobrantes--;
                }
                */
            });
        }

    }

    /// <summary>
    /// Jugará si se requere y se puede un digimon support.
    /// </summary>
    public void JugarDigimonSupport()
    {
        //A contemplar mas adelante
    }

    /// <summary>
    /// Activará si se puede una option card.
    /// </summary>
    public void activarOptionCard()
    {
        //Iterar sobre las option slot y activar la que se requiera.
    }
}
