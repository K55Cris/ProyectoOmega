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

    public void TurnoIA(bool espera=false)
    {
        PartidaManager.instance.MenuPhases.ChangePhase(false);
        StaticRules.instance.WhosPlayer = IAPlayer;
        if(espera)
        StartCoroutine(Whaiting(NowPhase,5f));
        else
        StartCoroutine(Whaiting(NowPhase,1f));
    }


    public void NowPhase(string Result)
    {
        switch (StaticRules.NowPhase)
        {
            case DigiCartas.Phases.PreparationPhase:
                PreparationPhase();
                break;
            case DigiCartas.Phases.PreparationPhase2:
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
        // CHECAMOS EL CHILD 
        CheckChangeChild();

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
                            ChangeChild(item,CheckSetEvolutions);
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
        FinishTurnoIA();
    }

    public void ChangeChild(CartaDigimon Child,UnityAction<CartaDigimon> Paso2Preparation)
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

    public void FinishTurnoIA()
    {
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
        StaticRules.SiguienteFase();
        // QUITAR BLOQUEO A PLAYER 1
    }

    public void JugarCarta(CartaDigimon Dcard, Campo Destino)
    {
        Transform Lugar=null;
        switch (Destino)
        {
            case Campo.DigimonBox:
                Lugar = MesaManager.instance.GetSlot(MesaManager.Slots.DigimonSlot,IAPlayer);
                break;
            case Campo.OptionSlot:
                GetOptionSlot(ref Lugar);
                break;
            case Campo.EvolutionSlot:
                Lugar = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionBox, IAPlayer);
                break;
            case Campo.Requeriment:
                Lugar = MesaManager.instance.GetSlot(MesaManager.Slots.EvolutionRequerimentBox, IAPlayer);
                break;
            case Campo.Deck:
                Lugar = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, IAPlayer);
                break;
            case Campo.Netocean:
                Lugar = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, IAPlayer);
                break;
            case Campo.Support:
                Lugar = MesaManager.instance.GetSlot(MesaManager.Slots.SupportBox, IAPlayer);
                break;
            default:
                break;
        }
        if(Lugar)
        PartidaManager.instance.SetMoveCard(Lugar,Dcard.transform, StaticRules.Ajustar);

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
        int OffD1 = StaticRules.instance.WhatAtackUse(Digimon2,D1);
        int OffD2 = StaticRules.instance.WhatAtackUse(Digimon1,TCard);

        Debug.Log(D1.DatosDigimon.Nombre + ":" + OffD1 +"|"+ TCard.DatosDigimon.Nombre + ":" + OffD2);
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

    public  IEnumerator WhaitFase(UnityAction<CartaDigimon> Loaction,CartaDigimon Dcard,float segundos)
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

    public void TerminarTurno()
    {
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
        StaticRules.SiguienteFase();
    }

}
