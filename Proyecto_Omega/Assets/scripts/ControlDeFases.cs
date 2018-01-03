using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlDeFases : MonoBehaviour
{

    public static ControlDeFases instance = null;

    public enum Phase
    {
        GameSetup = 0, PreparationPhase = 1, EvolutionPhase = 2, EvolutionRequirements = 3, FusionRequirements = 4,
        AppearanceRequirements = 5, BattlePhase = 6, PointCalculationPhase = 7, EndPhase = 8
    };

    //private Player player1, player2, currentPlayer;

    public int pointGaugePlayer1, pointGaugePlayer2, actualPhase;
    private bool faseFinalizada, rendicion;
    

    // Use this for initialization
    void Start()
    {
        pointGaugePlayer1 = 100;
        pointGaugePlayer2 = 100;
        actualPhase = 0;
        faseFinalizada = false;
        rendicion = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (faseFinalizada)
        {
            Phase faseActual = (Phase)actualPhase;
            Console.WriteLine(faseActual);
            SiguienteFase();
        }
    }

    //Aumenta en 1 el indice de la fase actual y ejecuta la siguiente.
    private void SiguienteFase()
    {
        actualPhase++;
        switch (actualPhase)
        {
            case 0:
                StartGameSetup();
                break;
            case 1:
                StartPreparationPhase();
                break;
            case 2:
                StartEvolutionPhase();
                break;
            case 3:
                CheckEvolutionRequirements();
                break;
            case 4:
                CheckFusionRequirements();
                break;
            case 5:
                CheckAppearanceRequirements();
                break;
            case 6:
                StartBattlePhase();
                break;
            case 7:
                StartPointCalculationPhase();
                break;
            case 8:
                StartEndPhase();
                break;
            default:
                Console.WriteLine("Error en el cambio de fase");
                break;
        }
        faseFinalizada = true;
    }

    //INICIO metodos para cada fase
    private void StartGameSetup()
    {
        //A ejecutar por ambos jugadores:
        //Colocar Digimon Nivel III boca abajo en la Digimon Box
        //Barajar Net Ocean
        //Colocar primera carta del mazo boca abajo en la Point Gauge
        SeleccionPrimerJugador();
        //Robar 6 cartas del Net Ocean
    }

    private void StartPreparationPhase()
    {
        //El jugador atacante realiza su Preparation Phase en primer lugar
        //Descartar cartas de la mano y Option Slots si se desea al Dark Area siempre que esta accion no este impedida
        //Robar hasta tener 6 cartas en la mano o hasta que se agoten las cartas disponibles en el Net Ocean
        //Reemplazar Digimon Nivel III de la Digimon Box por otro Digimon Nivel III boca abajo desde la mano sobre el anterior si se desea
        //...

        //Antes de terminar la fase debe ejecutarla el jugador defensor
    }

    private void StartEvolutionPhase()
    {

    }

    private void CheckEvolutionRequirements()
    {

    }

    private void CheckFusionRequirements()
    {

    }

    private void CheckAppearanceRequirements()
    {

    }

    private void StartBattlePhase()
    {

    }

    private void StartPointCalculationPhase()
    {

    }

    private void StartEndPhase()
    {
        //Cambiar jugador activo
        actualPhase = 0;
    }
    //FIN metodos para cada fase

    //Comprueba si la vida de algun jugador se ha reducido a 0 o alguno de los jugadores se ha rendido.
    private bool CumplidaCondicionVictoria()
    {
        if (pointGaugePlayer1 <= 0 || pointGaugePlayer2 <= 0 || rendicion) return true; //Fin de la partida.
        return false;
    }
    
    //Fija el jugador activo al inicio de la partida
    public void SeleccionPrimerJugador()
    {
        //Por ahora se elige aleatoriamente no por Piedra Papel Tijera

        int aux = Random.Range(0,2);
        if(aux == 0)
        {
            //currentPlayer = player1;
        }
        else
        {
            //CurrentPlayer = player2;
        }
    }
    

}