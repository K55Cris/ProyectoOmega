using UnityEngine;
using UnityEngine.Events;
using DigiCartas;
public class StaticRules : MonoBehaviour
{
    public static StaticRules instance = null;
    // Use this for initialization
    public int Turno = 0;
    public int LifepointsPlayer1 = 100;
    public int LifepointsPlayer2 = 100;

    public int NowPhase = 0;

    public enum Phases { GameSetup = 0, PreparationPhase = 1, EvolutionPhase = 2, EvolutionRequirements = 3, FusionRequirements = 4,
                           AppearanceRequirements = 5, BattlePhase = 6, PointCalculationPhase = 7, EndPhase = 8, };

    public void Start()
    {
        NowPhase = 0;
        WhoFirstPlayer();
    }

    public void SelectDigimonChild()
    {
        StaticRules loRule = FailSafeInstance();
        // player 1 selecciona digimon child
        // player 2 selecciona digimon child
        // barajear mazo player 1
    }

    public void GameSetupPhase()
    {

    }


    public void WhoFirstPlayer()
    {
        StaticRules loRule = FailSafeInstance();
        // juego al azar para saber quien va primero
    }

    /// <summary>
    /// Esto es para que el objeto siempre este en ecena y que siempre exista
    /// </summary>
    /// <returns></returns>
    public static StaticRules FailSafeInstance()
    {
        if (instance == null)
        {
            
            GameObject loGameObject = new GameObject("Reglas", typeof(StaticRules));
            return loGameObject.GetComponent<StaticRules>();
        }
        return instance;
    }

    public static void InvocarDigimon(int player)
    {
        StaticRules loRule = FailSafeInstance();
    }

    public static int PerderPuntos()
    {
        StaticRules loRule = FailSafeInstance();
        // metodo para calcular cuantos puntos se perderan 
        int puntos = 0;
        // calculado segun tipo y quien fue el atacante
        return puntos;
    }
    public static string Habilidades(DigiCarta digimon)
    {
        StaticRules loRule = FailSafeInstance();
        // aqui se verifican que es lo que hacen las habilidades 
        string Habilidad="";
        switch (digimon.Habilidad)
        {
            default:
                Habilidad = "Volador";
                break;
        }
        return Habilidad;
    }
    public static void MoveToNetOcean(DigiCarta digimonActivador, DigiCarta DigimonAfectado)
    {
        // se verifica que dicho digimon que ative este efecto cuente con el  si es asi se manda a net ocean el digimon afectado
    }

    public static void RecuperPuntos(DigiCarta Digimon, int player)
    {
        StaticRules loRule = FailSafeInstance();
        // se verifica que el digicarta tenga esa habilidad 
        if (Habilidades(Digimon)=="RestoreLife")
        if (player==1)
        {
                loRule.LifepointsPlayer1 += 0; // aqui va otro metoido para calucular cuanto sube dicha habilidad especial
        }
        else
        {
                loRule.LifepointsPlayer2 += 0; // aqui va otro metoido para calucular cuanto sube dicha habilidad especial
        }
    }
    private static bool WaithPlayer = false;
    public static void WaithPlayers(UnityAction<string> player)
    {
        if (WaithPlayer)
        {
            StaticRules loRule = FailSafeInstance();
            if (loRule.NowPhase==7)
            {
                loRule.NowPhase = 0;
            }
            loRule.NowPhase += 1;
            // Aca va metodo de siguiente phase yo lo calcule aqui pero se puede hacer por separado

            WaithPlayer = false;
        }
        else
        {
            WaithPlayer = true;
        }
    }
    

    
    
}

