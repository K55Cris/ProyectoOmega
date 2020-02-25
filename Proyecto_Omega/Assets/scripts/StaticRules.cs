using DigiCartas;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class StaticRules : MonoBehaviour
{
    public static StaticRules instance = null;
    // Use this for initialization
    public int Turno = 0;
    public Player PlayerFirstAtack;
    public Player WhosPlayer;
    public Player LostPlayerRound;
    public int RondPuntosPerdidos = 0;
    public int PointGaugePlayer1 = 20;
    public static bool ProcesoDebuff = false;
    public int PointGaugePlayer2 = 20;
    public Phases NowPhase;
    public PreparationPhase NowPreparationPhase;
    public static IAPhase NowIAPhase;
    public List<Efecto> EfectosRonda = new List<Efecto>();
    public List<GameObject> CartasDescartadas = new List<GameObject>();
    public List<GameObject> CartasDescartadasIA = new List<GameObject>();
    public UnityAction<string> FinishEvolution;

    public enum PreparationPhase
    {
        DiscardPhase = 0, ChangeDigimon = 1, SetEvolition = 2, ActivarOption = 3, SetOptionCard = 4
    };
    public enum IAPhase
    {
        DiscardPhase = 0, PreparationPhase = 1, EvolitionPhase = 2, BattlePhase = 3
    };

    public bLOCKCARDS FueraJuego = new bLOCKCARDS();

    public List<TurnEfect> EfectosDeTurno = new List<TurnEfect>();

    public bool WhosPlayPlayer()
    {
        if (WhosPlayer == PartidaManager.instance.Player1)
            return true;
        else
            return false;

    }

    public void Start()
    {
        NowPhase = Phases.GameSetup;
        NowIAPhase = 0;
        //Referenciar los puntos de vida con cada jugador o
        //Codificar clase jugador con sus atributos publicos

        PointGaugePlayer1 = 100;
        PointGaugePlayer2 = 100;

        CartasBloqueadas BlockCards1 = new CartasBloqueadas();
        BlockCards1.jugador = PartidaManager.instance.Player1;
        CartasBloqueadas BlockCards2 = new CartasBloqueadas();
        BlockCards2.jugador = PartidaManager.instance.Player2;

        FueraJuego.UNO = BlockCards1;
        FueraJuego.DOS = BlockCards2;
    }


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public static void SelectDigimonChild()
    {
        FailSafeInstance();


        //Cargamos Mazos de Ambos Jugadores 
        PartidaManager.instance.CargarMazos(PartidaManager.instance.Player1.IDCartasMazo, MesaManager.instance.Campo1.NetOcean, PartidaManager.instance.Player1);
        PartidaManager.instance.CargarMazos(PartidaManager.instance.Player2.IDCartasMazo, MesaManager.instance.Campo2.NetOcean, PartidaManager.instance.Player2);

        ///Acomodamos el Deck por Bug que mueve las cartas >> Revisar que pasa en la fase de seledDigimonChild
        foreach (Transform item in MesaManager.instance.Campo1.NetOcean)
        {
            item.GetComponent<CartaDigimon>().AjustarSlot();

        }

        foreach (Transform item in MesaManager.instance.Campo2.NetOcean)
        {
            item.GetComponent<CartaDigimon>().AjustarSlot();
        }

        // player 1 selecciona digimon child
        SelectedDigimons.instance.Activar(SetDigimonChildPlayer1, GetDigimonChildInDeck(PartidaManager.instance.Player1.Deck), "Choose a Child");

    }

    public static void SetDigimonChildPlayer1(string IDCarta)
    {

        foreach (Transform item in MesaManager.instance.Campo1.NetOcean)
        {
            CartaDigimon ID = item.GetComponent<CartaDigimon>();
            if (IDCarta == ID.cardNumber.ToString())
            {
                MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().SetDigimon(item);
                MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().RobarEspesifico(ID.DatosDigimon.Nombre);
                ID.Mostrar();
                // player 2 selecciona digimon child
                IA.instance.SelectChild(SetDigimonChildPlayer2, GetDigimonChildInDeck(PartidaManager.instance.Player2.Deck));
                break;
            }
        }
    }
    public static void SetDigimonChildPlayer2(string IDCarta)
    {
        foreach (Transform item in MesaManager.instance.Campo2.NetOcean)
        {
            CartaDigimon ID = item.GetComponent<CartaDigimon>();
            if (IDCarta == ID.cardNumber.ToString())
            {

                MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().SetDigimon(item);
                MesaManager.instance.Campo2.NetOcean.GetComponent<NetOcean>().RobarEspesifico(ID.DatosDigimon.Nombre);
                ID.Mostrar();
                SelectDigimonChildPart2();
                break;
            }
        }
    }

    public static void SelectDigimonChildPart2()
    {
        Transform Deck1 = MesaManager.instance.Campo1.NetOcean;
        Transform Deck2 = MesaManager.instance.Campo2.NetOcean;
        // barajear mazo player 1
        PartidaManager.Barajear(Deck1);
        SoundManager.instance.PlaySfx(Sound.Barajear);
        // barajear mazo player 2
        PartidaManager.Barajear(Deck2);

        //Colocar primera carta del mazo boca abajo en la Point Gauge PLAYER1
        CartaDigimon FirstCarta = MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Robar();
        MesaManager.instance.Campo1.PointGauge.GetComponent<PointGaugeBox>().Dcard = FirstCarta.GetComponent<CartaDigimon>();
        MesaManager.instance.Campo1.PointGauge.GetComponent<PointGaugeBox>().iniciar();
        PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.PointGauge, FirstCarta.transform, Ajustar);

        // Sacamos del juego la carta del PointGauge
        PartidaManager.instance.Player1.Deck.cartas.Remove(FirstCarta.GetComponent<CartaDigimon>());

        //Colocar primera carta del mazo boca abajo en la Point Gauge PLAYER2
        CartaDigimon FirstCarta2 = MesaManager.instance.Campo2.NetOcean.GetComponent<NetOcean>().Robar();
        MesaManager.instance.Campo2.PointGauge.GetComponent<PointGaugeBox>().Dcard = FirstCarta2.GetComponent<CartaDigimon>();
        PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo2.PointGauge, FirstCarta2.transform, Ajustar);

        MesaManager.instance.Campo2.PointGauge.GetComponent<PointGaugeBox>().iniciar();
        // Sacamos del juego la carta del PointGauge PLAYER2
        PartidaManager.instance.Player2.Deck.cartas.Remove(FirstCarta2.GetComponent<CartaDigimon>());


        ///Acomodamos el Deck por Bug que mueve las cartas >> Revisar que pasa en la fase de seledDigimonChild
        foreach (Transform item in MesaManager.instance.Campo1.NetOcean)
        {
            item.GetComponent<CartaDigimon>().AjustarSlot();

        }

        foreach (Transform item in MesaManager.instance.Campo2.NetOcean)
        {
            item.GetComponent<CartaDigimon>().AjustarSlot();
        }


        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
        {
            // Elegir Primer Jugador
            WhoIsPlayer1.instance.Activar(PartidaManager.instance.Player1, PartidaManager.instance.Player2, StaticRules.instance.WhoFirstPlayer);
        }
        else
        {
            // Espacio para nuevos dialogos
            Tutorial.instance.Iniciar();
        }
    }



    public static List<CartaDigimon> GetDigimonChildInDeck(Mazo netocean)
    {
        List<CartaDigimon> DigimonChild = new List<CartaDigimon>();
        foreach (var item in netocean.cartas)
        {

            if (item.DatosDigimon.Nivel == "III")
            {
                DigimonChild.Add(item);
            }
        }
        return DigimonChild;
    }

    public static void ChangeFirstAtackPlayer()
    {
        if (StaticRules.instance.PlayerFirstAtack == PartidaManager.instance.Player1)
            StaticRules.instance.PlayerFirstAtack = PartidaManager.instance.Player2;
        else
            StaticRules.instance.PlayerFirstAtack = PartidaManager.instance.Player1;
    }
    /// <summary>
    /// Fija el jugador activo al inicio de la partida.
    /// Metodo redundante, comprobar SeleccionPrimerJugador()
    /// </summary>
    public void WhoFirstPlayer(Player jugador)
    {
        StaticRules loRule = FailSafeInstance();
        Debug.Log(jugador.name);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
        {
            loRule.PlayerFirstAtack = PartidaManager.instance.Player1; ;
            StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
        }
        else
        {
            loRule.PlayerFirstAtack = jugador;
            StaticRules.instance.WhosPlayer = jugador;
        }
        //cargar Manos player1
        PartidaManager.instance.cargarManos(PartidaManager.instance.ManoPlayer1, PartidaManager.instance.Player1, MesaManager.instance.Campo1.NetOcean);
        //cargar Manos player 2
        PartidaManager.instance.cargarManos(PartidaManager.instance.ManoPlayer2, PartidaManager.instance.Player2, MesaManager.instance.Campo2.NetOcean);

        CartasBloqueadas CardsBlock1 = new CartasBloqueadas();
        CardsBlock1.jugador = jugador;
        CardsBlock1.Cartasbloqueadas = new List<CartaDigimon>();

        CartasBloqueadas CardsBlock2 = new CartasBloqueadas();
        if (PartidaManager.instance.Player1 == jugador)
            CardsBlock2.jugador = jugador;
        else
            CardsBlock2.jugador = PartidaManager.instance.Player2;

        CardsBlock2.Cartasbloqueadas = new List<CartaDigimon>();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
        {
            SiguienteFase();
        }
        else
        {
            Tutorial.instance.Iniciar();
        }


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

    public void ActiveHAbiliti(CartaDigimon Digimon)
    {
        StaticRules loRule = FailSafeInstance();
        if (Digimon.Habilidades.Count > 0)
        {
            // tiene alguna habilidad
            Debug.LogWarning("owo");

            if (Digimon.Habilidades.Contains(Habilidades.Heal))
            {
                Debug.LogWarning("owo2");
                if (Digimon._jugador == PartidaManager.instance.Player1)
                {
                    loRule.PointGaugePlayer1 += 30; // aqui va otro metoido para calucular cuanto sube dicha habilidad especial

                    if (loRule.PointGaugePlayer1 > 100)
                        loRule.PointGaugePlayer1 = 100;

                        MesaManager.instance.Campo1.PointGauge.GetComponent<PointGaugeBox>().SetCard(loRule.PointGaugePlayer1);
                }
                else
                {
                    MesaManager.instance.Campo2.PointGauge.GetComponent<PointGaugeBox>().SetCard(loRule.PointGaugePlayer2);
                    loRule.PointGaugePlayer2 += 30; // aqui va otro metoido para calucular cuanto sube dicha habilidad especial

                    if (loRule.PointGaugePlayer2 > 100)
                        loRule.PointGaugePlayer2 = 100;

                    MesaManager.instance.Campo2.PointGauge.GetComponent<PointGaugeBox>().SetCard(loRule.PointGaugePlayer2);
                }
                PartidaManager.instance.ViewHeal();
                SoundManager.instance.PlaySfx(Sound.Heal);
            }
           
        }
    }

    private bool WaithPlayer = false;

    public void WaithPlayers(UnityAction<string> Siguiente)
    {
        if (WaithPlayer)
        {
            Debug.Log("segundo asi que cambio");

            Siguiente(""); // Aca va metodo de siguiente phase yo lo calcule aqui pero se puede hacer por separado

            WaithPlayer = false;
        }
        else
        {
            Debug.Log("Primero en entrar");
            WaithPlayer = true;
        }
    }

    public static void CheckSetDigiCardSlot(Transform Slot, Transform _Digicarta = null, bool Condicional = false)
    {
        if (Slot != null)
        {
            StaticRules loRule = FailSafeInstance();
            if (loRule.WhosPlayer == PartidaManager.instance.Player1)
            {

                DigiCarta _Carta = new DigiCarta();

                if (_Digicarta)
                    _Carta = _Digicarta.GetComponent<CartaDigimon>().DatosDigimon;

                switch (Slot.name)
                {
                    case "DigimonSlot":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.instance.NowPhase == Phases.PreparationPhase || StaticRules.instance.NowPhase == Phases.PreparationPhase2)
                        {
                            if (_Carta.Nivel == "III")
                            {
                                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().SetDigimon(_Digicarta.transform);
                            }
                        }
                        break;
                    case "Option Slot 1":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.instance.NowPhase == Phases.PreparationPhase || StaticRules.instance.NowPhase == Phases.PreparationPhase2)
                        {
                            if (isDigimonOrChip(_Carta))
                            {
                                MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot1).GetComponent<OptionSlot>().SetCard(_Digicarta.transform);
                            }
                        }
                        break;
                    case "Option Slot 2":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.instance.NowPhase == Phases.PreparationPhase || StaticRules.instance.NowPhase == Phases.PreparationPhase2)
                        {
                            if (isDigimonOrChip(_Carta))
                                MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot2).GetComponent<OptionSlot>().SetCard(_Digicarta.transform);
                        }
                        break;
                    case "Option Slot 3":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.instance.NowPhase == Phases.PreparationPhase || StaticRules.instance.NowPhase == Phases.PreparationPhase2)
                        {
                            if (isDigimonOrChip(_Carta))
                                MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot3).GetComponent<OptionSlot>().SetCard(_Digicarta.transform);
                        }
                        break;
                    case "EvolutionBox":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.instance.NowPhase == Phases.PreparationPhase || StaticRules.instance.NowPhase == Phases.PreparationPhase2)
                        {
                            if (!isDigimonOrChip(_Carta))
                            {
                                bool pase = true
; foreach (var item in MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas)
                                {
                                    if (item.DatosDigimon.id == _Carta.id)
                                    {
                                        pase = false;
                                        break;
                                    }
                                }
                                if (pase)
                                {
                                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().SetDigimon(_Digicarta);
                                }

                            }
                        }
                        break;

                    case "SupportBox":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.instance.NowPhase == Phases.PreparationPhase)
                        {
                            if (!isDigimonOrChip(_Carta))
                                MesaManager.instance.GetSlot(MesaManager.Slots.SupportBox).GetComponent<SupportBox>().SetDigimon(_Digicarta.transform);
                        }
                        break;
                    case "EvolutionRequerimentBox":

                        if (StaticRules.instance.NowPhase == Phases.PreparationPhase || StaticRules.instance.NowPhase == Phases.PreparationPhase2)
                        {
                            if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
                            {
                                if (StaticRules.instance.NowPreparationPhase == StaticRules.PreparationPhase.SetEvolition)
                                {
                                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(_Digicarta.GetComponent<CartaDigimon>());
                                }
                            }
                            else
                            {
                                MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos(_Digicarta.GetComponent<CartaDigimon>());
                            }
                        }
                        break;
                }
            }
        }
    }

    public static void SaltoFase(Phases phase)
    {

        StaticRules.instance.NowPhase = phase;
        Debug.Log(StaticRules.instance.NowPhase);

        switch (StaticRules.instance.NowPhase)
        {
            case Phases.GameSetup:
                StartGameSetup();
                break;
            case Phases.DiscardPhase:
                PartidaManager.instance.CambioDePhase(true);

                PartidaManager.instance.Cambio("Discard Phase");
                StartDiscardPhase();
                break;
            case Phases.PreparationPhase:
                PartidaManager.instance.Cambio("Preparation Phase");
                StartPreparationPhase();
                break;
            case Phases.EvolutionPhase:
                PartidaManager.instance.Cambio("Evolution Phase");
                StartEvolutionPhase();
                break;
            case Phases.FusionRequirements:
                PartidaManager.instance.CambioDePhase(false);
                CheckFusionRequirements();
                break;
            case Phases.AppearanceRequirements:
                PartidaManager.instance.CambioDePhase(true);
                CheckAppearanceRequirements();
                break;
            case Phases.BattlePhase:
                PartidaManager.instance.CambioDePhase(false);
                PartidaManager.instance.Cambio("Battle Phase");
                StartBattlePhase();
                break;
            case Phases.OptionBattlePhase:
                StartBattlePhase5("Salto Normal");
                break;
            case Phases.PointCalculationPhase:
                PartidaManager.instance.CambioDePhase(false);
                PartidaManager.instance.Cambio("Point Calculation Phase");
                StartPointCalculationPhase();
                break;
            case Phases.EndPhase:
                PartidaManager.instance.Cambio("End Phase");
                StartEndPhase("");
                break;
            default:
                Console.WriteLine("Error en el cambio de fase");
                break;
        }

    }

    public static bool isDigimonOrChip(DigiCarta carta)
    {
        if (carta.Capasidad > 0)
            return true;
        return false;
    }



    /// <summary>
    /// Aumenta en 1 el indice de la fase actual y ejecuta la siguiente.
    /// </summary>
    /// 

    public static void SiguienteFase()
    {
        if (StaticRules.instance.NowPhase == DigiCartas.Phases.OptionBattlePhase)
        {
            StaticRules.instance.OptionPlayerComplete();
            return;
        }


        StaticRules.instance.NowPhase++;
        Debug.Log(StaticRules.instance.NowPhase);
        switch (StaticRules.instance.NowPhase)
        {
            case Phases.GameSetup:
                PartidaManager.instance.CambioDePhase(false);
                StartGameSetup();
                break;
            case Phases.DiscardPhase:

                IA.instance.TerminarTurno("");
                PartidaManager.instance.CambioDePhase(true);
                PartidaManager.instance.MenuPhases.ChangePhase(true);
                PartidaManager.instance.Cambio("Discard Phase");
                StartDiscardPhase();
                break;
            case Phases.WhaitDiscardPhase:
                // se espera a cambiar de phase
                PartidaManager.instance.CambioDePhase(false);
                StaticRules.instance.DiscardCards(PartidaManager.instance.Player1);
                break;
            case Phases.PreparationPhase:

                // Revisamos quien juega primero
                PartidaManager.instance.MenuPhases.ChangePhase(true);
                if (StaticRules.instance.PlayerFirstAtack == PartidaManager.instance.Player1)
                {

                    StartPreparationPhase();
                    PartidaManager.instance.ActivateHand(true);
                    PartidaManager.instance.CambioDePhase(true);
                    PartidaManager.instance.Cambio("Preparation Phase");

                }
                else
                {
                    StartPreparationPhase();
                    PartidaManager.instance.ActivateHand(false);
                    PartidaManager.instance.CambioDePhase(false);
                    PartidaManager.instance.Cambio("Preparation Phase");
                    IA.instance.TurnoIA(true);
                }
                break;
            case Phases.PreparationPhase2:
                if (StaticRules.instance.PlayerFirstAtack == PartidaManager.instance.Player1)
                {
                    PartidaManager.instance.ActivateHand(false);
                    PartidaManager.instance.CambioDePhase(false);
                    IA.instance.TurnoIA(false);
                }
                else
                {
                    PartidaManager.instance.ActivateHand(true);
                    PartidaManager.instance.CambioDePhase(true);
                    PartidaManager.instance.MenuPhases.ChangePhase(true);
                    PartidaManager.instance.ActivateHand(true);
                }
                break;
            case Phases.EvolutionPhase:
                if (StaticRules.instance.PlayerFirstAtack == PartidaManager.instance.Player1)
                {
                    // Player 1 Evoluciona Primero
                    PartidaManager.instance.MenuPhases.ChangePhase(true);
                    PartidaManager.instance.CambioDePhase(false);
                    PartidaManager.instance.ActivateHand(false);
                    PartidaManager.instance.Cambio("Evolution Phase");
                    StartEvolutionPhase();
                }
                else
                {
                    // Ia Evoluciona primero
                    PartidaManager.instance.MenuPhases.ChangePhase(false);
                    PartidaManager.instance.CambioDePhase(false);
                    PartidaManager.instance.ActivateHand(false);
                    PartidaManager.instance.Cambio("Evolution Phase");
                    IA.instance.TurnoIA(false);
                }
                break;

            case Phases.EvolutionPhase2:
                if (StaticRules.instance.PlayerFirstAtack == PartidaManager.instance.Player1)
                {
                    PartidaManager.instance.MenuPhases.ChangePhase(false);
                    PartidaManager.instance.CambioDePhase(false);
                    PartidaManager.instance.ActivateHand(false);
                    PartidaManager.instance.Cambio("Evolution Phase");
                    IA.instance.TurnoIA(false);

                }
                else
                {
                    // Mandar a Preguntar si quiere seguir evolucionado
                    PartidaManager.instance.MenuPhases.ChangePhase(true);
                    PartidaManager.instance.CambioDePhase(false);
                    PartidaManager.instance.ActivateHand(false);
                    StartEvolutionPhase();
                }
                break;
            case Phases.EvolutionRequirements:
                PartidaManager.instance.CambioDePhase(false);
                foreach (var item in PartidaManager.instance.Player1._Mano.Cartas)
                {
                    item.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().CanvasSeleted.SetActive(false);
                }
                CheckEvolutionRequirements(PartidaManager.instance.Player1);
                break;

            case Phases.EvolutionRequirements2:
                CheckEvolutionRequirements(PartidaManager.instance.Player2);
                break;

            case Phases.FusionRequirements:
                PartidaManager.instance.CambioDePhase(false);
                CheckFusionRequirements();
                break;
            case Phases.AppearanceRequirements:
                PartidaManager.instance.CambioDePhase(false);
                CheckAppearanceRequirements();
                break;
            case Phases.BattlePhase:
                IA.instance.TerminarTurno("");
                PartidaManager.instance.MenuPhases.ChangePhase(true);
                PartidaManager.instance.CambioDePhase(false);
                PartidaManager.instance.Cambio("Battle Phase");
                StartBattlePhase();
                break;
            case Phases.OptionBattlePhase:
                StartBattlePhase5("Salto Normal");
                break;
            case Phases.PointCalculationPhase:
                if (StaticRules.instance.WhosPlayer == IA.instance.IAPlayer)
                    IA.instance.TerminarTurno("");

                PartidaManager.instance.CambioDePhase(false);
                PartidaManager.instance.Cambio("Point Calculation Phase");
                StartPointCalculationPhase();
                break;
            case Phases.EndPhase:
                PartidaManager.instance.CambioDePhase(false);
                PartidaManager.instance.Cambio("End Phase");
                StartEndPhase("");
                break;
            default:
                Console.WriteLine("Error en el cambio de fase");
                break;
        }


    }
    public void OptionPlayerComplete()
    {
        StaticRules.instance.WhaitPlayersReadyBattlet(true);
        IA.instance.TurnoIA(false);
    }

    //INICIO metodos para cada fase

    /// <summary>
    /// Inicial la Game Setup phase
    /// </summary>
    public static void StartGameSetup()
    {
        Debug.Log("This is GameSetup");
        StaticRules.instance.LostPlayerRound = null;

        // aumentar Tuno a las Option Card de la Ia

        if (MesaManager.instance.Campo2.OptionSlot1.GetComponent<OptionSlot>().OpCarta)
        {
            MesaManager.instance.Campo2.OptionSlot1.GetComponent<OptionSlot>().TurnOpcard += 1;
        }
        if (MesaManager.instance.Campo2.OptionSlot2.GetComponent<OptionSlot>().OpCarta)
        {
            MesaManager.instance.Campo2.OptionSlot2.GetComponent<OptionSlot>().TurnOpcard += 1;
        }
        if (MesaManager.instance.Campo2.OptionSlot3.GetComponent<OptionSlot>().OpCarta)
        {
            MesaManager.instance.Campo2.OptionSlot3.GetComponent<OptionSlot>().TurnOpcard += 1;
        }

        if (StaticRules.instance.PlayerFirstAtack != StaticRules.instance.WhosPlayer)
            StaticRules.instance.WhosPlayer = StaticRules.instance.PlayerFirstAtack;
        // jalar carta para ambos 
        for (int i = PartidaManager.instance.Player1._Mano.Cartas.Count; i < 6; i++)
        {
            PartidaManager.instance.TomarCarta(PartidaManager.instance.ManoPlayer1, PartidaManager.instance.Player1, MesaManager.instance.Campo1.NetOcean);
        }
        // jalar carta para ambos 
        for (int i = PartidaManager.instance.Player2._Mano.Cartas.Count; i < 6; i++)
        {
            PartidaManager.instance.TomarCarta(PartidaManager.instance.ManoPlayer2, PartidaManager.instance.Player2, MesaManager.instance.Campo2.NetOcean);
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
        {
            StaticRules.instance.InterTimePhase(2F);
        }
        else
        {
            Tutorial.instance.TargetID = 26;
            Tutorial.instance.Iniciar();
        }

    }

    private static void StartDiscardPhase()
    {
        // Este metodo se llama para que la Ia haga su fase de Descarte
        IA.instance.DiscarPhase();
    }
    public void StartPreparationPhaseDiscard(Player Jugador)
    {
        if (Jugador == PartidaManager.instance.Player1)
        {
            //agregamos a la mano las cartas descartadas
            for (int i = PartidaManager.instance.Player1._Mano.Cartas.Count; i < 6; i++)
            {
                PartidaManager.instance.TomarCarta(PartidaManager.instance.ManoPlayer1,
                PartidaManager.instance.Player1, MesaManager.instance.GetSlot(MesaManager.Slots.
                NetOcean, PartidaManager.instance.Player1));
            }
            //Vaciamos lista de Cartas 
            CartasDescartadas = new List<GameObject>();
            // Habilitamos la Mano
            PartidaManager.instance.ActivateHand(true);
        }
        else
        {
            //agregamos a la mano las cartas descartadas
            for (int i = PartidaManager.instance.Player2._Mano.Cartas.Count; i < 6; i++)
            {
                PartidaManager.instance.TomarCarta(PartidaManager.instance.ManoPlayer2,
                PartidaManager.instance.Player2, MesaManager.instance.GetSlot(MesaManager.Slots.
                NetOcean, PartidaManager.instance.Player2));
            }
            //Vaciamos lista de Cartas 
            CartasDescartadasIA = new List<GameObject>();
            // Habilitamos la Mano
            // PartidaManager.instance.ActivateHand(true);
        }
    }
    public void EndDiscardPhase(string result)
    {
        // Este medoto se llama para que la Ia haga su fase de Descart
        Debug.Log("hOLIS END DISCARD");
        SiguienteFase();
    }

    public void DiscardPhaseEndPlayer(string result)
    {
        Debug.Log("Player:" + result + " termino su descarte");
        // mandamos a metodo de espera
        StaticRules.instance.WaithPlayers(EndDiscardPhase);

    }
    public void AddListDiscard(GameObject Carta, bool addOrRemove)
    {
        StaticRules loRule = FailSafeInstance();
        if (addOrRemove)
        {
            loRule.CartasDescartadas.Add(Carta);

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
            {
                Tutorial.instance.Iniciar();
            }
        }
        else
        {
            loRule.CartasDescartadas.Remove(Carta);
        }
    }

    public void DiscardCards(Player Jugador)
    {
        if (Jugador == PartidaManager.instance.Player1)
        {
            // Descratamos cartas del player 1

            // DESCARTAMOS LAS CARTAS
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player1).GetComponent<DarkArea>().moviendo = false;
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player1).GetComponent<DarkArea>().setAction(DiscardPhaseEndPlayer);
            if (CartasDescartadas.Count == 0)
                DiscardPhaseEndPlayer("Salto la Fase sin Descartar");
            foreach (var item in CartasDescartadas)
            {
                CartaDigimon _CARD = item.GetComponent<CartaDigimon>();
                _CARD.Front.GetComponent<MovimientoCartas>().CanvasSeleted.SetActive(false);
                PartidaManager.instance.Player1._Mano.DescartarCarta(_CARD);
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player1).GetComponent<DarkArea>().AddListDescarte(item.GetComponent<CartaDigimon>(), 0.38f);
            }
            foreach (var item in PartidaManager.instance.Player1._Mano.Cartas)
            {
                item.Front.GetComponent<MovimientoCartas>().preparationPhase();
            }
        }
        else
        {
            //Descartamos catas de la IA
            // DESCARTAMOS LAS CARTAS
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player2).GetComponent<DarkArea>().moviendo = false;
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player2).GetComponent<DarkArea>().setAction(DiscardPhaseEndPlayer);
            if (CartasDescartadasIA.Count == 0)
                DiscardPhaseEndPlayer("Salto la Fase sin Descartar");
            foreach (var item in CartasDescartadasIA)
            {
                CartaDigimon _CARD = item.GetComponent<CartaDigimon>();
                _CARD.Front.GetComponent<MovimientoCartas>().CanvasSeleted.SetActive(false);
                PartidaManager.instance.Player2._Mano.DescartarCarta(_CARD);
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player2).GetComponent<DarkArea>().AddListDescarte(item.GetComponent<CartaDigimon>(), 0.38f);
            }
            foreach (var item in PartidaManager.instance.Player2._Mano.Cartas)
            {
                item.Front.GetComponent<MovimientoCartas>().preparationPhase();
            }
        }
    }

    public static bool CheckEvolutionList(CartaDigimon evolucion, CartaDigimon Digimon)
    {
        string nombreDigimon;

        nombreDigimon = Digimon.DatosDigimon.Nombre.ToUpper();
        nombreDigimon = nombreDigimon.Replace(" ", "");

        foreach (string requerimiento in evolucion.GetComponent<CartaDigimon>().DatosDigimon.ListaRequerimientos)
        {
            Debug.Log(nombreDigimon + ":" + requerimiento.Split(' ')[0].Replace(" ", ""));
            if (nombreDigimon.Equals(requerimiento.Split(' ')[0].Replace(" ", "")))
            {
                Debug.Log(nombreDigimon + "==" + requerimiento.Split(' ')[0].Replace(" ", ""));
                return true;
            }
            else if (requerimiento.Split(' ').Length >= 1)
            {
                try
                {

                    if (requerimiento.Split(' ')[1].Equals("+"))
                    {
                        if (nombreDigimon.Equals(requerimiento.Split(' ')[2]))
                        {
                            Debug.Log(nombreDigimon + "==" + requerimiento.Split(' ')[0].Replace(" ", ""));
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        return false;
    }
    public static List<string> GetListRequerimentsDigimon(DigiCarta DatosDigimon, DigiCarta BaseDigimon)
    {
        foreach (var item in DatosDigimon.ListaRequerimientos)
        {
            string Requerimiento = item.ToUpper();
            string FiltroNombre = BaseDigimon.Nombre.ToUpper().Replace(" ", "");
            if (Requerimiento.Contains(FiltroNombre))
            {
                List<string> RequerimientosFinales = new List<string>();

                string[] Reque = Requerimiento.Split(' ');
                foreach (string item2 in Reque)
                {
                    RequerimientosFinales.Add(item2);
                }
                return RequerimientosFinales;
            }
        }
        return new List<string>();
    }

    /// <summary>
    /// Inicial la Preparation phase
    /// </summary>
    private static void StartPreparationPhase()
    {

        StaticRules loRule = FailSafeInstance();
        Debug.Log("preparationPhase" + loRule.PlayerFirstAtack.Nombre);
        StaticRules.instance.NowPreparationPhase++;

        // AGREGAMOS LAS CARTAS A LA MANO DE LOS JUGADORES

        StaticRules.instance.StartPreparationPhaseDiscard(PartidaManager.instance.Player1);
        StaticRules.instance.StartPreparationPhaseDiscard(PartidaManager.instance.Player2);
    }


    public List<DigiEvoluciones> ListEvos = new List<DigiEvoluciones>();
    public void Evol(DigiEvoluciones Evolucion, int CantEvos)
    {

        ListEvos.Add(Evolucion);

        if (ListEvos.Count == CantEvos)
        {
            RecursivoEvo(ListEvos[0].DigiCarta, ListEvos[0].isJoggres, ListEvos[0].IsIgnore);
        }
        else
        {
            // las evoluciones no concuerdan
            if (WhosPlayer == PartidaManager.instance.Player1)
                PartidaManager.instance.CambioDePhase(true);
            else
                IA.instance.FinishTurnoIA();
        }
    }
    public void RecursivoEvo(Transform Evolucion, bool joggres, bool ignoreRe)
    {

        //foreach (var Request in MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().ListaRequerimientos)
        //{);
        Debug.Log(Evolucion.GetComponent<CartaDigimon>().name);

        List<string> requerimientos = StaticRules.GetListRequerimentsDigimon(Evolucion.GetComponent<CartaDigimon>().DatosDigimon,
             MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon);

        int contO = 0;
        int contX = 0;
        int RequestEvo = 0;

        if (requerimientos.Count > 0 && !ignoreRe)
        {

            List<Transform> Dcards = new List<Transform>();
            foreach (var item in requerimientos)
            {
                if (item == "O")
                {
                    // quitar carta delos requisitos 
                    Transform _DigiCarta = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().O;

                    if (_DigiCarta.childCount > contO)
                    {
                        Debug.Log(_DigiCarta.childCount + ":" + contO);
                        _DigiCarta = _DigiCarta.transform.GetChild(contO).transform;
                        contO++;
                        RequestEvo++;
                        SendDarkArea(_DigiCarta, 0.6f);
                    }
                }
                else if (item == ("X"))
                {
                    // quitar carta delos requisitos 
                    Transform _DigiCarta = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().X;
                    if (_DigiCarta.childCount > 0 && _DigiCarta)
                    {
                        _DigiCarta = _DigiCarta.transform.GetChild(contX).transform;
                        contX++;
                        RequestEvo++;
                        SendDarkArea(_DigiCarta, 0.6f);
                    }
                }
                else
                {
                    CartaDigimon DBox = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta;
                    string FiltroNombre = DBox.DatosDigimon.Nombre.ToUpper().Replace(" ", "");
                    // Revisar si el requisto es el digimon base 
                    if (item.ToUpper().Contains(FiltroNombre))
                    {
                        RequestEvo++;
                    }
                    else
                    {
                        // fusion o digimons extra o cartas de plugin
                        // revisar si la carta esta en los requisitos

                        Transform ReEvo = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox);
                        foreach (var Requeriment in ReEvo.GetComponent<EvolutionRequerimentBox>().ListaRequerimientosAdicionales)
                        {
                            if (Requeriment.GetComponent<CartaDigimon>().DatosDigimon.Nombre.ToUpper() == item)
                            {
                                RequestEvo++;
                                Dcards.Add(Requeriment.transform);
                            }
                            else
                            {
                                Debug.Log(Requeriment.GetComponent<CartaDigimon>().DatosDigimon.Nombre.ToUpper() + ":" + item + ",XD");
                                string name = Requeriment.GetComponent<CartaDigimon>().DatosDigimon.Nombre;
                                string ShapeName = name.Substring(name.Length - 4, 3);

                                if (ShapeName.Contains(item))
                                {
                                    RequestEvo++;
                                    Dcards.Add(Requeriment.transform);
                                }
                            }
                        }
                        if (RequestEvo != requerimientos.Count)
                        {
                            if (item == "60%")
                            {
                                // buscamos el plugin 
                                DigiCarta digicard = new DigiCarta();
                                digicard.id = 60;
                                Transform slot = MesaManager.instance.GetOptionSlotForCard(digicard);
                                if (slot)
                                {

                                    CartaDigimon OpCard = slot.GetComponent<OptionSlot>().OpCarta;
                                    OpCard.Volteo();
                                    slot.GetComponent<OptionSlot>().Vaciar();
                                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetAdicionalRequiriment(OpCard.transform);
                                    RequestEvo++;
                                    Dcards.Add(OpCard.transform);

                                }
                            }
                            else if (item == "40%")
                            {
                                // buscamos el plugin 
                                DigiCarta digicard = new DigiCarta();
                                digicard.id = 59;
                                Transform slot = MesaManager.instance.GetOptionSlotForCard(digicard);
                                if (slot)
                                {
                                    CartaDigimon OpCard = slot.GetComponent<OptionSlot>().OpCarta;
                                    OpCard.Volteo();
                                    slot.GetComponent<OptionSlot>().Vaciar();
                                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetAdicionalRequiriment(OpCard.transform);
                                    RequestEvo++;
                                    Dcards.Add(OpCard.transform);

                                }
                            }
                            if (joggres)
                            {
                                if (item.Contains("+"))
                                {
                                    RequestEvo++;
                                }
                            }
                        }
                    }
                }
            }
            Debug.Log(RequestEvo + ":" + requerimientos.Count);
            if (RequestEvo >= requerimientos.Count)
            {
                foreach (var item in Dcards)
                {
                    if (item.GetComponent<CartaDigimon>().DatosDigimon.id == 60)
                    {
                        DiscardRamdomCardHand(1);
                    }
                    SendDarkArea(item, 0.5f);
                }
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().moviendo = false;
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(StaticRules.instance.SetEvolution);
                ListEvos.RemoveAll(x => x.DigiCarta == Evolucion);
                Invoke("ReEvent", 3.5f);
                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evolucionar(Evolucion.GetComponent<CartaDigimon>().DatosDigimon.Nivel, joggres);
            }
            else
            {
                if (WhosPlayer == PartidaManager.instance.Player1)
                    PartidaManager.instance.CambioDePhase(true);
                else
                    IA.instance.FinishTurnoIA();

                ListEvos.RemoveAll(x => x.DigiCarta == Evolucion);
                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();

                if (joggres)
                {
                    StaticRules.instance.InterTimePhase(0.4f);
                }

                if (FinishEvolution != null)
                {
                    FinishEvolution.Invoke("Termino");
                }

            }
        }
        else if (ignoreRe)
        {

            // ignoremos los requisitos para esta evolucion
            SetEvolution("");
            MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evolucionar(Evolucion.GetComponent<CartaDigimon>().DatosDigimon.Nivel, joggres);

            ListEvos.RemoveAll(x => x.DigiCarta == Evolucion);
            Invoke("ReEvent", 3.5f);
        }
        else
        {
            if (WhosPlayer == PartidaManager.instance.Player1)
                PartidaManager.instance.CambioDePhase(true);
            else
                IA.instance.FinishTurnoIA();

            ListEvos.RemoveAll(x => x.DigiCarta == Evolucion);
            MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
            if (joggres)
            {
                StaticRules.instance.InterTimePhase(0.4f);
            }
        }

    }


    public void ReEvent()
    {
        if (ListEvos.Count == 0)
        {
            if (WhosPlayer == PartidaManager.instance.Player2)
            {
                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
                if (StaticRules.instance.NowPhase < DigiCartas.Phases.EvolutionRequirements)
                    IA.instance.FinishTurnoIA();
            }
            else
            {
                if (FinishEvolution != null)
                {
                    FinishEvolution.Invoke("Termino");
                }
                PartidaManager.instance.CambioDePhase(true);

                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
            }
        }
        else
        {
            MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
            RecursivoEvo(ListEvos[0].DigiCarta, ListEvos[0].isJoggres, ListEvos[0].IsIgnore);
        }
    }
    public static void SecondEvolutionPhase()
    {
        DarkArea DarkArea = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>();
        EvolutionRequerimentBox EvoRe = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>();
        EvolutionBox Evolution = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>();

        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
        // Tirar las cartas sobrantes a la dark area

        DarkArea.moviendo = false;
        //> primero Requisitos
        foreach (var item in EvoRe.ListaXO)
        {
            DarkArea.AddListDescarte(item.GetComponent<CartaDigimon>(), 0.5f, true);
        }
        foreach (var item2 in EvoRe.ListaRequerimientosAdicionales)
        {
            DarkArea.AddListDescarte(item2.GetComponent<CartaDigimon>(), 0.5f, true);
        }
        foreach (var item3 in Evolution.Cartas)
        {
            DarkArea.AddListDescarte(item3.GetComponent<CartaDigimon>(), 0.5f, true);
        }
        Evolution.Cartas = new List<CartaDigimon>();
        EvoRe.EndEvolution();
    }

    /// <summary>
    /// Inicial la Evolution phase
    /// </summary>
    public static void StartEvolutionPhase()
    {
        // voltear Digimon de Player 1
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta.Volteo();

        // Realizar Evolucion correspondiente 
        if (MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas.Count != 0)
        {
            foreach (var Evolucion in MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas)
            {
                DigiEvoluciones ItemEvo = new DigiEvoluciones();
                ItemEvo.DigiCarta = Evolucion.transform;
                StaticRules.instance.Evol(ItemEvo, MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas.Count);
            }

        }
        else
        {
            if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
                PartidaManager.instance.CambioDePhase(true);
            else
                IA.instance.TerminarTurno("");
        }
    }
    public void SetEvolution(string tiempo)
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evolution(
        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas[0].transform);
        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().NowPhase();

    }


    public static void SendDarkArea(Transform Dcard, float tiempo)
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, StaticRules.instance.WhosPlayer).GetComponent<DarkArea>().AddListDescarte(Dcard.GetComponent<CartaDigimon>(), tiempo, true);
    }
    private static void CheckEvolutionRequirements(Player Jugador)
    {


        if (PartidaManager.instance.Player1 == Jugador)
        {
            // revisa si la evolucion no se llevo adecuadamente ya sea por efectos especiales
            if (MesaManager.instance.Campo1.EvolutionBox.GetComponent<EvolutionBox>().Cartas.Count > 0)
            {
                foreach (var Evolucion in MesaManager.instance.Campo1.EvolutionBox.GetComponent<EvolutionBox>().Cartas)
                {
                    bool CartaAgregada = false;
                    DigiEvoluciones ItemEvo = new DigiEvoluciones();
                    ItemEvo.DigiCarta = Evolucion.transform;
                    foreach (var item in StaticRules.instance.EfectosDeTurno)
                    {
                        if (item.Efecto == Efectos.SinRequerimientosAll && item.Jugador == StaticRules.instance.WhosPlayer)
                        {
                            // evolucionar como si nada 
                            ItemEvo.IsIgnore = true;
                            ItemEvo.Namefecto = Efectos.SinRequerimientosAll;
                            StaticRules.instance.Evol(ItemEvo,
                            MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).
                            GetComponent<EvolutionBox>().Cartas.Count);
                            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(StaticRules.instance.SetEvolution);
                            CartaAgregada = true;
                            break;
                        }
                        else if (item.Efecto == Efectos.SinRequerimientos4 && item.Jugador == StaticRules.instance.WhosPlayer)
                        {
                            // revisamos si tu digimon no es nivel 3 
                            if (MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent
                            <DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel != "III")
                            {
                                // evolucionar como si nada 

                                ItemEvo.IsIgnore = true;
                                ItemEvo.Namefecto = Efectos.SinRequerimientos4;
                                StaticRules.instance.Evol(ItemEvo,
                                MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).
                                GetComponent<EvolutionBox>().Cartas.Count);
                                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(StaticRules.instance.SetEvolution);
                                CartaAgregada = true;
                                break;
                            }
                        }
                    }
                    if (!CartaAgregada)
                    {
                        ItemEvo.IsIgnore = false;
                        // la carta no se agrego -- no existen efectos para evolucionar
                        StaticRules.instance.Evol(ItemEvo,
                        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).
                        GetComponent<EvolutionBox>().Cartas.Count);
                    }
                    else
                    {
                        StaticRules.instance.EfectosDeTurno.RemoveAll(item => item.Efecto == ItemEvo.Namefecto);
                    }
                }
                // vaciamos los efectos
                StaticRules.instance.EfectosDeTurno.RemoveAll(item => item.Jugador == StaticRules.instance.WhosPlayer);
            }
            else
            {
                StaticRules.instance.InterTimePhase(0.4f);
            }
        }

        else if (PartidaManager.instance.Player2==Jugador)
        {


        if (MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas.Count > 0)
        {
            // IA
            foreach (var Evolucion in MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas)
            {
                bool CartaAgregada = false;
                DigiEvoluciones ItemEvo = new DigiEvoluciones();
                ItemEvo.DigiCarta = Evolucion.transform;
                foreach (var item in StaticRules.instance.EfectosDeTurno)
                {
                    if (item.Efecto == Efectos.SinRequerimientosAll && item.Jugador == StaticRules.instance.WhosPlayer)
                    {
                        // evolucionar como si nada 
                        ItemEvo.IsIgnore = true;
                        ItemEvo.Namefecto = Efectos.SinRequerimientosAll;
                        StaticRules.instance.Evol(ItemEvo,
                        MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas.Count);
                        MesaManager.instance.Campo2.DarkArea.GetComponent<DarkArea>().setAction(StaticRules.instance.SetEvolution);
                        CartaAgregada = true;
                        break;
                    }
                    else if (item.Efecto == Efectos.SinRequerimientos4 && item.Jugador == StaticRules.instance.WhosPlayer)
                    {
                        // revisamos si tu digimon no es nivel 3 
                        if (MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent
                        <DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel != "III")
                        {
                            // evolucionar como si nada 

                            ItemEvo.IsIgnore = true;
                            ItemEvo.Namefecto = Efectos.SinRequerimientos4;
                            StaticRules.instance.Evol(ItemEvo,
                            MesaManager.instance.Campo2.EvolutionBox.
                            GetComponent<EvolutionBox>().Cartas.Count);
                            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(StaticRules.instance.SetEvolution);
                            CartaAgregada = true;
                            break;
                        }
                    }
                }
                if (!CartaAgregada)
                {
                    ItemEvo.IsIgnore = false;
                    // la carta no se agrego -- no existen efectos para evolucionar
                    StaticRules.instance.Evol(ItemEvo,
                    MesaManager.instance.Campo2.EvolutionBox.GetComponent<EvolutionBox>().Cartas.Count);
                }
                else
                {
                    StaticRules.instance.EfectosDeTurno.RemoveAll(item => item.Efecto == ItemEvo.Namefecto);
                }
            }
            // vaciamos los efectos
            StaticRules.instance.EfectosDeTurno.RemoveAll(item => item.Jugador == StaticRules.instance.WhosPlayer);
            }
            else
            {
                StaticRules.instance.InterTimePhase(0.4f);
            }
        }
    }


    private static void CheckFusionRequirements()
    {
        if (MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas.Count > 0)
        {
            // HAY EVO POSIBLE  
            // Realizar segundo intento de Evolucion correspondiente 
            foreach (var Evolucion in MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas)
            {
                DigiEvoluciones ItemEvo = new DigiEvoluciones();
                ItemEvo.DigiCarta = Evolucion.transform;
                ItemEvo.isJoggres = true;
                StaticRules.instance.Evol(ItemEvo, 1);
            }
        }
        else
        {
            StaticRules.instance.InterTimePhase(0.4f);
        }
    }
    public bool evo = false;
    private static void CheckAppearanceRequirements()
    {

        // descarta las cartas usadas y pasamos al la evolucion del rival
        StaticRules.instance.ListEvos = new List<DigiEvoluciones>();
        StaticRules.SecondEvolutionPhase();
        MesaManager.instance.Campo1.FronDigimon.GetComponent<FrontDigimon>().RevelarDigimon(MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta,
            MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.TipoBatalla);

        if (StaticRules.instance.evo)
            StaticRules.instance.InterTimePhase(0.4f);
        else
        {
            StaticRules.instance.evo = true;
            IA.instance.TurnoIA(false);
        }

    }

    public int WhatAtackUse(string Type, CartaDigimon carta)
    {
        switch (Type)
        {
            case "A":
                return carta.DatosDigimon.DanoAtaqueA;
            case "B":
                return carta.DatosDigimon.DanoAtaqueB;
            case "C":
                return carta.DatosDigimon.DanoAtaqueC;
            default:
                return 0;
        }
    }
    public bool WhatNivelMayor(string uno, string dos)
    {
        int nivel1 = 0;
        int nivel2 = 0;

        switch (uno)
        {
            case "III":
                nivel1 = 1;
                break;
            case "IV":
                nivel1 = 2;
                break;
            case "Perfect":
                nivel1 = 3;
                break;
            case "Ultimate":
                nivel1 = 4;
                break;
        }
        switch (dos)
        {
            case "III":
                nivel2 = 1;
                break;
            case "IV":
                nivel2 = 2;
                break;
            case "Perfect":
                nivel2 = 3;
                break;
            case "Ultimate":
                nivel2 = 4;
                break;
        }
        if (nivel1 > nivel2)
            return true;
        else
            return false;

    }

    /// <summary>
    /// Inicial la Battle phase
    /// </summary>
    private static void StartBattlePhase()
    {
        StaticRules.instance.evo = false;

        // Revelamos al Rival
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player2;

        MesaManager.instance.Campo2.FronDigimon.GetComponent<FrontDigimon>().RevelarDigimon(MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta,
            MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.TipoBatalla);
        Debug.Log("Hora de Pelear");

        IA.instance.TerminarTurno(null);
        DigimonBoxSlot DigimonBox1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();
        DigimonBoxSlot DigimonBox2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>();
        // saltarme fase del enemigo de evolution

        DigimonBox2._DigiCarta.Volteo();

        //inisializamos la batalla 
        DigimonBox1.BattelPhase(StaticRules.instance.WhatAtackUse(DigimonBox2._DigiCarta.DatosDigimon.TipoBatalla,
            DigimonBox1._DigiCarta), StaticRules.instance.StartBattlePhase2);

        DigimonBox2.BattelPhase(StaticRules.instance.WhatAtackUse(DigimonBox1._DigiCarta.DatosDigimon.TipoBatalla,
            DigimonBox2._DigiCarta), null);


    }

    private void StartBattlePhase2(string result)

    {
        DigimonBoxSlot DigimonBox1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();
        DigimonBoxSlot DigimonBox2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>();
        // Activar habilidad de Digimon de jugador 2
        // Activar habilidad de Digimon de jugador 1
        // revisar si ataco con C para efecto de Digimon jugador 1
        if (PlayerFirstAtack == PartidaManager.instance.Player1)
        {
            StaticRules.instance.EfectosDeAtaque(DigimonBox1._DigiCarta, DigimonBox2._DigiCarta, StaticRules.instance.StartBattlePhase3);
        }
        else
        {
            StaticRules.instance.EfectosDeAtaque(DigimonBox2._DigiCarta, DigimonBox1._DigiCarta, StaticRules.instance.StartBattlePhase3);
        }
        // revisar si ataco con C para efecto de Digimon jugador 2   
    }

    private void StartBattlePhase3(string result)
    {
        Debug.Log(result);
        DigimonBoxSlot DigimonBox1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();
        DigimonBoxSlot DigimonBox2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>();

        if (PlayerFirstAtack == PartidaManager.instance.Player1)
        {
            StaticRules.instance.EfectosDeAtaque(DigimonBox2._DigiCarta, DigimonBox1._DigiCarta, StaticRules.instance.StartBattlePhase4);
        }
        else
        {
            StaticRules.instance.EfectosDeAtaque(DigimonBox1._DigiCarta, DigimonBox2._DigiCarta, StaticRules.instance.StartBattlePhase4);

        }
    }
    private void StartBattlePhase4(string result)
    {
        // Jugador Dos ativa su carta y termina el Turno 

        PartidaManager.instance.CambioDePhase(true);
        // pasar turno si la Ia es el jugador 2
        StaticRules.instance.InterTimePhase(0.4f);

    }
    private static void StartBattlePhase5(string result)
    {
        if (StaticRules.instance.PlayerFirstAtack == PartidaManager.instance.Player2)
        {
            //  IA
            IA.instance.TurnoIA(false);
        }
        else
        {
            // ESPACIO PARA CARTAS
            PartidaManager.instance.ListoOption.gameObject.SetActive(true);
            PartidaManager.instance.Listo.gameObject.SetActive(false);

        }
    }


    public void EfectosDeAtaque(CartaDigimon DigiCartaAfectada, CartaDigimon DigimonContrario, UnityAction<string> Phase)
    {
        if (DigimonContrario.DatosDigimon.TipoBatalla == "C")
        {
            Guard("A", DigiCartaAfectada.DatosDigimon.TipoBatalla, 0, DigimonContrario, Phase);
        }
        else
        {
            Phase.Invoke("Tipo de Batalla no concuerda con la Habilidad");
        }
    }
    public void Guard(string Ataque, string TipoAtaquePropio, int cantidadresultante, CartaDigimon CartaTarget, UnityAction<string> Phase)
    {
        if (TipoAtaquePropio == Ataque)
        {
            DigimonBoxSlot slot = CartaTarget.transform.parent.GetComponent<DigimonBoxSlot>();
            slot.DEbuff.Play();
            slot.CanvasContador.GetComponent<ContadorOffencivo>().EFECTOS(cantidadresultante, Phase);
        }
        else
        {

            Phase.Invoke("Sin efecto");
        }
    }

    /// <summary>
    /// Inicial la Point Calculation phase
    /// </summary>
    private static void StartPointCalculationPhase()
    {
        //Ambos jugadores usaran cualquier Option Card que requiera ser usada durante la Point Calculation Phase.

        // mando atacar a jugador 1
        if (StaticRules.instance.PlayerFirstAtack == PartidaManager.instance.Player1)
        {
            Debug.Log("Ataca el 1");
            MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().IniciarAtaque();
        }

        else
        {
            Debug.Log("Ataca el 2");
            MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().IniciarAtaque();
        }

        //El jugador que haya perdido la batalla recibe daño
        CalcularPuntosPerdidos();
        //Empezando con el primer jugador en atacar, ambos usaran cualquier habilidad de Digimon que requiera ser usada después de la calculación de puntos.
        //Si el Digimon que perdió el combate no es un nivel III descarta todas las otras cartas de la Digimon Box.
        //Envía cualquier Digimon support en tu Support Box al Dark Area.
        //Si el Net Ocean se ha quedado sin cartas descarta todas las otras cartas de tu Digimon Box de manera que solo quede el Digimon de nivel III. Mezcla tu Dark Area, y úsalo como Net Ocean.
        //El jugador que gana la batalla se convertirá en el “primero en atacar” urante el siguiente turno. 
        //Si se empata, el orden de los turnos permanece como el turno anterior.
    }


    /// <summary>
    /// Inicial la End phase
    /// </summary>
    private static void StartEndPhase(string result)
    {
        Debug.Log("TurnoAcabado");
        StaticRules.instance.Listos = false;
        //activamos los efectos de la ronda
        try
        {
            StaticRules.instance.EfectosEstaticos(StartEndPhase2);
        }
        catch (Exception)
        {
            StartEndPhase2("ERROR");
        }

    }
    private static void StartEndPhase2(string result)
    {
        Debug.Log(result);
        StaticRules.instance.NowPhase = 0;
        StaticRules.instance.NowPreparationPhase = 0;
        StaticRules.instance.EfectosRonda = new List<Efecto>();

        MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().EndPhase();
        MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().EndPhase();

        MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().Cambiado = false;
        MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().Cambiado = false;

        // Quitar todas las catas usadas en la Ronda 

        // Player 1
        StaticRules.instance.ResetCardsToDarkArea(MesaManager.instance.Campo1, StartEndPhase3);

    }
    public void ResetCardsToDarkArea(Campos Campo, UnityAction<string> LoAction)
    {
        List<CartaDigimon> Cards = new List<CartaDigimon>();
        /*foreach (var item in Campo.EvolutionBox.GetComponent<EvolutionBox>().Cartas)
       {
           Cards.Add(item);
       }
       foreach (var item in Campo.EvolutionBox.GetComponent<EvolutionRequerimentBox>().ListaRequerimientosAdicionales)
        {
            Cards.Add(item);
        }
        foreach (var item in Campo.EvolutionBox.GetComponent<EvolutionRequerimentBox>().ListaXO)
        {
            Cards.Add(item);
        }


        if (!Campo.OptionSlot1.GetComponent<OptionSlot>().OpCarta.mostrar)
        {
            Cards.Add(Campo.OptionSlot1.GetComponent<OptionSlot>().OpCarta);
        }
        if (!Campo.OptionSlot2.GetComponent<OptionSlot>().OpCarta.mostrar)
        {
            Cards.Add(Campo.OptionSlot1.GetComponent<OptionSlot>().OpCarta);
        }
        if (!Campo.OptionSlot3.GetComponent<OptionSlot>().OpCarta.mostrar)
        {
            Cards.Add(Campo.OptionSlot1.GetComponent<OptionSlot>().OpCarta);
        }
        */
        // mandar A LA DarkArea

        if (Cards.Count == 0)
        {
            LoAction("Limpio");
        }
        else
        {
            Campo.DarkArea.GetComponent<DarkArea>().setAction(LoAction);
            foreach (var item in Cards)
            {
                Campo.DarkArea.GetComponent<DarkArea>().AddListDescarte(item, 0.3f);
            }
        }
    }
    private static void StartEndPhase3(string result)
    {
        // Quitar todas las catas usadas en la Ronda 

        // Player 2
        StaticRules.instance.ResetCardsToDarkArea(MesaManager.instance.Campo2, StartEndPhase4);
    }

    private static void StartEndPhase4(string result)
    {
        // revisamos si el deck esta vacio 
        Debug.Log(MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Cartas.Count);
        if (MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Cartas.Count == 0)
        {
            MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Reiniciar(StaticRules.instance.WhaitReinicio, PartidaManager.instance.Player1);
        }
        else
        {
            // revisamos el deck de la IA 
            if (MesaManager.instance.Campo2.NetOcean.GetComponent<NetOcean>().Cartas.Count == 0)
            {
                StaticRules.instance.WhosPlayer = PartidaManager.instance.Player2;
                MesaManager.instance.Campo2.NetOcean.GetComponent<NetOcean>().Reiniciar(StaticRules.instance.WhaitReinicioIA, IA.instance.IAPlayer);
            }
            else
                SaltoFase(Phases.GameSetup);
        }
    }



    public void WhaitReinicio(string resul)
    {
        // revisamos el deck de la IA 
        if (MesaManager.instance.Campo2.NetOcean.GetComponent<NetOcean>().Cartas.Count == 0)
        {
            StaticRules.instance.WhosPlayer = PartidaManager.instance.Player2;
            Debug.Log("Revisando Deck Ia");
            MesaManager.instance.Campo2.NetOcean.GetComponent<NetOcean>().Reiniciar(WhaitReinicioIA, IA.instance.IAPlayer);
        }
        else
            SaltoFase(Phases.GameSetup);
    }
    public void WhaitReinicioIA(string resul)
    {
        Debug.Log("MAzoIA Finish");
        IA.instance.TerminarTurno("");
        SaltoFase(Phases.GameSetup);
    }
    //FIN metodos para cada fase
    public void InterTimePhase(float time)
    {
        Invoke("NextPhase", time);
    }
    public void NextPhase()
    {
        SiguienteFase();
    }

    /// <summary>
    /// Comprueba si la vida de algun jugador se ha reducido a 0 o alguno de los jugadores se ha rendido.
    /// </summary>
    private bool CumplidaCondicionVictoria()
    {
        if (PointGaugePlayer1 <= 0 || PointGaugePlayer2 <= 0)
        {
            //Mostrar mensaje de victoria o derrota.
            return true; //Fin de la partida.
        }
        return false;
    }

    /// <summary>
    /// Fija el jugador activo al inicio de la partida.
    /// Metodo redundante, comprobar WhoFirstPlayer()
    /// </summary>
    public static void SeleccionPrimerJugador()
    {
        int aux = Random.Range(0, 2);
        if (aux == 0)
        {
            //currentPlayer = player1;
        }
        else
        {
            //CurrentPlayer = player2;
        }
    }

    /// <summary>
    /// Calcula el daño recibido en cada combate
    /// </summary>
    public static void CalcularPuntosPerdidos()
    {
        float Digimon1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().CanvasContador.PoderDeAtaque;
        float Digimon2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().CanvasContador.PoderDeAtaque;
        Debug.Log("FinalPoints:" + Digimon1 + " | " + Digimon2);
        if (Digimon1 > Digimon2)
        {
            //Gano Digimon 1
            PartidaManager.instance.WinTurno = PartidaManager.instance.Player1;
            // nivel de tu digimon
            string nivel = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel;
            // Obtenemos puntos que vamos a reducir al jugador 2
            int PuntosPerdidos = PerdidaPuntos(MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta, nivel);
            // descontamos los puntos
            StaticRules.instance.ReducirPuntos(PuntosPerdidos, true);
            if (!PartidaManager.instance.Player1 == StaticRules.instance.PlayerFirstAtack)
                ChangeFirstAtackPlayer();

        }
        else if (Digimon2 > Digimon1)
        {
            //Gano Digimon 2
            PartidaManager.instance.WinTurno = PartidaManager.instance.Player2;
            // nivel de tu digimon
            string nivel = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel;
            // Obtenemos puntos que vamos a reducir al jugador 2
            int PuntosPerdidos = PerdidaPuntos(MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta, nivel);
            // descontamos los puntos
            StaticRules.instance.ReducirPuntos(PuntosPerdidos, false);

            if (!PartidaManager.instance.Player2 == StaticRules.instance.PlayerFirstAtack)
                ChangeFirstAtackPlayer();
        }
        else if (Digimon1 == Digimon2)
        {
            // Empate Restamos a ambos 10 puntos
            StaticRules.instance.ReducirPuntos(10, true, true);
        }
        /*
        Fijándose en los Lost Points que dice su Digimon, y
        sustrayendo los puntos perdidos correspondientes al nivel del Digimon
        oponente, moviendo la carta del Point Gauge abajo. Si la batalla terminó en
        empate, ambos jugadores pierden 10 puntos, y sus Digimon permanecen
        como están.
         */

    }

    public void ReducirPuntos(int Cantidad, bool player1, bool empate = false)
    {
        Debug.Log(Cantidad + " :" + player1);
        if (empate)
        {
            PointGaugePlayer2 -= Cantidad;
            // mandar a reducir en el tablero player 2
            MesaManager.instance.Campo2.PointGauge.GetComponent<PointGaugeBox>().SetCard(PointGaugePlayer2);

            // mandar a reducir en el tablero player 1
            PointGaugePlayer1 -= Cantidad;
            MesaManager.instance.Campo1.PointGauge.GetComponent<PointGaugeBox>().SetCard(PointGaugePlayer1);

            // mandar a reducir en el tablero
            LostPlayerRound = new Player();
        }
        else
        {
            if (player1)
            {
                PointGaugePlayer2 -= Cantidad;
                // mandar a reducir en el tablero
                MesaManager.instance.Campo2.PointGauge.GetComponent<PointGaugeBox>().SetCard(PointGaugePlayer2);
                LostPlayerRound = PartidaManager.instance.Player2;
            }
            else
            {
                PointGaugePlayer1 -= Cantidad;
                MesaManager.instance.Campo1.PointGauge.GetComponent<PointGaugeBox>().SetCard(PointGaugePlayer1);
                LostPlayerRound = PartidaManager.instance.Player1;
                // mandar a reducir en el tablero
            }
        }
        RondPuntosPerdidos = Cantidad;
        Invoke("FinalPointPhase", 2f);
    }

    public void FinalPointPhase()
    {

        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().moviendo = false;
        if (LostPlayerRound == PartidaManager.instance.Player1)
        {
            MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(BeforePointFase);
        }
        else if (LostPlayerRound == PartidaManager.instance.Player2)
        {
            MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(BeforePointFase);
        }
        else
        {
            MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(null);
            MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(BeforePointFase);
        }

    }
    public void BeforePointFase(string result)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Tutorial")
        {
            StaticRules.instance.InterTimePhase(1);
        }
        else
        {
            Tutorial.instance.Iniciar();
        }
    }

    public static int PerdidaPuntos(CartaDigimon DCarta, string nivel)
    {
        switch (nivel)
        {
            case "III":
                return DCarta.DatosDigimon.PerdidaVidaIII;
            case "IV":
                return DCarta.DatosDigimon.PerdidaVidaIV;
            case "Perfect":
                return DCarta.DatosDigimon.PerdidaVidaPerfect;
            case "Ultimate":
                return DCarta.DatosDigimon.PerdidaVidaUltimate;
            default:
                return 0;
        }
    }

    public static void Ajustar(CartaDigimon _Carta)
    {
        _Carta.AjustarSlot();
    }

    public static void Victori()
    {
        if (StaticRules.instance.PointGaugePlayer1 <= 0 && StaticRules.instance.PointGaugePlayer2 > 0)
        {
            PartidaManager.instance.Cambio(PartidaManager.instance.Player2.Nombre + " a Ganado");
            CartaDigimon Digimon = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().DRoquin;
            PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.DarkArea, Digimon.transform, Ajustar);
            PlayerManager.instance.LosePlayer(StaticRules.instance.PointGaugePlayer2 / 10);
            PartidaManager.instance.Victory(false);
            return;
        }
        else if (StaticRules.instance.PointGaugePlayer2 <= 0 && StaticRules.instance.PointGaugePlayer1 >= 0)
        {
            PartidaManager.instance.Cambio(PartidaManager.instance.Player1.Nombre + " a Ganado");
            CartaDigimon Digimon = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().DRoquin;
            PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo2.DarkArea, Digimon.transform, Ajustar);
            PlayerManager.instance.WinPlayer(StaticRules.instance.PointGaugePlayer1 / 10);
            PartidaManager.instance.Victory(true);
        }
        else
        {
            PartidaManager.instance.Cambio("Empate");
            PlayerManager.instance.WinPlayer(-5);
            PartidaManager.instance.Victory(true);
        }

    }


    public static void ActivateOptionCard(CartaDigimon OpCard)
    {
        foreach (var item in OpCard.DatosDigimon.ListaActivacion)
        {
            if (StaticRules.instance.ConvertPhases(item) == StaticRules.instance.NowPhase || StaticRules.instance.ConvertPhases2(item) == StaticRules.instance.NowPhase)
            {
                OpCard.Volteo();
                List<string> Efectos = OpCard.DatosDigimon.ListaEfectos;
                foreach (var item2 in Efectos)
                {

                    StaticRules.instance.WhatEfect(item2, OpCard);
                }
            }
        }

    }
    public void WhatEfect(string efecto, CartaDigimon Opcard)
    {
        List<Efecto> EF = new List<Efecto>();

        switch (efecto)
        {
            case "allyChageAtack to A":
                Efecto _ef = new Efecto();
                _ef.NameEfecto = EfectosActivos.allyChageAtack;
                _ef.Ataque = "A";
                _ef.OptionCard = Opcard;
                _ef.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef.CartaAfecta.Add(MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).
                    GetComponent<DigimonBoxSlot>()._DigiCarta);
                _ef.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef);
                break;
            case "allyChageAtack to C":
                Efecto _ef1 = new Efecto();
                _ef1.NameEfecto = EfectosActivos.allyChageAtack;
                _ef1.Ataque = "C";
                _ef1.OptionCard = Opcard;
                _ef1.CartaAfecta.Add(MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).
                    GetComponent<DigimonBoxSlot>()._DigiCarta);
                _ef1.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef1.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef1);
                break;
            case "allyChageAtack to B":
                Efecto _ef2 = new Efecto();
                _ef2.NameEfecto = EfectosActivos.allyChageAtack;
                _ef2.Ataque = "B";
                _ef2.OptionCard = Opcard;
                _ef2.CartaAfecta.Add(MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).
                    GetComponent<DigimonBoxSlot>()._DigiCarta);
                _ef2.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef2.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef2);
                break;

            case "enemy drop 3 anycards from netocean in darkarea where lostbattle":
                Efecto _ef3 = new Efecto();
                _ef3.NameEfecto = EfectosActivos.dropCards;
                _ef3.cantidadCartas = 3;
                _ef3.OptionCard = Opcard;
                _ef3.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                if (WhosPlayer == PartidaManager.instance.Player1)
                {
                    _ef3.Origen = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, PartidaManager.instance.Player2);
                    _ef3.Destino = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player2);
                }
                else
                {
                    _ef3.Origen = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, PartidaManager.instance.Player1);
                    _ef3.Destino = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player1);
                }

                _ef3.where = "lostbattle";
                _ef3.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef3);
                break;
            case "allyAttackPoints + 50":
                Efecto _ef4 = new Efecto();
                _ef4.NameEfecto = EfectosActivos.BuffAtack;
                _ef4.buffo = 50;
                _ef4.OptionCard = Opcard;
                _ef4.CartaAfecta.Add(MesaManager.instance.GetSlot(MesaManager.Slots.
                    DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta);
                _ef4.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef4.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef4);
                break;
            case "battlePhase enemyBigDigimon allyAttackPoints * 2 where allyL3 not use":
                Efecto _ef5 = new Efecto();
                DigimonBoxSlot midigi = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>();
                DigimonBoxSlot digienemigo = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot, PartidaManager.instance.GetEnemy()).GetComponent<DigimonBoxSlot>();

                _ef5.NameEfecto = EfectosActivos.doblePower;
                _ef5.OptionCard = Opcard;
                _ef5.buffo = 2;
                _ef5.CartaAfecta.Add(MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta);
                _ef5.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef5.Jugador = StaticRules.instance.WhosPlayer;
                _ef5.where = "allyL3 not use";
                if (WhatNivelMayor(digienemigo._DigiCarta.DatosDigimon.Nivel, midigi._DigiCarta.DatosDigimon.Nivel) && digienemigo._DigiCarta.DatosDigimon.Nivel != "III")
                {
                    Debug.Log("El enemigo es mayor nivel");
                    EF.Add(_ef5);
                }

                break;
            case "QuitDigimonBox":
                Efecto _ef7 = new Efecto();
                CartaDigimon midigi2 = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta;
                _ef7.NameEfecto = EfectosActivos.QuitDigimonBox;
                _ef7.OptionCard = Opcard;
                _ef7.CartaAfecta.Add(midigi2);
                _ef7.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef7.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef7);
                break;
            case "winBattle enemyLostPoints * 2":
                Efecto _ef12 = new Efecto();
                _ef12.NameEfecto = EfectosActivos.doublelostpoint;
                _ef12.OptionCard = Opcard;
                _ef12.buffo = 2;
                _ef12.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef12.Jugador = StaticRules.instance.WhosPlayer;
                _ef12.Destino = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea);
                _ef12.where = "lostbattle";
                EF.Add(_ef12);
                break;
            case "return to Lv3 allyLv4 or enemyLv4":
                Efecto _ef11 = new Efecto();
                DigimonBoxSlot midigi3 = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>();
                DigimonBoxSlot digienemigo2 = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot, PartidaManager.instance.GetEnemy()).GetComponent<DigimonBoxSlot>();

                _ef11.OptionCard = Opcard;
                _ef11.NameEfecto = EfectosActivos.EnemyDesDigivolucionar;
                _ef11.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                if (WhosPlayer == PartidaManager.instance.Player1)
                {
                    _ef11.Jugador = PartidaManager.instance.Player2;
                }
                else
                {
                    _ef11.Jugador = PartidaManager.instance.Player1;
                }
                _ef11.where = "IV to III";
                EF.Add(_ef11);


                break;
            case "allyEvolutionRequierimets allyDigivolution to nextLvl all allyHand drop in allyDarkArea":
                Efecto _ef8 = new Efecto();
                _ef8.NameEfecto = EfectosActivos.IgnorAll;
                _ef8.OptionCard = Opcard;
                _ef8.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef8.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef8);
                break;
            case "allyEvolutionRequierimets allyDigivolution to Lv4":
                Efecto _ef9 = new Efecto();
                _ef9.NameEfecto = EfectosActivos.Ignor4;
                _ef9.OptionCard = Opcard;
                _ef9.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef9.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef9);
                break;
            case "drop in allyDarkArea":
                Efecto _ef6 = new Efecto();
                _ef6.CartaAfecta.Add(Opcard);
                _ef6.OptionCard = Opcard;
                _ef6.NameEfecto = EfectosActivos.SetDarkArea;
                _ef6.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef6.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef6);
                break;
            case "drop allHand allyDarkArea":
                Efecto _ef10 = new Efecto();
                foreach (var item in StaticRules.instance.WhosPlayer._Mano.Cartas)
                {
                    _ef10.CartaAfecta.Add(item);
                }
                _ef10.OptionCard = Opcard;
                _ef10.NameEfecto = EfectosActivos.DiscardHand;
                _ef10.Limite = ConvertPhases(Opcard.DatosDigimon.Limite);
                _ef10.Jugador = StaticRules.instance.WhosPlayer;
                EF.Add(_ef10);
                break;
            default:
                break;
        }

        if (StaticRules.instance.NowPhase == DigiCartas.Phases.OptionBattlePhase && WhosPlayer == PartidaManager
            .instance.Player1 && EF.Count > 0)
            StaticRules.instance.WhaitPlayersReadyBattlet(false);

        MultipleEfect(EF);

    }


    public void MultipleEfect(List<Efecto> EF)
    {
        foreach (Efecto item in EF)
        {
            ActivarEfecto(item);
        }
    }
    Efecto EfectoEnEspera;
    public void ActivarEfecto(Efecto efect)
    {
        ActivarCarta.instance.Activar(efect.OptionCard);
        switch (efect.NameEfecto)
        {
            case EfectosActivos.allyChageAtack:
                MesaManager.instance.GetSlot(MesaManager.Slots.frontSlot).GetComponent<FrontDigimon>().RevelarDigimon(efect.CartaAfecta[0], efect.Ataque);
                break;
            case EfectosActivos.BuffAtack:
                // Envia una carta de tu mano al dark area 
                //lanzamos seleccion de carta 
                if (efect.Jugador == PartidaManager.instance.Player1)
                {
                    if (WhosPlayer._Mano.Cartas.Count > 0)
                    {
                        EfectoEnEspera = efect;
                        SelectedDigimons.instance.Activar(WhaitBuffAtack, StaticRules.instance.WhosPlayer._Mano.Cartas, "choose a Digicard");
                    }
                    else
                    {
                        // no hay cartas en la mano se descarta 
                        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, efect.Jugador).GetComponent<DarkArea>().AddListDescarte(efect.OptionCard, 0.3F, true);
                        Transform slot = MesaManager.instance.GetOptionSlotForCard(efect.OptionCard.DatosDigimon);
                        if (slot != null)
                        {
                            slot.GetComponent<OptionSlot>().Vaciar();
                        }
                    }
                }
                else
                {
                    if (WhosPlayer._Mano.Cartas.Count > 0)
                    {
                        EfectoEnEspera = efect;
                        WhaitBuffAtack(WhosPlayer._Mano.Cartas[0].cardNumber.ToString());
                    }
                    else
                    {
                        // no hay cartas en la mano se descarta 
                        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, efect.Jugador).GetComponent<DarkArea>().AddListDescarte(efect.OptionCard, 0.3F, true);
                        Transform slot = MesaManager.instance.GetOptionSlotForCard(efect.OptionCard.DatosDigimon);
                        if (slot != null)
                        {
                            slot.GetComponent<OptionSlot>().Vaciar();
                        }
                    }
                }
                break;
            case EfectosActivos.EnemyDesDigivolucionar:
                // Envia una carta de tu mano al dark area 
                //lanzamos seleccion de carta 
                if (efect.Jugador == PartidaManager.instance.Player1)
                {
                    if (WhosPlayer._Mano.Cartas.Count > 0)
                    {
                        EfectoEnEspera = efect;
                        SelectedDigimons.instance.Activar(WhaitDesDigivolucionar, StaticRules.instance.WhosPlayer._Mano.Cartas, "choose a DigiCard");
                    }
                }
                else
                {
                    if (WhosPlayer._Mano.Cartas.Count > 0)
                    {
                        EfectoEnEspera = efect;
                        WhaitDesDigivolucionar(WhosPlayer._Mano.Cartas[0].cardNumber.ToString());
                    }
                }
                break;
            case EfectosActivos.doblePower:
                // No se puede usar si tu digimon es nivel 3
                if (WhatNivelMayor(MesaManager.instance.GetDigimonSlot().DatosDigimon.Nivel, "III"))
                {
                    MesaManager.instance.GetSlot(MesaManager.Slots.frontSlot).GetComponent<FrontDigimon>().DuplicatePower(efect.buffo, efect.CartaAfecta[0]);
                }
                else
                {
                    // EL DIGIMON ES NIVEL 3
                }
                break;
            case EfectosActivos.doublelostpoint:
                if (WhatNivelMayor(MesaManager.instance.GetDigimonSlot().DatosDigimon.Nivel, "III"))
                {
                    // descartamos 3 cartas de net ocean del juagador

                    for (int i = 0; i < 3; i++)
                    {
                        CartaDigimon DCard = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, efect.Jugador).GetComponent<NetOcean>().Robar();
                        //Mandamos a la dark area
                        if (DCard)
                        {
                            DCard.Destruir();
                        }
                    }
                    EfectosRonda.Add(efect);
                }
                else
                {
                    // EL DIGIMON ES NIVEL 3
                }
                break;
            case EfectosActivos.dropCards:
                EfectosRonda.Add(efect);
                break;
            case EfectosActivos.lostcardgame:
                break;
            case EfectosActivos.QuitDigimonBox:

                EfectosRonda.Add(efect);
                break;
            case EfectosActivos.SetDarkArea:
                foreach (var item in efect.CartaAfecta)
                {
                    item.Destruir();
                    Transform slot = MesaManager.instance.GetOptionSlotForCard(item.DatosDigimon);
                    if (slot != null)
                    {
                        slot.GetComponent<OptionSlot>().Vaciar();
                    }
                }
                break;
            case EfectosActivos.DiscardHand:
                foreach (var item in efect.CartaAfecta)
                {
                    MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item, 0.3F);
                    Transform slot = MesaManager.instance.GetOptionSlotCard(item);
                    if (slot != null)
                    {
                        slot.GetComponent<OptionSlot>().Vaciar();
                    }
                }
                break;
            case EfectosActivos.IgnorAll:
                // Check card in EvolutionBox Player
                EvolutionBox EB = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox,efect.Jugador).GetComponent<EvolutionBox>();
                if (EB.Cartas.Count > 0)
                {
                    TurnEfect _efec = new TurnEfect();
                    _efec.Efecto = Efectos.SinRequerimientosAll;
                    _efec.Jugador = StaticRules.instance.WhosPlayer;
                    EfectosDeTurno.Add(_efec);
                    break;
                }
                else
                {
                    /// Invocacion especial
                    if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player1)
                    {
                        CartaDigimon DigimonCambatiente = MesaManager.instance.GetDigimonSlot();
                        string NivelRequerido = "";
                        if (DigimonCambatiente.DatosDigimon.Nivel == "III")
                        {
                            NivelRequerido = "IV";
                        }
                        else if (DigimonCambatiente.DatosDigimon.Nivel == "IV")
                        {
                            NivelRequerido = "Perfect";
                        }
                        else if (DigimonCambatiente.DatosDigimon.Nivel == "Perfect")
                        {
                            NivelRequerido = "Ultimate";
                        }
                        List<CartaDigimon> Evoluciones = new List<CartaDigimon>();
                        foreach (var item in PartidaManager.instance.Player1._Mano.Cartas)
                        {
                            if (item.DatosDigimon.Nivel == NivelRequerido)
                                Evoluciones.Add(item);
                        }

                        if (Evoluciones.Count > 0)
                        {
                            // Madamos el panel de seleccion
                            SelectedDigimons.instance.Activar(WhaitCardSt56, Evoluciones, "choose a Digimon");
                            break;
                        }
                        else
                        {
                            // salimos
                            break;
                        }
                    }
                    else
                    {
                        // deside que carta vas a colocar la IA
                        CartaDigimon DigimonCambatiente = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta;
                        string NivelRequerido = "";
                        if (DigimonCambatiente.DatosDigimon.Nivel == "III")
                        {
                            NivelRequerido = "IV";
                        }
                        else if (DigimonCambatiente.DatosDigimon.Nivel == "IV")
                        {
                            NivelRequerido = "Perfect";
                        }
                        else if (DigimonCambatiente.DatosDigimon.Nivel == "Perfect")
                        {
                            NivelRequerido = "Ultimate";
                        }
                        List<CartaDigimon> Evoluciones = new List<CartaDigimon>();
                        foreach (var item in PartidaManager.instance.Player2._Mano.Cartas)
                        {
                            if (item.DatosDigimon.Nivel == NivelRequerido)
                                Evoluciones.Add(item);
                        }

                        if (Evoluciones.Count > 0)
                        {
                            // Madamos el panel de seleccion
                            foreach (var item in Evoluciones)
                            {
                                if (IA.instance.SimularBatalla(item))
                                {
                                    WhaitCardSt56(item.cardNumber.ToString());
                                    break;
                                }
                            }
                        }
                        //salimos si aun no salimos


                    }
                    break;
                }
            case EfectosActivos.Ignor4:
                EfectoEnEspera = efect;
                List<CartaDigimon> ListaCampeones = new List<CartaDigimon>();
                foreach (var item in StaticRules.instance.WhosPlayer._Mano.Cartas)
                {
                    if (item.DatosDigimon.Nivel == "IV")
                    {
                        ListaCampeones.Add(item);
                    }
                }
                if (ListaCampeones.Count > 0)
                {
                    SelectedDigimons.instance.Activar(WhaitEvolutionFour, ListaCampeones, "choose a champion");
                }
                else
                {
                    // El efecto no se puede aplicar por que no hay carta de campeon en la mano
                }
                //   
                break;
        }
    }

    public void WhaitBuffAtack(string ID)
    {
        //descartamos carta 
        foreach (var item in StaticRules.instance.WhosPlayer._Mano.Cartas)
        {
            int DID = Convert.ToInt32(ID);
            if (item.CardNumber == DID)
            {
                SendDarkArea(item.transform, 0.4f);

                // si logramos descartar la carta procedemos con el efecto
                if (EfectoEnEspera != null)
                {
                    MesaManager.instance.GetSlot(MesaManager.Slots.frontSlot).GetComponent<FrontDigimon>().BuffAtack(EfectoEnEspera.buffo, EfectoEnEspera.CartaAfecta[0]);


                    MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, EfectoEnEspera.Jugador).GetComponent<DarkArea>().AddListDescarte(EfectoEnEspera.OptionCard, 0.3F, true);
                    Transform slot = MesaManager.instance.GetOptionSlotForCard(EfectoEnEspera.OptionCard.DatosDigimon);
                    if (slot != null)
                    {
                        slot.GetComponent<OptionSlot>().Vaciar();
                    }
                    EfectoEnEspera = null;
                }
            }
        }
    }
    public void WhaitDesDigivolucionar(string ID)
    {
        //descartamos carta 
        bool Discar = false;
        foreach (var item in StaticRules.instance.WhosPlayer._Mano.Cartas)
        {
            int DID = Convert.ToInt32(ID);
            if (item.CardNumber == DID)
            {
                SendDarkArea(item.transform, 0.4f);
                Discar = true;
                break;
            }
        }
        if (Discar)
        {
            // si logramos descartar la carta procedemos con el efecto
            if (EfectoEnEspera != null)
            {

                // REVIZAR SI ES UN CHAMPION EL DIGIMON CONTRINCANTE 
                if (EfectoEnEspera.Jugador == PartidaManager.instance.Player1)
                {
                    //player2 IA
                    if (MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel == "IV")
                    {
                        MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(null);
                    }

                }
                else
                {
                    // player 1
                    if (MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel == "IV")
                    {
                        MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(null);
                    }

                }
            }
        }
    }
    public void WhaitCardSt56(string ID)
    {
        //descartamos carta 
        Debug.Log(StaticRules.instance.WhosPlayer);
        CartaDigimon Carta = MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>()._Cartas.Find(K => K.cardNumber.ToString() == ID);
        if (Carta)
        {
            Debug.Log(Carta.transform.parent);
            // Revisamos si ya es hijo de la dark area
            if (Carta.transform.parent == MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea))
            {
                // LO MANDAMOS AL  Evolution Box
                MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().
                SetDigimon(MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>()._Cartas.Find(K => K.cardNumber == Convert.ToInt32(ID)).transform, true);
                // si logramos descartar la carta procedemos con el efecto
                TurnEfect _efec = new TurnEfect();
                _efec.Efecto = Efectos.SinRequerimientosAll;
                _efec.Jugador = StaticRules.instance.WhosPlayer;
                EfectosDeTurno.Add(_efec);
                EfectoEnEspera = null;

                if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player2)
                    IA.instance.FinishTurnoIA();
            }
            else
            {
                PartidaManager.instance.RecurisvoEfectoID(WhaitCardSt56, ID);
            }
        }
        else
        {
            //ESPERAR UN SEGUNDO
            PartidaManager.instance.RecurisvoEfectoID(WhaitCardSt56, ID);
        }
    }



    public void WhaitEvolutionFour(string ID)
    {
        //descartamos carta 
        foreach (var item in StaticRules.instance.WhosPlayer._Mano.Cartas)
        {
            int DID = Convert.ToInt32(ID);
            if (item.CardNumber == DID)
            {
                // LO MANDAMOS AL  Evolution Box
                MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().SetDigimon(item.transform, true);
                // si logramos descartar la carta procedemos con el efecto
                if (EfectoEnEspera != null)
                {
                    TurnEfect _efec = new TurnEfect();
                    _efec.Efecto = Efectos.SinRequerimientos4;
                    _efec.Jugador = StaticRules.instance.WhosPlayer;
                    EfectosDeTurno.Add(_efec);
                    EfectoEnEspera = null;
                }
            }
        }
    }
    public void DiscardRamdomCardHand(int cantidad)
    {
        // descartamos carta 
        for (int i = 0; i < cantidad; i++)
        {
            // LO MANDAMOS AL  dark area una carta aleatoria
            if (StaticRules.instance.WhosPlayer._Mano.Cartas.Count >= cantidad)
            {
                SendDarkArea(StaticRules.instance.WhosPlayer._Mano.Cartas[Random.Range(0,
                    StaticRules.instance.WhosPlayer._Mano.Cartas.Count - 1)].transform, 0.4f);
            }
            // no se descarta las cartas que no se tienen 
        }
    }

    public UnityAction<string> _LoAction;
    public int EfectosRond, TurnoEfect = 0;
    public bool SkipEfect = false;

    public void EfectosEstaticos(UnityAction<string> Loaction)
    {
        // Revisa si hay un efecto 
        if (Loaction != null)
            _LoAction = Loaction;

        SkipEfect = false;
        EfectosRond = EfectosRonda.Count;
        Debug.Log("efecto lel");
        if (EfectosRonda.Count != 0)
        {
            if (EfectosRonda[TurnoEfect].Limite == NowPhase)
            {
                // activar Efecto
                if (CartaViable(EfectosRonda[TurnoEfect].OptionCard, EfectosRonda[TurnoEfect].Jugador))
                {

                    ActivarEfectoStatico(EfectosRonda[TurnoEfect], EsperaEfectos);
                }
                else
                {
                    SkipEfect = true;
                    // LA CARTA ESTA FUERA DE JUEGO para este jugador 
                    EsperaEfectos("Esta Fuera de Juego");
                    // no se puede activar 
                }
            }
            else
            {
                SkipEfect = true;
                // LA CARTA ESTA FUERA DE JUEGO para este jugador 
                EsperaEfectos("Esta Carta no se puede activar en esta Fase");
                // no se puede activar 
            }
        }
        else
        {
            _LoAction.Invoke("No hay efectos");
        }
    }
    public void ActivarEfectoStatico(Efecto efect, UnityAction<string> Loaction)
    {
        efect.OptionCard.transform.parent.GetComponent<OptionSlot>().Vaciar();

        switch (efect.NameEfecto)
        {
            case EfectosActivos.QuitDigimonBox:
                efect.OptionCard.transform.parent.GetComponent<OptionSlot>().Vaciar();
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, efect.Jugador).GetComponent<DarkArea>().AddListDescarte(efect.OptionCard, 0.2f);
                break;
            case EfectosActivos.dropCards:
                ActivarCarta.instance.Activar(efect.OptionCard);

                if (efect.Jugador == LostPlayerRound)
                {
                    // Si el Jugador que activo la carta perdio la batalla
                    List<CartaDigimon> Pcartas = new List<CartaDigimon>();

                    for (int i = 0; i < efect.cantidadCartas; i++)
                    {
                        Pcartas.Add(efect.Origen.GetComponent<NetOcean>().Robar());
                    }
                    foreach (var item in Pcartas)
                    {
                        efect.Destino.GetComponent<DarkArea>().SetCard(item.transform);
                    }
                }



                // Descartamos esta carta  
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, efect.Jugador).GetComponent<DarkArea>().AddListDescarte(efect.OptionCard, 0.3f, true);
                Loaction.Invoke("Dropcards");
                break;
            case EfectosActivos.doublelostpoint:
                if (CartaViable(efect.OptionCard, efect.Jugador))
                {
                    ActivarCarta.instance.Activar(efect.OptionCard);
                    if (efect.Jugador == PartidaManager.instance.WinTurno)
                    {
                        // si el jugador que activo la carta Gana la batalla 
                        ReducirPuntos(RondPuntosPerdidos, efect.Jugador, true);
                        // retiramos del juego esta carta para este jugador 
                        RetirarDelJuego(efect.OptionCard, efect.Jugador);
                    }
                }
                // DESCARTAMOS LA CARTA
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, efect.Jugador).GetComponent<DarkArea>().SetCard(efect.OptionCard.transform);
                Loaction.Invoke("Doublelosrtpoint");
                break;
        }
    }

    public Phases ConvertPhases(string fase)
    {
        Phases conver = Phases.PreparationPhase;
        switch (fase)
        {
            case "Battle Phase":
                conver = Phases.BattlePhase;
                break;
            case "Evolution Phase":
                conver = Phases.EvolutionPhase;
                break;
            case "Point Phase":
                conver = Phases.PointCalculationPhase;
                break;
            case "End Phase":
                conver = Phases.EndPhase;
                break;
            default:
                break;
        }
        return conver;
    }
    public Phases ConvertPhases2(string fase)
    {
        Phases conver = Phases.PreparationPhase;
        switch (fase)
        {
            case "Evolution Phase":
                conver = Phases.EvolutionPhase2;
                break;
            case "Battle Phase":
                conver = Phases.OptionBattlePhase;
                break;
            default:
                break;
        }
        return conver;
    }
    public static int ConvertNivel(string Nivel)
    {
        switch (Nivel)
        {
            case "III":
                return 1;
            case "IV":
                return 2;
            case "Perfect":
                return 3;
            case "Ultimate":
                return 4;
        }
        return 0;
    }
    public static string ConvertToNivel(int Nivel)
    {
        switch (Nivel)
        {
            case 1:
                return "III";
            case 2:
                return "IV";
            case 3:
                return "Perfect";
            case 4:
                return "Ultimate";
            default:
                return null;
        }
    }

    public void EsperaEfectos(string activos)
    {
        TurnoEfect++;
        Debug.Log(activos);
        Debug.Log(TurnoEfect + " : " + EfectosRond);
        if (TurnoEfect >= EfectosRond)
        {
            _LoAction.Invoke("Acabo Los efectos");
            return;
        }
        else
        {
            if (!SkipEfect)
            {
                PartidaManager.instance.RecurisvoEfecto(ReEfect);
            }
            else
            {
                ReEfect("Saltamos este efecto");
            }
        }
    }
    public void ReEfect(string result)
    {
        Debug.Log(result);
        EfectosEstaticos(null);
    }

    public bool CartaViable(CartaDigimon OpCard, Player jugador)
    {
        if (jugador == PartidaManager.instance.Player1)
        {
            // cartas del jugador 1
            if (FueraJuego.UNO.Cartasbloqueadas.Count > 0)
            {
                //existe almenos una carta fuera de juego
                if (FueraJuego.UNO.Cartasbloqueadas.Contains(OpCard))
                {
                    // la carta esta bloqueado
                    return false;
                }
            }
        }
        else
        {
            // cartas del jugador 2 
            if (FueraJuego.DOS.Cartasbloqueadas.Count > 0)
            {
                //existe almenos una carta fuera de juego
                if (FueraJuego.DOS.Cartasbloqueadas.Contains(OpCard))
                {
                    // la carta esta bloqueado
                    return false;
                }
            }
        }
        return true;
    }

    public void RetirarDelJuego(CartaDigimon Dcard, Player Jugador)
    {
        if (Jugador == PartidaManager.instance.Player1)
        {
            FueraJuego.UNO.Cartasbloqueadas.Add(Dcard);
        }
        else
        {
            FueraJuego.DOS.Cartasbloqueadas.Add(Dcard);
        }
    }

    public bool Listos = false;

    public void WhaitPlayersReadyBattlet(bool action)
    {
        if (Listos && action)
        {
            DataManager.instance.EndFrame(Salto, 0.2F);
        }

        if (action)
        {
            Listos = true;
        }
        else
            Listos = false;

    }

    public void Salto(string resul)
    {
        SaltoFase(Phases.PointCalculationPhase);
    }

}

