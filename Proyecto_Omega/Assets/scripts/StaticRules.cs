using UnityEngine;
using UnityEngine.Events;
using DigiCartas;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class StaticRules : MonoBehaviour
{
    public static StaticRules instance = null;
    // Use this for initialization
    public int Turno = 0;
    public Player PlayerFirstAtack;
    public Player WhosPlayer;
    public Player LostPlayerRound;
    public int PointGaugePlayer1 = 100;
    public static bool ProcesoDebuff = false;
    public int PointGaugePlayer2 = 100;
    public static Phases NowPhase;
    public static PreparationPhase NowPreparationPhase;
    public static EvolutionPhase NowEvolutionPhase;
    public List<GameObject> CartasDescartadas = new List<GameObject>();

    public enum Phases { GameSetup = 0, DiscardPhase = 1, PreparationPhase = 2, EvolutionPhase = 3, EvolutionRequirements = 4, FusionRequirements = 5,
        AppearanceRequirements = 6, BattlePhase = 7, OptionBattlePhase = 8, PointCalculationPhase = 9, EndPhase = 10 };
    public enum PreparationPhase
    {
        DiscardPhase = 0, ChangeDigimon = 1, SetEvolition = 2, ActivarOption = 3, SetOptionCard = 4
    };
    public enum EvolutionPhase
    {
        FirstRequeriment = 0, SecondRequerimient = 1
    };
    public enum Efectos
    {
        SinRequerimientos = 0, NADA = 1
    };

    public List<Efectos> EfectosDeTurno = new List<Efectos>();

    public void Start()
    {
        NowPhase = Phases.GameSetup;
        NowEvolutionPhase = 0;
        //Referenciar los puntos de vida con cada jugador o
        //Codificar clase jugador con sus atributos publicos
        PointGaugePlayer1 = 100;
        PointGaugePlayer2 = 100;
        WhosPlayer = PartidaManager.instance.Player1;
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
        StaticRules loRule = FailSafeInstance();

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
        SelectedDigimons.instance.Activar(SetDigimonChildPlayer1, GetDigimonChildInDeck(PartidaManager.instance.Player1.Deck));

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
                SelectedDigimons.instance.Activar(SetDigimonChildPlayer2, GetDigimonChildInDeck(PartidaManager.instance.Player2.Deck));
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
        // barajear mazo player 2
        PartidaManager.Barajear(Deck2);

        //Colocar primera carta del mazo boca abajo en la Point Gauge PLAYER1
        CartaDigimon FirstCarta = MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Robar();
        MesaManager.instance.Campo1.PointGauge.GetComponent<PointGaugeBox>().Dcard = FirstCarta.GetComponent<CartaDigimon>();
        PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.PointGauge, FirstCarta.transform, Ajustar);

        // Sacamos del juego la carta del PointGauge
        PartidaManager.instance.Player1.Deck.cartas.Remove(FirstCarta.GetComponent<CartaDigimon>());

        //Colocar primera carta del mazo boca abajo en la Point Gauge PLAYER2
        CartaDigimon FirstCarta2 = MesaManager.instance.Campo2.NetOcean.GetComponent<NetOcean>().Robar();
        MesaManager.instance.Campo2.PointGauge.GetComponent<PointGaugeBox>().Dcard = FirstCarta2.GetComponent<CartaDigimon>();
        PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo2.PointGauge, FirstCarta2.transform, Ajustar);


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


        // Elegir Primer Jugador
        WhoIsPlayer1.instance.Activar(PartidaManager.instance.Player1, PartidaManager.instance.Player2, StaticRules.instance.WhoFirstPlayer);
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
        loRule.PlayerFirstAtack = jugador;

        //cargar Manos player1
        PartidaManager.instance.cargarManos(PartidaManager.instance.ManoPlayer1, PartidaManager.instance.Player1, MesaManager.instance.Campo1.NetOcean);
        //cargar Manos player 2
        PartidaManager.instance.cargarManos(PartidaManager.instance.ManoPlayer2, PartidaManager.instance.Player2, MesaManager.instance.Campo2.NetOcean);

        SiguienteFase();
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
        string Habilidad = "";
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
        if (Habilidades(Digimon) == "RestoreLife")
            if (player == 1)
            {
                loRule.PointGaugePlayer1 += 0; // aqui va otro metoido para calucular cuanto sube dicha habilidad especial
            }
            else
            {
                loRule.PointGaugePlayer2 += 0; // aqui va otro metoido para calucular cuanto sube dicha habilidad especial
            }
    }
    private static bool WaithPlayer = false;
    public static void WaithPlayers(UnityAction<string> player)
    {
        if (WaithPlayer)
        {
            StaticRules loRule = FailSafeInstance();

            StartDiscardPhase(); // Aca va metodo de siguiente phase yo lo calcule aqui pero se puede hacer por separado

            WaithPlayer = false;
        }
        else
        {
            WaithPlayer = true;
        }
    }

    public static void CheckSetDigiCardSlot(Transform Slot, Transform _Digicarta = null)
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
                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase || StaticRules.NowPhase == StaticRules.Phases.GameSetup)
                        {
                            if (_Carta.Nivel == "III")
                            {
                                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().SetDigimon(_Digicarta.transform);
                            }
                        }
                        break;
                    case "Option Slot 1":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase || StaticRules.NowPhase == StaticRules.Phases.GameSetup)
                        {
                            if (isDigimonOrChip(_Carta))
                            {
                                MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot1).GetComponent<OptionSlot>().SetCard(_Digicarta.transform);
                            }
                        }
                        break;
                    case "Option Slot 2":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase || StaticRules.NowPhase == StaticRules.Phases.GameSetup)
                        {
                            if (isDigimonOrChip(_Carta))
                                MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot2).GetComponent<OptionSlot>().SetCard(_Digicarta.transform);
                        }
                        break;
                    case "Option Slot 3":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase || StaticRules.NowPhase == StaticRules.Phases.GameSetup)
                        {
                            if (isDigimonOrChip(_Carta))
                                MesaManager.instance.GetSlot(MesaManager.Slots.OptionSlot3).GetComponent<OptionSlot>().SetCard(_Digicarta.transform);
                        }
                        break;
                    case "EvolutionBox":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase)
                        {
                            if (!isDigimonOrChip(_Carta))
                                MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().SetDigimon(_Digicarta.transform);
                        }
                        break;

                    case "SupportBox":
                        // Verificar Si se puede colocar la Carta 
                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase)
                        {
                            if (!isDigimonOrChip(_Carta))
                                MesaManager.instance.GetSlot(MesaManager.Slots.SupportBox).GetComponent<SupportBox>().SetDigimon(_Digicarta.transform);
                        }
                        break;
                    case "DarkArea":
                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase && _Digicarta.parent.name.Contains("Option Slot"))
                        {
                            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().SetCard(_Digicarta.transform);
                        }
                        break;
                    case "EvolutionRequerimentBox":

                        if (StaticRules.NowPhase == StaticRules.Phases.PreparationPhase)
                        {
                            if (StaticRules.NowPreparationPhase == StaticRules.PreparationPhase.SetEvolition)
                            {
                                if (StaticRules.NowEvolutionPhase == StaticRules.EvolutionPhase.FirstRequeriment)
                                {
                                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetRequerimientos();
                                }
                                else
                                {
                                    MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().SetAdicionalRequiriment(_Digicarta.transform);
                                }
                            }

                        }
                        break;
                }
            }
        }
    }
    public static void SaltoFase(Phases phase)
    {
        NowPhase = phase;
        Debug.Log(NowPhase);
        switch (NowPhase)
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
            case Phases.EvolutionRequirements:
                CheckEvolutionRequirements();
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

        Debug.Log(NowPhase);
        NowPhase++;
        switch (NowPhase)
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
                PartidaManager.instance.CambioDePhase(false);
                PartidaManager.instance.ActivateHand(false);
                PartidaManager.instance.Cambio("Evolution Phase");
                StartEvolutionPhase();
                break;
            case Phases.EvolutionRequirements:
                CheckEvolutionRequirements();
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

    //INICIO metodos para cada fase

    /// <summary>
    /// Inicial la Game Setup phase
    /// </summary>
    public static void StartGameSetup()
    {
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
        StaticRules.instance.InterTimePhase(2F);
    }

    private static void StartDiscardPhase()
    {
        StaticRules loRule = FailSafeInstance();

    }
    public void AddListDiscard(GameObject Carta, bool addOrRemove)
    {
        StaticRules loRule = FailSafeInstance();
        if (addOrRemove)
        {
            loRule.CartasDescartadas.Add(Carta);
        }
        else
        {
            loRule.CartasDescartadas.Remove(Carta);
        }
    }

    public static bool CheckEvolutionList(CartaDigimon evolucion)
    {
        string nombreDigimon;

        nombreDigimon = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta.GetComponent<CartaDigimon>().DatosDigimon.Nombre.ToUpper();
        nombreDigimon = nombreDigimon.Replace(" ", "");

        foreach (string requerimiento in evolucion.GetComponent<CartaDigimon>().DatosDigimon.ListaRequerimientos)
        {
            Debug.Log(nombreDigimon + ":" + requerimiento.Split(' ')[0].Replace(" ", ""));
            if (nombreDigimon.Equals(requerimiento.Split(' ')[0].Replace(" ", "")))
            {
                return true;
            }
            else if (requerimiento.Split(' ')[1].Equals("+"))
            {
                if (nombreDigimon.Equals(requerimiento.Split(' ')[2]))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static List<string> GetListRequerimentsDigimon(DigiCarta DatosDigimon, DigiCarta BaseDigimon)
    {
        Debug.Log(DatosDigimon.Nombre + ":" + BaseDigimon.Nombre);
        foreach (var item in DatosDigimon.ListaRequerimientos)
        {
            string Requerimiento = item.ToUpper();
            string FiltroNombre = BaseDigimon.Nombre.ToUpper().Replace(" ", "");
            Debug.Log(Requerimiento + ":" + FiltroNombre);
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
    public void Evolucionar()
    {
        int X = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).childCount, j = 0;
        for (int i = 0; i < X; i++)
        {
            if (CheckEvolutionList(MesaManager.instance.Campo1.EvolutionBox.GetChild(j).GetComponent<CartaDigimon>()))
            {
                j = 0;
                i = 0;
                X--;
            }
            else
            {
                j++;
            }
        }
        X = MesaManager.instance.Campo1.EvolutionBox.childCount;
        if (MesaManager.instance.Campo1.EvolutionBox.childCount > 0)
        {
            for (int i = 0; i < X; i++)
            {
                Transform carta = MesaManager.instance.Campo1.EvolutionBox.GetChild(0).GetComponent<Transform>();
                PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.DarkArea, carta, Ajustar);
            }
        }

    }
    /// <summary>
    /// Inicial la Preparation phase
    /// </summary>
    private static void StartPreparationPhase()
    {

        StaticRules loRule = FailSafeInstance();
        Debug.Log("preparationPhase" + loRule.PlayerFirstAtack.Nombre);
        NowPreparationPhase++;

        // DESCARTAMOS LAS CARTAS
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().moviendo = false;
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(StaticRules.StartPreparationPhaseDiscard);
        if (loRule.CartasDescartadas.Count == 0)
            StartPreparationPhaseDiscard("SAlto la Fase sin Descartar");
        foreach (var item in loRule.CartasDescartadas)
        {
            CartaDigimon _CARD = item.GetComponent<CartaDigimon>();
            _CARD.Front.GetComponent<MovimientoCartas>().CanvasSeleted.SetActive(false);
            StaticRules.instance.WhosPlayer._Mano.DescartarCarta(_CARD);
            SendDarkArea(item.transform);
        }

    }
    private static void StartPreparationPhaseDiscard(string result)
    {
        //agregamos a la mano las cartas descartadas
        for (int i = StaticRules.instance.WhosPlayer._Mano.Cartas.Count; i < 6; i++)
        {
            PartidaManager.instance.TomarCarta(PartidaManager.instance.GetHand(), StaticRules.instance.WhosPlayer, MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean));
        }

        //Vaciamos lista de Cartas 
        StaticRules loRule = FailSafeInstance();
        loRule.CartasDescartadas = new List<GameObject>();
        // Habilitamos la Mano
        PartidaManager.instance.ActivateHand(true);
    }

    public List<Transform> ListEvos = new List<Transform>();
    public void Evol(Transform Evolucion, int CantEvos,bool joggres = false)
    {
        Debug.Log(Evolucion.GetComponent<CartaDigimon>().DatosDigimon.Nombre);
        ListEvos.Add(Evolucion);
        if (ListEvos.Count == CantEvos)
        {
            RecursivoEvo(ListEvos[0],joggres);
        }
    }
    public void RecursivoEvo(Transform Evolucion, bool joggres = false)
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evolucionar(Evolucion.GetComponent<CartaDigimon>().DatosDigimon.Nivel,joggres);
        //foreach (var Request in MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().ListaRequerimientos)
        //{);
        Debug.Log(Evolucion.GetComponent<CartaDigimon>().name);

        List<string> requerimientos = StaticRules.GetListRequerimentsDigimon(Evolucion.GetComponent<CartaDigimon>().DatosDigimon,
             MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon);

        int contO = 0;
        int contX = 0;
        int RequestEvo = 0;
        if (requerimientos.Count > 0)
        {
            List<Transform> Dcards = new List<Transform>();
            foreach (var item in requerimientos)
            {
                if (item == "O")
                {
                    // quitar carta delos requisitos 
                    Transform _DigiCarta = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().O;
                    if (_DigiCarta.childCount>0)
                    {
                       _DigiCarta = _DigiCarta.transform.GetChild(contO).transform;
                        contO++;
                        RequestEvo++;
                        SendDarkArea(_DigiCarta);
                    }
                }
                else if (item == ("X"))
                {
                    // quitar carta delos requisitos 
                    Transform _DigiCarta = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().X;
                    if (_DigiCarta.childCount>0 && _DigiCarta)
                    {
                        _DigiCarta = _DigiCarta.transform.GetChild(contX).transform;
                        contX++;
                        RequestEvo++;
                        SendDarkArea(_DigiCarta);
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
                                Dcards.Add(Requeriment);
                            }
                            else
                            {
                                Debug.Log(Requeriment.GetComponent<CartaDigimon>().DatosDigimon.Nombre.ToUpper() + ":" + item + ",XD");
                                string name = Requeriment.GetComponent<CartaDigimon>().DatosDigimon.Nombre;
                                string ShapeName = name.Substring(name.Length - 4, 3);
                                
                                if (ShapeName.Contains(item))
                                {
                                    RequestEvo++;
                                    Dcards.Add(Requeriment);
                                }
                            }   
                        }
                         if (RequestEvo != requerimientos.Count) {
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
            if (RequestEvo == requerimientos.Count)
            {
                foreach (var item in Dcards)
                {
                    SendDarkArea(item);
                }
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().moviendo = false;
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(StaticRules.instance.SetEvolution);
                ListEvos.Remove(Evolucion);
                Invoke("ReEvent",3.5f);
            }
            else
            {
                PartidaManager.instance.CambioDePhase(true);
                ListEvos.Remove(Evolucion);
                MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
                if (joggres)
                {
                    SiguienteFase();
                }
            }
        }
    }
    public void ReEvent()
    {
        if (ListEvos.Count == 0)
        {
            PartidaManager.instance.CambioDePhase(true);
            MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
        }
        else
        {
            MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
            RecursivoEvo(ListEvos[0],false);
        }
    }
    public static void SecondEvolutionPhase()
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().TerminarEvolucionar();
        // Tirar las cartas sobrantes a la dark area
        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().EndEvolution();
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().moviendo = false;
        //> primero Requisitos
        foreach (var item in MesaManager.instance.GetSlot(MesaManager.
            Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().ListaXO)
        {
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item.GetComponent<CartaDigimon>(), 0.5f);
        }
        foreach (var item2 in MesaManager.instance.GetSlot(MesaManager.
           Slots.EvolutionRequerimentBox).GetComponent<EvolutionRequerimentBox>().ListaRequerimientosAdicionales)
        {
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item2.GetComponent<CartaDigimon>(), 0.5f);
        }
        foreach (var item3 in MesaManager.instance.GetSlot(MesaManager.
           Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas)
        {
            Debug.Log(item3.GetComponent<CartaDigimon>().DatosDigimon.Nombre);
            MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item3.GetComponent<CartaDigimon>(), 0.5f);
        }
        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas = new List<CartaDigimon>();
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
                StaticRules.instance.Evol(Evolucion.transform, MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas.Count);
            }

        }
        else
        {
            PartidaManager.instance.CambioDePhase(true);
        }


        /*
         El jugador que resultó primer atacante será quien complete su Evolution Phase antes de que 
         el segundo jugador comience la suya, antes de comenzar la siguiente fase.

        1. El jugador que resultó primer atacante procederá a realizar el cambio de su 
        Digimon Nivel III que anunció en su fase anterior. El segundo jugador
        puede decidir si continuar o no con su cambio de Digimon Nivel III a partir
        de las decisiones del primer jugador.

        2. El jugador que resultó primer atacante procederá a realizar las evoluciones
        regulares que se anunciaron y prepararon en la fase anterior. El segundo
        jugador puede decidir si continuar o no con sus evoluciones anunciadas en
        la fase anterior.

        3. Las cartas utilizadas para los requisitos de evolución colocadas en la
        Requirement Evolution Box serán enviadas al Dark Area.

        4. Después de la evolución, las cartas que se encontraban antes de la evolución
        serán enviadas al Dark Area exceptuando el Digimon Nivel III. El jugador
        solo debe poseer su evolución y su Digimon Nivel III en la Digimon Box.

        5. Se puede activar cualquier habilidad u Option Card que requiera ser
        activada durante la Evolution Phase. Se pueden activar a lo largo de toda la
        fase en cualquier momento.

        6. Se activará cualquier habilidad que se active inmediatamente una vez que
        se cumplan los requisitos.

        7. Una vez que ambos jugadores hayan realizado sus respectivas evoluciones,
        todas las cartas que se encuentran en la Evolution Box y la Evolution
        Requirement Box que no fueron utilizadas, serán enviadas al Dark Area.
         */
        /*      int X = MesaManager.instance.Campo1.EvolutionBox.childCount, j = 0;
              for (int i = 0; i < X; i++)
              {
                  if (CheckEvolutionList(MesaManager.instance.Campo1.EvolutionBox.GetChild(j).GetComponent<CartaDigimon>()))
                  {
                      j = 0;
                      i = 0;
                      X--;
                  }
                  else
                  {
                      j++;
                  }
              }
              X = MesaManager.instance.Campo1.EvolutionBox.childCount;
              if (MesaManager.instance.Campo1.EvolutionBox.childCount > 0)
              {
                  for (int i = 0; i < X; i++)
                  {
                      Transform carta = MesaManager.instance.Campo1.EvolutionBox.GetChild(0).GetComponent<Transform>();
                      carta.transform.parent = MesaManager.instance.Campo1.DarkArea;
                      carta.GetComponent<CartaDigimon>().AjustarSlot();
                  }
              }

              */
    }
    public void SetEvolution(string tiempo)
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>().Evolution(
        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas[0].transform);
        MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().NowPhase();

    }


    public static void SendDarkArea(Transform Dcard)
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(Dcard.GetComponent<CartaDigimon>(), 0.3f);
    }
    private static void CheckEvolutionRequirements()
    {

        // revisa si la evolucion no se llevo adecuadamente ya sea por efectos especiales
        if (MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas.Count > 0)
        {
            // HAY EVO POSIBLE  
            // Realizar segundo intento de Evolucion correspondiente 
            foreach (var Evolucion in MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox).GetComponent<EvolutionBox>().Cartas)
            {
                StaticRules.instance.Evol(Evolucion.transform, 1,true);
            }

          // StaticRules.instance.EfectosDeTurno.Add(Efectos.SinRequerimientos);

            foreach (var item in StaticRules.instance.EfectosDeTurno)
            {
                if (item == Efectos.SinRequerimientos)
                {
                    // evolucionar como si nada 
                    MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().setAction(StaticRules.instance.SetEvolution);

                }
            }
        }
        else
        {
            SiguienteFase();
        }
    }
    
    private static void CheckFusionRequirements()
    {
        /*
        Los Digimon que evolucionan mediante una Armor Evolution o que
        requieran ciertas Option Card para la evolución, evolucionaran utilizando
        los Fusion Requirements.

        Si “No se puede fusionar desde el atributo de virus Digimon”, o requisitos
        similares, se enumeran en los Fusion Requirements del Digimon se aplica
        a todos los Digimon que se utilizaran para la fusión.

        Si “Al final del turno durante el cual se realizó la fusión” o requisitos
        similares se enumeran en los Fusion Requirements del Digimon, se
        aplicaran aunque se hubiera invocado la carta directamente de la mano por
        medio de una Option Card.18

        No se puede evolucionar a un Digimon que evolucione utilizando
        Evolution / Appearance Requirements cuando se usa un efecto o habilidad
        que diga “Ignora los Fusion Requirements”
         */

        // revisa si existe una carta en evolution box aun , y si existe , revisa si esta es una fusion
        // realiza apartado de fusion

        SiguienteFase();
    }

    private static void CheckAppearanceRequirements()
    {
        /*
        Los digimon que aparecen mediante “Requerimientos de Aparición”
        aparecerán en tu Digimon Box cuando esos requerimientos sean
        completados.

        Si “Durante la batalla” es puesto en los Requerimientos de Aparición del
        Digimon, este aparecerá durante la Battle Phase, durante el momento en el
        que las habilidades especiales son activadas, con el segundo jugador para
        atacar yendo primero (Si el jugador que está primero para atacar está
        haciendo una aparición de un Digimon mediante Requerimientos de
        Aparición, lo hará luego de que el segundo jugador en atacar haya
        activado todas sus habilidades especiales.)

        Si ambos jugadores han completado sus Requerimientos de Aparición, el
        segundo jugador en atacar mandara su aparición primero.

        Si “Al comienzo de la batalla” es puesto en los Requerimientos de
        Aparición de tu Digimon, este no aparecerá hasta que todas las
        habilidades especiales que pongan “Al comienzo de la batalla” se hayan
        activado.

        Si “Al comienzo de la batalla” es puesto en los Requerimientos de
        Aparición de tu Digimon, y este tiene una habilidad especial que dice
        “Cuando esta carta hace aparición”, activa esa habilidad especial antes de
        activar otras habilidades especiales que dicen “Al comienzo de la batalla”
        o “Durante la batalla” (La segunda persona en atacar activa las suyas
        primero”).19

        No podras evolucionar en un Digimon que evoluciona usando Evolution /
        Fusion Requirements cuando se está usando un efecto o habilidad que
        indique “Ignora los Requerimientos de Aparición”
         */

        // descarta las cartas usadas y pasamos al la evolucion del rival
        StaticRules.instance.ListEvos = new List<Transform>();
        StaticRules.SecondEvolutionPhase();
        SiguienteFase();
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

    /// <summary>
    /// Inicial la Battle phase
    /// </summary>
    private static void StartBattlePhase()
    {
        Debug.Log("Hora de Pelear");
        DigimonBoxSlot DigimonBox1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();
        DigimonBoxSlot DigimonBox2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>();
        // saltarme fase del enemigo de evolution

        DigimonBox2._DigiCarta.Volteo();

        //inisializamos la batalla 
        DigimonBox1.BattelPhase(StaticRules.instance.WhatAtackUse(DigimonBox2._DigiCarta.DatosDigimon.TipoBatalla, 
            DigimonBox1._DigiCarta),StaticRules.instance.StartBattlePhase2);

        DigimonBox2.BattelPhase(StaticRules.instance.WhatAtackUse(DigimonBox1._DigiCarta.DatosDigimon.TipoBatalla,
            DigimonBox2._DigiCarta),null);
        /*

        
        1. Empezando con el segundo jugador en atacar, si cualquier jugador ha
        completado los Requerimientos de Aparición de cualquier Digimon con
        “Al comienzo de la batalla” indicado en su Requerimiento de Aparición,
        tendrán que hacer aparecer al Digimon ahora. Si ese Digimon tiene una
        habilidad especial que indica “Cuando esta carta hace aparición”, tendrá
        que ser activado inmediatamente después de que la carta hace su aparición
        (la segunda persona en atacar activará su habilidad primero, antes que la
        primera persona en atacar haga que su Digimon aparezca).
        
        2. Empezando con el segundo jugador en atacar, ambos jugadores usaran
        cualquier efecto o habilidad que indique “Al comienzo de la batalla”.
        
        3. Empezando con el segundo jugador en atacar, ambos jugadores revisaran
        el Battle Type de cada uno. Todas las cartas Digimon tienen un Battle Type
        A, B, o C, esta es mostrada por la letra que está en la esquina superior
        izquierda de cada carta. Esta letra determina el tipo de ataque que el
        oponente hará contra ti - por ejemplo, si el Battle Type de tu Digimon es A,
        el oponente usará su ataque A. Luego de revisar su Battle Type, cada
        jugador determina su Attack Power, basado en la cantidad de Attack Power
        escrito en los ataques A, B o C en sus cartas. Si estás usando tu ataque C
        con efecto (A→0), y el oponente está usando su ataque A, el Attack Power
        del ataque A de tu oponente se convertirá en cero.
        
        4. Empezando con el segundo jugador en atacar, ambos usaran cualquier
        Option Card / habilidad que requiera ser usada durante la Battle Phase. Esto
        incluye habilidades o Digimon que usen “Durante la batalla” en sus20
        Requerimientos de Aparición. El primer jugador en atacar comenzará
        después de que el segundo jugador en atacar haya finalizado con la
        activación de todos sus efectos/habilidades y haga aparecer a su Digimon.
        
        5. Cuando ambos jugadores hayan terminado de usar sus Option Cards /
        habilidades, determinan su Attack Power final. Aumentos/disminuciones
        del Attack Power se acumularan; sin embargo las multiplicaciones en el
        Attack Power no se acumularan, y solo la última multiplicación activada se
        aplicará. El jugador con mayor Attack Power gana la batalla; si ambos
        tienen el mismo Attack Power, la batalla termina en empate.
         */

    }
    
    private void StartBattlePhase2(string result)
    {

        if (!IsInvoking("StartBattlePhase2"))
        {
            DigimonBoxSlot DigimonBox1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();
            DigimonBoxSlot DigimonBox2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>();
            // Activar habilidad de Digimon de jugador 2
            // Activar habilidad de Digimon de jugador 1
            // revisar si ataco con C para efecto de Digimon jugador 1
            if(PlayerFirstAtack==PartidaManager.instance.Player1)
                StaticRules.instance.EfectosDeAtaque(DigimonBox1._DigiCarta, DigimonBox2._DigiCarta, StaticRules.instance.StartBattlePhase3);
            else
            {
                StaticRules.instance.EfectosDeAtaque(DigimonBox2._DigiCarta, DigimonBox1._DigiCarta, StaticRules.instance.StartBattlePhase3);

            }
            // revisar si ataco con C para efecto de Digimon jugador 2   
        }
    }

    private void StartBattlePhase3(string result)
    {
        Debug.Log(result);
            DigimonBoxSlot DigimonBox1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>();
            DigimonBoxSlot DigimonBox2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>();
           
        if (PlayerFirstAtack == PartidaManager.instance.Player1)
            StaticRules.instance.EfectosDeAtaque(DigimonBox2._DigiCarta, DigimonBox1._DigiCarta, StaticRules.instance.StartBattlePhase4);
        else
        {
            StaticRules.instance.EfectosDeAtaque(DigimonBox1._DigiCarta, DigimonBox2._DigiCarta, StaticRules.instance.StartBattlePhase4);

        }
    }
    private void StartBattlePhase4(string result)
    {
        Debug.Log(result);

        // Jugador Dos ativa su carta y termina el Turno 

        PartidaManager.instance.CambioDePhase(true);
        // pasar turno si la Ia es el jugador 2
        if (StaticRules.instance.WhosPlayer == PartidaManager.instance.Player2)
        {
            SiguienteFase();
        }
        else
        {
            // Damos espacio a que el jugaro 1 ative sus cartas option y pase el turno 
        }
    }
    private static void StartBattlePhase5(string result)
    {

        if (StaticRules.instance.WhosPlayer != PartidaManager.instance.Player2)
        {
            // salto de phase por parte de la IA
            SiguienteFase();
        }
        else
        {
            // Damos espacio a que el jugaro 1 ative sus cartas option y pase el turno 
        }
    }


    public void EfectosDeAtaque(CartaDigimon DigiCartaAfectada, CartaDigimon DigimonContrario, UnityAction<string> Phase)
    {
        if (DigimonContrario.DatosDigimon.TipoBatalla == "C") {
            
            if (DigiCartaAfectada.DatosDigimon.NombreAtaqueC.Contains("Guard (A→0)"))
            {
                    Guard("A", DigiCartaAfectada.DatosDigimon.TipoBatalla, 0, DigimonContrario,Phase);
            }
        }
        else
        {
            Phase.Invoke("Tipo de Batalla no concuerda con la Habilidad");
        }
    }
    public void Guard(string Ataque, string TipoAtaquePropio,int cantidadresultante, CartaDigimon CartaTarget, UnityAction<string> Phase)
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
        
        NowPhase = 0;
        NowEvolutionPhase = 0;
        NowPreparationPhase = 0;
        
        MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().EndPhase();
        MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().EndPhase();

        MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().Cambiado = false;
        MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().Cambiado = false;


        // revisamos si el deck esta vacio 
        Debug.Log(MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Cartas.Count);
        if (MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Cartas.Count == 0)
        {
            MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().Reiniciar(StaticRules.instance.WhaitReinicio);
          
           // StaticRules.instance.InterTimePhase(2F);
        }
        else
        {
            SaltoFase(Phases.GameSetup);
           // StaticRules.instance.InterTimePhase(0.5F);
        }  
    }

    public void WhaitReinicio(string resul)
    {
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

	/// <summary>
    /// Calcula el daño recibido en cada combate
    /// </summary>
    public static void CalcularPuntosPerdidos()
    {
        float Digimon1 = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().CanvasContador.PoderDeAtaque;
        float Digimon2 = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().CanvasContador.PoderDeAtaque;

        if (Digimon1 > Digimon2)
        {
            //Gano Digimon 1
            // nivel de tu digimon
            string nivel = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel;
            // Obtenemos puntos que vamos a reducir al jugador 2
            int PuntosPerdidos = PerdidaPuntos(MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta, nivel);
            // descontamos los puntos
            StaticRules.instance.ReducirPuntos(PuntosPerdidos, true);
            if (!PartidaManager.instance.Player1==StaticRules.instance.PlayerFirstAtack)
                ChangeFirstAtackPlayer();

        }
        else if(Digimon2> Digimon1)
        {
            //Gano Digimon 2
            // nivel de tu digimon
            string nivel = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.Nivel;
            // Obtenemos puntos que vamos a reducir al jugador 2
            int PuntosPerdidos = PerdidaPuntos(MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta, nivel);
            // descontamos los puntos
            StaticRules.instance.ReducirPuntos(PuntosPerdidos,false);

            if (!PartidaManager.instance.Player2 == StaticRules.instance.PlayerFirstAtack)
                ChangeFirstAtackPlayer();
        }
        else if(Digimon1==Digimon2)
        {
            // Empate Restamos a ambos 10 puntos
            StaticRules.instance.ReducirPuntos(10,true,true);
            StaticRules.instance.ReducirPuntos(10, false);
        }
        /*
        Fijándose en los Lost Points que dice su Digimon, y
        sustrayendo los puntos perdidos correspondientes al nivel del Digimon
        oponente, moviendo la carta del Point Gauge abajo. Si la batalla terminó en
        empate, ambos jugadores pierden 10 puntos, y sus Digimon permanecen
        como están.
         */
    }

    public void ReducirPuntos(int Cantidad, bool player1, bool guardia=false)
    {
        Debug.Log(Cantidad + " :" + player1);
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
        if (!guardia)
        {
            Invoke("FinalPointPhase", 2f);
        }
    }

    public void FinalPointPhase()
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().moviendo = false;
        Debug.Log("End Phase");
        if (LostPlayerRound == PartidaManager.instance.Player1)
        {
            MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(StaticRules.StartEndPhase);
        }
        else
        {
            MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(StaticRules.StartEndPhase);
        }
        //StaticRules.instance.InterTimePhase(1);
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
        if (StaticRules.instance.PointGaugePlayer1 <= 0&& StaticRules.instance.PointGaugePlayer2 > 0)
        {
            PartidaManager.instance.Cambio(PartidaManager.instance.Player2.Nombre+" a Ganado");
            CartaDigimon Digimon = MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().DRoquin;
            PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.DarkArea,Digimon.transform, Ajustar);
        }
        else if(StaticRules.instance.PointGaugePlayer2 > 0 && StaticRules.instance.PointGaugePlayer1 >= 0)
        {
            PartidaManager.instance.Cambio(PartidaManager.instance.Player1.Nombre + " a Ganado");
            CartaDigimon Digimon = MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>().DRoquin;
            PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo2.DarkArea, Digimon.transform, Ajustar);
        }
        else
        {
            PartidaManager.instance.Cambio("Empate");
        }
        PartidaManager.instance.cambioEcena();
    }
}

