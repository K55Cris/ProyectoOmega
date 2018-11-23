using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
using System;
using UnityEngine.Events;

public class IA : MonoBehaviour {
    public List<DecksIA> Decks;
    public int Nivel;
    public DecksIA ChoseDeck;
    public static IA instance;

    public Dificultad IALevel;
    private Player IAPlayer;
    private Transform ManoEspacio;

    public void Start()
    {
        IAPlayer = PartidaManager.instance.Player2;
        ManoEspacio = PartidaManager.instance.ManoPlayer2;
        CargarDificultad();
        ChoiseDeck();
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }

        else if (instance != this)
            Destroy(gameObject);
    }

    public void TurnoIA()
    {
        StaticRules.instance.WhosPlayer = IAPlayer;
        NowPhase();
    }

    public void NowPhase()
    {
        switch (StaticRules.NowPhase)
        {
            case DigiCartas.Phases.PreparationPhase:
                PreparationPhase();
                break;
            case DigiCartas.Phases.EvolutionPhase:
                EvolutionPhase();
                break;
            case DigiCartas.Phases.EvolutionRequirements:
                EvolutionRequirements();
                break;
            case DigiCartas.Phases.FusionRequirements:
                FusionRequirements();
                break;
            case DigiCartas.Phases.EndPhase:
                EndPhase();
                break;
            default:
                Debug.Log("Fase no recocida para este Slot");
                break;

        }
    }

    private void EndPhase()
    {
    }

    private void FusionRequirements()
    {
    }

    private void EvolutionRequirements()
    {
    }

    private void EvolutionPhase()
    {
    }

    private void PreparationPhase()
    {
        // DESCARTAR cartas

        //revisamos si en nuestra mano hay cartas que ocupamos

        foreach (var item in PartidaManager.instance.Player2._Mano.Cartas)
        {
            
        } 

    }
    public void DiscarPhase()
    {
        if (IAPlayer._Mano.Cartas.Count != 0)
        {
            foreach (var item in IAPlayer._Mano.Cartas)
            {
                // revisamos carta por carta si es util 
                if (!CheckUso(item))
                {
                    // la carta es inutil marcar para descarte
                    StaticRules.instance.CartasDescartadasIA.Add(item.gameObject);
                }
            }
            Invoke("SimulateThinkIADiscard", 0.5f);
        }
        else
        {
            // esperar a tener cartas 
            Invoke("DiscarPhase",2f);
        }
    }
    public void SimulateThinkIADiscard()
    {
        StaticRules.instance.DiscardCards(IAPlayer);
    }

    public void EsperarRespuestaIA(UnityAction<string> LoAction)
    {
    }

    public void CargarDificultad()
    {
       int Cant = PlayerManager.instance.Nivel;
        if (Cant<25)
            IALevel = Dificultad.Facil;
        else if(Cant < 50)
            IALevel = Dificultad.Normal;
        else if (Cant < 75)
            IALevel = Dificultad.Dificil;
        else if (Cant > 75)
            IALevel = Dificultad.Experto;
        else
            IALevel = Dificultad.Normal;
    }
    public void ChoiseDeck()
    {
        List<DecksIA> DecksWhitDificult=new List<DecksIA>();
        foreach (var item in Decks)
        {
            if (item.DificultadMazo == IALevel)
                DecksWhitDificult.Add(item);
        }
        // CARGAMOS UN MAZO aleatorio dependiendo la dificultad
        int Rand = DataManager.GetRandom(0, DecksWhitDificult.Count);
        ChoseDeck = DecksWhitDificult[Rand];
        IAPlayer.IDCartasMazo = ChoseDeck.mazo;
    }

    public void SelectChild(UnityAction<string> fase, List<CartaDigimon> cartas)
    {
        // CARGAMOS UNA carta Aleatoria
        int Rand = DataManager.GetRandom(0,cartas.Count);
        fase.Invoke(cartas[Rand].cardNumber.ToString());
    }

    public bool CheckUso(CartaDigimon Dcard)
    {
        // Es evolucion de la carta que esta puesta
        if (StaticRules.CheckEvolutionList(Dcard))
            return true;

        // es Requerimiento del Digimon que esta puesto para evolucionar
        List<string> requerimientos = StaticRules.GetListRequerimentsDigimon(Dcard.DatosDigimon, MesaManager.instance.GetDigimonSlot().DatosDigimon);

        if (requerimientos.Count > 0)
        {
            string FiltroNombre = Dcard.DatosDigimon.Nombre.ToUpper().Replace(" ", "");
            foreach (var item in requerimientos)
            {
                if (item.Contains(FiltroNombre))
                    return true;
                if (item == "40%" && FiltroNombre.Contains("40%"))
                    return true;
                if (item == "60%" && FiltroNombre.Contains("60%"))
                    return true;
            }
        }
        float Number = DataManager.GetRandom(0, 11);
        switch (IALevel)
        {
            case Dificultad.Facil:
                Number *= 0.30f;
                break;
            case Dificultad.Normal:
                Number *= 0.38f;
                break;
            case Dificultad.Dificil:
                Number *= 0.45f;
                break;
            case Dificultad.Experto:
                Number *= 0.6f;
                break;
            default:
                break;
        }
        if (Number > 2)
        {
            // la carta no se descarta
            return true;
        }
        return false;
    }

    public void FinishTurnoIA()
    {
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
        // QUITAR BLOQUEO A PLAYER 1
    }

}
