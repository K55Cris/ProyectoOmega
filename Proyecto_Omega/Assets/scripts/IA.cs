using DigiCartas;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IA : MonoBehaviour
{
    public List<DecksIA> Decks;
    public int Nivel;
    public DecksIA ChoseDeck;
    public static IA instance;
    public Dificultad IALevel;
    public Player IAPlayer;
    private Transform ManoEspacio;
    public GameObject PanelClosePlayer;
    public DecksIA Defaul;
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

    public void TurnoIA(bool espera = false)
    {
        PanelClosePlayer.gameObject.SetActive(true);
        PartidaManager.instance.MenuPhases.ChangePhase(false);
        StaticRules.instance.WhosPlayer = IAPlayer;
        if (espera)
            StartCoroutine(Whaiting(NowPhase, 3f));
        else
            StartCoroutine(Whaiting(NowPhase, 1f));
    }


    public void NowPhase(string Result)
    {

        switch (StaticRules.instance.NowPhase)
        {
            case DigiCartas.Phases.PreparationPhase:
                PreparationPhase();
                break;
            case DigiCartas.Phases.PreparationPhase2:
                PreparationPhase();
                break;
            case DigiCartas.Phases.EvolutionPhase:
                StartEvolutionPhase();
                break;
            case DigiCartas.Phases.EvolutionPhase2:
                StartEvolutionPhase();
                break;

            case DigiCartas.Phases.OptionBattlePhase:
                PartidaManager.instance.CambioDePhase(false);
                BattlelPhases();
                break;
            case DigiCartas.Phases.AppearanceRequirements:
                AppearanceRequirements();
                break;
            case DigiCartas.Phases.PointCalculationPhase:

                break;
            default:
                Debug.Log("Fase no recocida para este Slot");
                break;

        }
    }

    private void AppearanceRequirements()
    {
        PanelClosePlayer.SetActive(false);
        StaticRules.instance.ListEvos = new List<DigiEvoluciones>();
        StaticRules.SecondEvolutionPhase();
        FinishTurnoIA();
    }

    public bool CanSetCardOptionBattlet()
    {
        if (MesaManager.instance.Campo2.OptionSlot1.GetComponent<OptionSlot>().OpCarta != null ||
        MesaManager.instance.Campo2.OptionSlot2.GetComponent<OptionSlot>().OpCarta != null ||
        MesaManager.instance.Campo2.OptionSlot3.GetComponent<OptionSlot>().OpCarta != null)
            return true;
        else
            return false;
    }
    private void BattlelPhases()
    {
        CartaDigimon Digimon = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta;
        // Revisamos si ganamos la Batalla
        if (SimularBatalla(Digimon))
        {
            DigiCarta wins = new DigiCarta();
            wins.id = 58;
            // si seguimos ganando la Ronda revisamos si temaos esta carta para activarla
            if (MesaManager.instance.GetOptionSlotForCard(wins))
            {
                // revisamos si non es child
                if (Digimon.DatosDigimon.Nivel != "III")
                {
                    // Activamos la carta
                    StaticRules.ActivateOptionCard(MesaManager.instance.
                    GetOptionSlotForCard(wins).GetComponent<OptionSlot>().OpCarta);
                    TurnoJugador(true);
                }
            }
            else
            {
                // si no tenemos nada que activar termiamos
                TurnoJugador(true);
            }
        }
        else
        {
            // si perdemos revisamos si tenemos option en el campo
            if (MesaManager.instance.Campo2.OptionSlot1.GetComponent<OptionSlot>().OpCarta ||
                MesaManager.instance.Campo2.OptionSlot2.GetComponent<OptionSlot>().OpCarta ||
                MesaManager.instance.Campo2.OptionSlot3.GetComponent<OptionSlot>().OpCarta)
            {
                // si contamos con alguno revisamos si nos sirven para ganar 
                // Buscar algun chip que cambie el ataque para ganar 
                DigiCarta A = new DigiCarta();
                A.id = 49;
                DigiCarta C = new DigiCarta();
                C.id = 50;
                DigiCarta B = new DigiCarta();
                B.id = 51;
                DigiCarta Lost = new DigiCarta();
                Lost.id = 52;
                DigiCarta Metal = new DigiCarta();
                Metal.id = 53;
                DigiCarta Conter = new DigiCarta();
                Conter.id = 54;
                DigiCarta DeEvolution = new DigiCarta();
                DeEvolution.id = 57;

                if (MesaManager.instance.GetOptionSlotForCard(A))
                {
                    // revisamos si El cambio de Batalla hace que ganes 
                    if (SimularBatallaAtaque(Digimon, "A", 0))
                    {
                        // Activamos la carta de lo contrario revisamos la siguiente 
                        StaticRules.ActivateOptionCard(MesaManager.instance.
                            GetOptionSlotForCard(A).GetComponent<OptionSlot>().OpCarta);
                        TurnoJugador(false);
                        return;
                    }
                }
                if (MesaManager.instance.GetOptionSlotForCard(B))
                {
                    // revisamos si El cambio de Batalla hace que ganes 
                    if (SimularBatallaAtaque(Digimon, "B", 0))
                    {
                        // Activamos la carta de lo contrario revisamos la siguiente 
                        StaticRules.ActivateOptionCard(MesaManager.instance.
                           GetOptionSlotForCard(B).GetComponent<OptionSlot>().OpCarta);
                        TurnoJugador(false);
                        return;
                    }
                }
                if (MesaManager.instance.GetOptionSlotForCard(C))
                {
                    // revisamos si El cambio de Batalla hace que ganes 
                    if (SimularBatallaAtaque(Digimon, "C", 0))
                    {
                        // Activamos la carta de lo contrario revisamos la siguiente 
                        StaticRules.ActivateOptionCard(MesaManager.instance.
                        GetOptionSlotForCard(C).GetComponent<OptionSlot>().OpCarta);
                        TurnoJugador(false);
                        return;
                    }
                }

                // buscamos alguna carta de Conter para poder Ganar aun 
                if (MesaManager.instance.GetOptionSlotForCard(Metal))
                {
                    if (SimularBatallaAtaque(Digimon, "", 50))
                    {
                        StaticRules.ActivateOptionCard(MesaManager.instance.
                        GetOptionSlotForCard(Metal).GetComponent<OptionSlot>().OpCarta);
                        TurnoJugador(false);
                        return;
                    }
                }
                if (MesaManager.instance.GetOptionSlotForCard(Conter))
                {
                    DigimonBoxSlot midigi = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>();
                    DigimonBoxSlot digienemigo = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();

                    if (StaticRules.instance.WhatNivelMayor(digienemigo._DigiCarta.DatosDigimon.Nivel, midigi._DigiCarta.DatosDigimon.Nivel))
                    {
                        StaticRules.ActivateOptionCard(MesaManager.instance.
                        GetOptionSlotForCard(Conter).GetComponent<OptionSlot>().OpCarta);
                        TurnoJugador(false);
                        return;
                    }
                }
                if (MesaManager.instance.GetOptionSlotForCard(DeEvolution))
                {
                    // revisamos si cuenta el enemigo con un champeon
                    DigimonBoxSlot digienemigo = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();

                    if (digienemigo._DigiCarta.DatosDigimon.Nivel == "IV")
                    {
                        // Activamos la carta de lo contrario revisamos la siguiente 
                        StaticRules.ActivateOptionCard(MesaManager.instance.
                        GetOptionSlotForCard(DeEvolution).GetComponent<OptionSlot>().OpCarta);
                        TurnoJugador(false);
                        return;
                    }
                }


                if (MesaManager.instance.GetOptionSlotForCard(Lost))
                {
                    // SI PERDEMOS activamos carta
                    StaticRules.ActivateOptionCard(MesaManager.instance.
                      GetOptionSlotForCard(Lost).GetComponent<OptionSlot>().OpCarta);
                    TurnoJugador(true);
                    return;
                }
                TurnoJugador(true);
            }
            else
            {
                // Pasamos el turno dando pie a que el jugador juegue
                TurnoJugador(true);
            }
        }

    }

    public void TurnoJugador(bool action)
    {
        StaticRules.instance.WhaitPlayersReadyBattlet(action);
        StartCoroutine(Whaiting(TerminarBatalla, 0.5f));

    }

    public void EndBattlePhases()
    {

    }


    private void PreparationPhase()
    {
        // CHECAMOS EL CHILD 
        CheckOptiondiscard();
        //
        CheckChangeChild();

    }

    public void CheckOptiondiscard()
    {
        Campos Campo2 = MesaManager.instance.Campo2;
        OptionSlot OptionSlot = Campo2.OptionSlot1.GetComponent<OptionSlot>();


        if (CheckPriorityOptionCard(OptionSlot.OpCarta, OptionSlot.TurnOpcard))
        {
            OptionSlot.Descartar();
        }
        OptionSlot = Campo2.OptionSlot2.GetComponent<OptionSlot>();
        if (CheckPriorityOptionCard(OptionSlot.OpCarta, OptionSlot.TurnOpcard))
        {
            OptionSlot.Descartar();
        }
        OptionSlot = Campo2.OptionSlot3.GetComponent<OptionSlot>();
        if (CheckPriorityOptionCard(OptionSlot.OpCarta, OptionSlot.TurnOpcard))
        {
            OptionSlot.Descartar();
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
            Invoke("DiscarPhase", 2f);
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
        int Cant = PlayerManager.instance.Jugador.Nivel;
        if (Cant < 25)
            IALevel = Dificultad.Facil;
        else if (Cant < 50)
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
        {
            List<DecksIA> DecksWhitDificult = new List<DecksIA>();

            foreach (var item in PlayerManager.instance.DeckIA.Decks)
            {
                if (item.DificultadMazo == IALevel)
                    DecksWhitDificult.Add(item);
            }
            // CARGAMOS UN MAZO aleatorio dependiendo la dificultad
            int Rand = DataManager.GetRandom(0, DecksWhitDificult.Count);
            ChoseDeck = DecksWhitDificult[Rand];
        }
        else
        {
            ChoseDeck = Defaul;
        }
        IAPlayer.IDCartasMazo = ChoseDeck.mazo;
    }

    public void SelectChild(UnityAction<string> fase, List<CartaDigimon> cartas)
    {
        // CARGAMOS UNA carta Aleatoria
        int Rand = DataManager.GetRandom(0, cartas.Count);
        fase.Invoke(cartas[Rand].cardNumber.ToString());
    }

    public void CheckChangeChild()
    {
        // Revisamos si el digimonslot tiene un Child
        if (MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel == "III")
        {
            CartaDigimon Child = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta;
            // Revisa si se tiene un Child extra en la Mano
            CartaDigimon NewChild = null;
            List<CartaDigimon> Childs = new List<CartaDigimon>();
            foreach (var item in IAPlayer._Mano.Cartas)
            {
                bool Skip = true;
                if (item.DatosDigimon.Nivel == "III")
                {
                    Childs.Add(item);
                    // Revisamos si tenemos una carta en la mano para que el child evolucione
                    foreach (var item2 in IAPlayer._Mano.Cartas)
                    {
                        if (StaticRules.CheckEvolutionList(item2, Child))
                        {
                            // Revisamos si dicha evolucion puede ganar la partida 
                            if (SimularBatalla(item2))
                            {
                                // no se cambia por el child ya puesto puede ganar 
                                //>>>>>>>
                                Debug.Log("no se cambia por el child ya puesto puede ganar evolucionado");
                                Skip = false;
                                CheckSetEvolutions(null);
                                return;
                            }
                            else
                            {
                                Skip = true;
                            }
                        }
                    }
                    if (!Skip)
                    {
                        // Revisamos si tenemos una carta en la mano para que el new child evolucuione
                        foreach (var item2 in IAPlayer._Mano.Cartas)
                        {
                            if (StaticRules.CheckEvolutionList(item2, item))
                            {
                                // Revisamos si dicha evolucion puede ganar la partida 
                                if (SimularBatalla(item2))
                                {
                                    // cambiamos el child y salimos
                                    NewChild = item;
                                    //>>>>>>>
                                    break;
                                }
                            }
                        }
                    }
                }
                if (NewChild != null)
                {
                    // el nuevo child a sido elegido asi que salimos 
                    Debug.Log("el child pierde cambiamos");
                    ChangeChild(NewChild, CheckSetEvolutions);
                    return;
                }
            }
            if (NewChild != null)
            {
                // revisamos si existe algun roquin que pueda ganar la batalla por si solo 
                // primero el seteado
                if (SimularBatalla(Child))
                {
                    // el child gana por su cuenta asi que paramos aqui
                    Debug.Log("el child NO EVOuciona pero si gana");
                    ChangeChild(Child, CheckSetEvolutions);

                }
                else
                {
                    foreach (var item in Childs)
                    {
                        // revisamos cual es el primer roquin que pueda ganar la batalla
                        if (SimularBatalla(item))
                        {
                            // ste child Gana asi que cambiemos a este 
                            Debug.Log("el child pierde pero este child si gana");
                            ChangeChild(item, CheckSetEvolutions);
                            break;
                        }
                    }
                }
            }
            else
            {
                CheckSetEvolutions(null);
            }
        }
        else
        {
            CheckSetEvolutions(null);
        }
    }

    public void CheckSetEvolutions(CartaDigimon Child)
    {

        Debug.Log("La Ia esta revisando sus evoluciones a setear:");

        if (Child == null)
            Child = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta;

        // Revisamos si tenemos una carta en la mano para que el evolucione
        foreach (var item2 in IAPlayer._Mano.Cartas)
        {
            if (StaticRules.CheckEvolutionList(item2, Child))
            {
                // El digimon Evoluciona y puede ganar 
                //>>>>>>>
                if (CheckRequeriments(item2, Child))
                {
                    Debug.LogWarning("owooo");
                    JugarCarta(item2, Campo.EvolutionSlot);
                    AnotherCardEvolution(item2);
                    return;
                }
            }
        }
        // saltamos a colocar los chips
        SetOptionCards("Salto");
    }

    public void AnotherCardEvolution(CartaDigimon PreEvo)
    {
        foreach (var item in IAPlayer._Mano.Cartas)
        {
            // Revisamos si tenemos una carta en la mano para que el evolucione
            if (StaticRules.CheckEvolutionList(item, PreEvo))
            {
                // El digimon Evoluciona y puede colocarse
                //>>>>>>>
                if (CheckRequeriments(item, PreEvo))
                {
                    StartCoroutine(SetAnotherEvo(item));
                    return;
                }
            }
        }
        Invoke("SetRequeriments", 0.5F);
    }
    public IEnumerator SetAnotherEvo(CartaDigimon Evo)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1.5f);
        JugarCarta(Evo, Campo.EvolutionSlot);
        yield return new WaitForSeconds(1.5f);
        Invoke("SetRequeriments", 0.5f);
    }
    public List<CartaDigimon> Requesitos = new List<CartaDigimon>();

    public void SetRequeriments()
    {

        List<string> Requerimientos = MesaManager.instance.Campo2.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().ListaRequerimientos;

        bool Colocar = false;
        Requesitos = new List<CartaDigimon>();
        foreach (var item in Requerimientos)
        {
            if (item == "X" | item == "O")
            {
                Colocar = true;
            }
            else if (item != "+")
            {
                if (item == "40%")
                {
                    // setear carta de 40
                    Requesitos.Add(SerchCardInHand("40", true));
                }
                else if (item == "60%")
                {
                    // setear Carta de 60
                    Requesitos.Add(SerchCardInHand("60", true));
                }
                else
                {
                    // Revisemos que no sea el digimon ya puesto 
                    if (item != MesaManager.instance.Campo2.DigimonSlot.GetComponent
                        <DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nombre.ToUpper().Replace(" ", ""))
                    {

                        // revisamos que no sea un digimon ya puesto en el evolution slot
                        // setear carta de la mano 
                        foreach (var item2 in MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas)
                        {
                            if (!item2.DatosDigimon.Nombre.ToUpper().Replace(" ", "").Contains(item))
                            {
                                // Seteamos la Carta 
                                Requesitos.Add(SerchCardInHand(item));
                            }
                        }
                    }
                }
            }
        }

        // mandamos a poner las X y O
        if (Colocar)
        {
            MesaManager.instance.Campo2.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().Requerimientos();
            StartCoroutine(Whaiting(EnterOtherRequeriments, 1f));
        }
        else
        {
            StartCoroutine(SetCardsTime(0.3f, Requesitos, Campo.Requeriment, SetOptionCards));
        }
    }

    public IEnumerator SetCardsTime(float time, List<CartaDigimon> Dcards, Campo place, UnityAction<string> LoAction = null)
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();

        foreach (var item in Dcards)
        {
            JugarCarta(item, place);
            yield return new WaitForSeconds(time);
        }
        if (LoAction != null)
            LoAction.Invoke("");
    }

    public void EnterOtherRequeriments(string result)
    {
        StartCoroutine(SetCardsTime(0.3f, Requesitos, Campo.Requeriment, SetOptionCards));
    }

    public void SetOptionCards(string result)
    {

        // Aqui desidimos  que Chips Colocar
        // obtenemos los chips posibles de la mano
        List<CartaDigimon> Options = new List<CartaDigimon>();
        List<CartaDigimon> CartasJugadas = new List<CartaDigimon>();
        foreach (var item in IAPlayer._Mano.Cartas)
        {
            if (item.DatosDigimon.IsSupport && !item.DatosDigimon.IsActivateHand)
                Options.Add(item);
        }
        if (Options.Count > 0)
        {
            foreach (var item in Options)
            {
                switch (IALevel)
                {
                    case Dificultad.Facil:
                        // Se colocan todos los chips que se tengan en la mano y puedan ponerse
                        CartasJugadas.Add(item);
                        break;
                    case Dificultad.Normal:
                        if (DataManager.GetRandom(1, 3) == 1)
                        {
                            // revisamos si Esat carta Tiene prioridad
                            if (CheckPriorityOptionCard(item, SimularBatalla(MesaManager.instance.Campo2.
                                DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta)))
                            {
                                CartasJugadas.Add(item);
                            }
                        }
                        else
                        {
                            CartasJugadas.Add(item);
                        }
                        break;
                    case Dificultad.Dificil:
                        if (1f / DataManager.GetRandom(1, 5) >= .26f)
                        {
                            // revisamos si Esta carta Tiene prioridad
                            if (CheckPriorityOptionCard(item, SimularBatalla(MesaManager.instance.Campo2.
                                DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta)))
                            {
                                CartasJugadas.Add(item);
                            }
                        }
                        else
                        {
                            CartasJugadas.Add(item);
                        }
                        break;
                    case Dificultad.Experto:
                        // revisamos si Esta carta Tiene prioridad
                        if (CheckPriorityOptionCard(item, SimularBatalla(MesaManager.instance.Campo2.
                            DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta)))
                        {
                            CartasJugadas.Add(item);
                        }
                        break;
                    default:
                        break;
                }
            }
            // seteamos las cartas
            StartCoroutine(SetCardsTime(0.3f, CartasJugadas, Campo.OptionSlot, FinishPreparation));
        }
        else
        {
            FinishPreparation("");
        }



    }
    public void FinishPreparation(string result)
    {
        Debug.Log("IA Finish preparation");
        FinishTurnoIA();
    }

    public bool CheckPriorityOptionCard(CartaDigimon OpCard, bool BatallaSimulada)
    {
        if (OpCard)
        {
            if (OpCard.DatosDigimon.id == 53 | OpCard.DatosDigimon.id == 56 | OpCard.DatosDigimon.id == 54 | OpCard.DatosDigimon.id == 57)
                return true;
            else if (!BatallaSimulada && (OpCard.DatosDigimon.id == 49 | OpCard.DatosDigimon.id == 50 | OpCard.DatosDigimon.id == 51))
                return true;
            else if (BatallaSimulada && OpCard.DatosDigimon.id == 58)
                return true;
            else if (!BatallaSimulada && OpCard.DatosDigimon.id == 52)
                return true;

            return false;
        }
        else
            return false;
    }

    public bool CheckPriorityOptionCard(CartaDigimon OpCard, int Turno)
    {
        if (OpCard)
        {
            if (OpCard.DatosDigimon.id == 53 | OpCard.DatosDigimon.id == 58 | OpCard.DatosDigimon.id == 54 | OpCard.DatosDigimon.id == 57)
            {
                if (Turno >= 5)
                {
                    return true;
                }

            }
            else if (OpCard.DatosDigimon.id == 49 | OpCard.DatosDigimon.id == 50 | OpCard.DatosDigimon.id == 51 | OpCard.DatosDigimon.id == 52)
            {
                if (Turno >= 3)
                {
                    return true;
                }
            }
            else if (OpCard.DatosDigimon.id == 59 | OpCard.DatosDigimon.id == 60)
            {
                if (Turno >= 4)
                {
                    return true;
                }
            }
            return false;
        }
        else
            return false;

    }


    public CartaDigimon SerchCardInHand(string name, bool isChip = false)
    {
        if (!isChip)
        {
            return IAPlayer._Mano.Cartas.Find(x => x.DatosDigimon.Nombre.ToUpper().Replace(" ", "") == name);
        }
        else
        {
            return IAPlayer._Mano.Cartas.Find(x => x.DatosDigimon.id == Convert.ToInt32(name));
        }
    }

    public bool CheckRequeriments(CartaDigimon Ecard, CartaDigimon Base)
    {
        List<string> requerimientos = StaticRules.GetListRequerimentsDigimon(Ecard.DatosDigimon, Base.DatosDigimon);

        if (requerimientos.Count > 0)
        {
            int CONTADOR = 0;
            foreach (var item in requerimientos)
            {

                if (item == "O" | item == ("X") | item == ("+"))
                {
                    // ignoramos
                    CONTADOR++;
                }
                else
                {
                    if (item.Equals(Base.DatosDigimon.Nombre.ToUpper().Replace(" ", "")))
                    {
                        // IGNORAMOS
                        CONTADOR++;
                    }
                    else if (item == "60%")
                    {
                        // buscamos en la mano
                        if (IAPlayer._Mano.Cartas.Find(x => x.DatosDigimon.id == 60))
                        {
                            CONTADOR++;
                        }
                        DigiCarta digicard = new DigiCarta();
                        digicard.id = 60;
                        Transform slot = MesaManager.instance.GetOptionSlotForCard(digicard);
                        if (slot)
                        {
                            CONTADOR++;
                        }
                    }
                    else if (item == "40%")
                    {
                        // buscamos en la mano
                        if (IAPlayer._Mano.Cartas.Find(x => x.DatosDigimon.id == 40))
                        {
                            CONTADOR++;
                        }
                        // BUSCAMOS en el campo
                        DigiCarta digicard = new DigiCarta();
                        digicard.id = 59;
                        Transform slot = MesaManager.instance.GetOptionSlotForCard(digicard);
                        if (slot)
                        {
                            CONTADOR++;
                        }
                    }
                    else
                    {
                        // buscamos en la mano el digimon faltante
                        if (IAPlayer._Mano.Cartas.Find(x => x.DatosDigimon.Nombre.
                        ToUpper().Replace(" ", "") == item))
                        {
                            CONTADOR++;
                        }
                    }
                }
            }
            if (CONTADOR == requerimientos.Count)
            {
                // si jugamos esta carta
                return true;
            }
            Debug.LogWarning(CONTADOR);
        }

        return false;
    }

    public void ChangeChild(CartaDigimon Child, UnityAction<CartaDigimon> Paso2Preparation)
    {
        // revisamos si el child es el mismo 
        CartaDigimon DRoquin = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta;
        if (DRoquin == Child)
        {
            // no se cambia asi que procegimos
            Paso2Preparation(DRoquin);
        }
        else
        {
            JugarCarta(Child, Campo.DigimonBox);
            StartCoroutine(WhaitFase(Paso2Preparation, Child, 1.5f));
        }
    }

    public void StartEvolutionPhase()
    {
        // voltear Digimon 
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta.Volteo();

        // Realizar Evolucion correspondiente 
        if (MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas.Count != 0)
        {
            foreach (var Evolucion in MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas)
            {
                DigiEvoluciones ItemEvo = new DigiEvoluciones();
                ItemEvo.DigiCarta = Evolucion.transform;
                StaticRules.instance.Evol(ItemEvo, MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas.Count);
            }
        }
        else
        {
            // revisar si se cuenta con la carta 56 
            CheckOptionEvolution(WaithOptionEvolution);
        }
    }
    public void WaithOptionEvolution(bool Result)
    {
        if (Result)
        {
            Debug.LogWarning("owooo en espera");
            StartCoroutine(Whaiting(TerminarTurno, 2f));
        }
        else
        {
            FinishTurnoIA();
        }
    }


    public bool CheckUso(CartaDigimon Dcard)
    {
        // Es evolucion de la carta que esta puesta
        if (StaticRules.CheckEvolutionList(Dcard, MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta))
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

    public void CheckOptionEvolution(UnityAction<bool> LoAction)
    {
        // Revisamos si se cuenta con una carta en la Mano 
        CartaDigimon Chip56 = IAPlayer._Mano.Cartas.Find(K => K.DatosDigimon.id == 56);
        EvolutionBox ESlot = MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>();
        if (Chip56)
        {
            // TENEMOS LA CARTA EN LA MANO
            if (ESlot.Cartas.Count == 0)
            {
                // revisamos que no haya evoluciones
                // revisamos si tenemos evolucion de la evolucion seteada
                string Nivel = StaticRules.ConvertToNivel(StaticRules.ConvertNivel(MesaManager.instance.Campo2.
                    DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel) + 1);

                if (Nivel!= null)
                {
                    foreach (var item in IAPlayer._Mano.Cartas)
                    {
                        if (Nivel == item.DatosDigimon.Nivel)
                        {
                            // Activamos el efecto
                            StaticRules.ActivateOptionCard(Chip56);
                            LoAction(true);
                            return;
                        }
                    }
                }
                LoAction(false);
            }
            else
            {
                // por ahóra la Ia no usara este chip ya que ya se cuenta con una previa evolucion
                LoAction(false);
            }
        }
        else
        {
            // EL cHIP 56 no se encuentra
            LoAction(false);
        }
    }



    public void JugarCarta(CartaDigimon Dcard, Campo Destino)
    {
        Transform Lugar = null;
        switch (Destino)
        {
            case Campo.DigimonBox:
                MesaManager.instance.GetSlot(MesaManager.Slots.
                    DigimonSlot, IAPlayer).GetComponent<DigimonBoxSlot>().SetDigimon(Dcard.transform);
                break;
            case Campo.OptionSlot:
                GetOptionSlot(ref Lugar);
                if (Lugar)
                {
                    Debug.Log(Lugar.gameObject.name);
                    Lugar.GetComponent<OptionSlot>().SetCard(Dcard.transform);
                }
                break;
            case Campo.EvolutionSlot:
                MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().
                    SetDigimon(Dcard.transform);
                break;
            case Campo.Requeriment:
                MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionRequerimentBox>().
                    SetRequerimientos(Dcard);
                break;
            case Campo.Netocean:
                MesaManager.instance.Campo2.EvolutionBox.GetComponent<NetOcean>().
                 addNetocean(Dcard);
                break;
            case Campo.Support:
                // Lugar = MesaManager.instance.GetSlot(MesaManager.Slots.SupportBox, IAPlayer);
                break;
            default:
                break;
        }

    }

    public void GetOptionSlot(ref Transform pos)
    {
        if (MesaManager.instance.Campo2.OptionSlot2.GetComponent<OptionSlot>().Vacio)
        {
            pos = MesaManager.instance.Campo2.OptionSlot2;
        }
        else if (MesaManager.instance.Campo2.OptionSlot1.GetComponent<OptionSlot>().Vacio)
        {
            pos = MesaManager.instance.Campo2.OptionSlot1;
        }
        else if (MesaManager.instance.Campo2.OptionSlot3.GetComponent<OptionSlot>().Vacio)
        {
            pos = MesaManager.instance.Campo2.OptionSlot3;
        }
        else
        {
            pos = null;
        }
    }

    public bool SimularBatalla(CartaDigimon TCard)
    {
        CartaDigimon D1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta;
        // Obtenemos ataques
        string Digimon1 = D1.DatosDigimon.TipoBatalla;
        string Digimon2 = TCard.DatosDigimon.TipoBatalla;


        // obtenemos Fuerza de los ataques
        int OffD1 = StaticRules.instance.WhatAtackUse(Digimon2, D1);
        int OffD2 = StaticRules.instance.WhatAtackUse(Digimon1, TCard);

        if (Digimon1 == "C" && Digimon2 == "A")
            OffD2 = 0;
        if (Digimon2 == "C" && Digimon1 == "A")
            OffD1 = 0;

        Debug.Log(D1.DatosDigimon.Nombre + ":" + OffD1 + "|" + TCard.DatosDigimon.Nombre + ":" + OffD2);
        // Verificamos si ganamos
        if (OffD2 >= OffD1)
            return true;
        else
            return false;
    }

    public bool SimularBatallaAtaque(CartaDigimon TCard, string ataque, int BUFFO)
    {
        CartaDigimon D1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta;
        // Obtenemos ataques
        string Digimon1 = D1.DatosDigimon.TipoBatalla;
        string Digimon2 = TCard.DatosDigimon.TipoBatalla;
        if (TCard.DatosDigimon.TipoBatalla == ataque)
            return false;

        int OffD1 = StaticRules.instance.WhatAtackUse(Digimon2, D1);

        if (ataque != "")
            Digimon2 = ataque;

        // obtenemos Fuerza de los ataques

        int OffD2 = StaticRules.instance.WhatAtackUse(Digimon2, TCard);

        OffD2 += BUFFO;
        Debug.Log(D1.DatosDigimon.Nombre + ":" + OffD1 + "|" + TCard.DatosDigimon.Nombre + ":" + OffD2);
        // Verificamos si ganamos
        if (OffD2 >= OffD1)
            return true;
        else
            return false;
    }
    public bool CompararAtaques(CartaDigimon Digimon1, CartaDigimon Digimon2)
    {
        string AtaqueOponente = MesaManager.instance.Campo1.DigimonSlot.GetComponent
                                <DigimonBoxSlot>()._DigiCarta.DatosDigimon.TipoBatalla;
        int OffD1 = StaticRules.instance.WhatAtackUse(AtaqueOponente, Digimon1);
        int OffD2 = StaticRules.instance.WhatAtackUse(AtaqueOponente, Digimon2);
        // Verificamos si ganamos
        if (OffD2 > OffD1)
            return true;
        else
            return false;
    }

    public IEnumerator WhaitFase(UnityAction<CartaDigimon> Loaction, CartaDigimon Dcard, float segundos)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(segundos);
        Loaction.Invoke(Dcard);
    }

    public IEnumerator Whaiting(UnityAction<string> Loaction, float segundos)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(segundos);
        Loaction.Invoke("");
    }

    public void TerminarTurno(string result)
    {
        PartidaManager.instance.CambioDePhase(true);
        PanelClosePlayer.SetActive(false);
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
    }
    public void TerminarBatalla(string result)
    {
        PartidaManager.instance.CambioDePhase(true);
        PanelClosePlayer.SetActive(false);
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;

        PartidaManager.instance.MenuPhases.ChangePhase(true);

        // si perdemos revisamos si JUGADOR TIENE options en el campo
        if (MesaManager.instance.Campo1.OptionSlot1.GetComponent<OptionSlot>().OpCarta ||
            MesaManager.instance.Campo1.OptionSlot2.GetComponent<OptionSlot>().OpCarta ||
            MesaManager.instance.Campo1.OptionSlot3.GetComponent<OptionSlot>().OpCarta)
        {
            // si las hay damos chance a activar
        }
        else
        {
            StaticRules.instance.WhaitPlayersReadyBattlet(true);
        }
    }


    public void FinishTurnoIA()
    {
        PanelClosePlayer.SetActive(false);
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
        StaticRules.SiguienteFase();
        // QUITAR BLOQUEO A PLAYER 1
    }
}
