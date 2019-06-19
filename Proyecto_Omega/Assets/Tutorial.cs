using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using DigiCartas;
public class Tutorial : MonoBehaviour {

    public TextMeshProUGUI Descripcion, Titulo, Dialogo;
    public GameObject Panel;
    public Button Nextdialog;
    public Transform OriginalPos;
    public CanvasGroup PanelTutorial;
    public CanvasGroup PanelTutorialDialog;
    public GameObject TargetCamera;
    public GameObject CanvasListo;
    public ListaTutorialText LtexCampos;
    public GameObject CartaPrefab;
    public ListaTutorialText LtexFases;
    public ListaTutorialText LteEvolucion;

    public static Tutorial instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
            Destroy(gameObject);
    }

    public int TargetID = 0;
    public enum TutoStates
    {
        Campos = 0, Fases = 1, Evolucion, Efectos
    };
    public enum DigiEfectos
    {
        None = 0, CartasCompletas=1
    };

    public bool WhaitAction;

    public TutoStates NowSelectTuto;
    public DigiEfectos NowDigiEfectos;

    public void Start()
    {
        // Musica de Duelo
        SoundManager.instance.PlayMusic(Sound.tutorial);
        // Poner las Reglas 
        StaticRules.FailSafeInstance();
    }


    public void CamposOver()
    {
        Descripcion.text = "Aprende el funcionamiento de cada espacio y su " +
            "relación con  el tablero y partida...";
        Panel.gameObject.SetActive(true);

    }
    public void FasesOver()
    {
        Descripcion.text = "Cada fase cambia el funcionamiento de los campos," +
            "aprende todo sobre qué hacer en cada una de ellas.";
        Panel.gameObject.SetActive(true);
    }
    public void EvolucionOver()
    {
        Descripcion.text = "La evolución es fundamental para ganar ," +
            "aprende a como lograr la una evolución perfecta y múltiples añadidos de ella.";
        Panel.gameObject.SetActive(true);
    }
    public void EfectosOver()
    {
        Descripcion.text = "Los efectos te ayudaran a dar la vuelta a una partida ,aprende a cómo y cuándo usarlos  y además a contrarrestarlos.";
        Panel.gameObject.SetActive(true);
    }
    public void Campos()
    {
        // Cargar Tutorial de Campos
        DataManager.instance.FadeCanvas(PanelTutorial);
        NowSelectTuto = TutoStates.Campos;
        Invoke("Iniciar", .6f);
    }
    public void Fases()
    {
        // Cargar Tutorial de Campos
        DataManager.instance.FadeCanvas(PanelTutorial);
        StaticRules.SelectDigimonChild();
        NowSelectTuto = TutoStates.Fases;
        Invoke("Iniciar", .6f);
    }
    public void Evolucion()
    {
        // Cargar Tutorial de Campos
        DataManager.instance.FadeCanvas(PanelTutorial);
        NowSelectTuto = TutoStates.Evolucion;
        Invoke("Iniciar", .6f);
    }
    public void Efectos()
    {
        // Cargar Tutorial de Campos
        DataManager.instance.FadeCanvas(PanelTutorial);
    }

    public void Back()
    {
        LevelLoader.instance.CargarEscena("Main Menu");
    }

    public void Iniciar()
    {
        Seguir(NowSelectTuto);
    }

    public void Seguir(TutoStates Apartado)
    {
        // Mover Camara si lo presisa y siguiente cuadro
        switch (Apartado)
        {
            case TutoStates.Campos:
                if (TargetID >= LtexCampos.LPos.Count)
                    StartCoroutine(Transicion(LtexCampos.LPos[LtexCampos.LPos.Count - 1].transform, TargetID, LtexCampos.ListaCompleta, LectorTexto));
                else
                {
                    StartCoroutine(Transicion(LtexCampos.LPos[TargetID].transform, TargetID, LtexCampos.ListaCompleta, LectorTexto));
                }
                break;
            case TutoStates.Fases:
                if (TargetID < LtexFases.ListaCompleta.Count)
                {
                    if (LtexFases.ListaCompleta[TargetID].Dialogos[Contador].Tipo != TypeTutorial.Action)
                    {
                        // Seguir dialogo
                        if (TargetID >= LtexFases.LPos.Count)
                            StartCoroutine(Transicion(LtexFases.LPos[LtexFases.LPos.Count - 1].transform, TargetID, LtexFases.ListaCompleta, LectorTexto));
                        else
                        {
                            StartCoroutine(Transicion(LtexFases.LPos[TargetID].transform, TargetID, LtexFases.ListaCompleta, LectorTexto));
                        }
                    }
                    else
                    {
                        // Espera Activador
                        DataManager.instance.FadeCanvas(PanelTutorialDialog);
                    }
                }
                else
                {
                    StartCoroutine(Transicion(LtexFases.LPos[26].transform, 26, LtexFases.ListaCompleta, LectorTexto));
                }
                break;
            case TutoStates.Evolucion:
                if (TargetID < LteEvolucion.ListaCompleta.Count)
                {
                    if (LteEvolucion.ListaCompleta[TargetID].Dialogos[Contador].Tipo != TypeTutorial.Action)
                    {
                        // Seguir dialogo
                        if (TargetID >= LteEvolucion.LPos.Count)
                            StartCoroutine(Transicion(LteEvolucion.LPos[LteEvolucion.LPos.Count - 1].transform, TargetID, LteEvolucion.ListaCompleta, LectorTexto));
                        else
                        {
                            StartCoroutine(Transicion(LteEvolucion.LPos[TargetID].transform, TargetID, LteEvolucion.ListaCompleta, LectorTexto));
                        }
                    }
                    else
                    {
                        // Espera Activador
                        DataManager.instance.FadeCanvas(PanelTutorialDialog);
                    }
                }
                else
                {
                    //   StartCoroutine(Transicion(LteEvolucion.LPos[26].transform, 26, LteEvolucion.ListaCompleta, LectorTexto));
                }
                break;
            case TutoStates.Efectos:
                break;
            default:
                break;
        }
    }

    public IEnumerator Transicion(Transform Destino, int ID, List<TextosTutorial> lista, UnityAction<int, List<TextosTutorial>> LoAction)
    {
        yield return new WaitForEndOfFrame();
        if (Destino)
        {
            var heading = Destino.position - TargetCamera.transform.position;
            var distance = heading.magnitude;
            while (distance > 5)
            {
                heading = Destino.position - TargetCamera.transform.position;
                distance = heading.magnitude;
                //var direction = heading / distance; // This is now the normalized direction.
                //transform.parent.transform.Translate(direction*Time.deltaTime*30);
                float step = 500 * Time.deltaTime;
                TargetCamera.transform.position = Vector3.MoveTowards(TargetCamera.transform.position, Destino.position, step);
                yield return new WaitForSecondsRealtime(0.01F);
            }
        }

        yield return new WaitForSecondsRealtime(0.5F);
        LoAction.Invoke(ID, lista);

    }

    public IEnumerator ReOriginalPos()
    {
        yield return new WaitForEndOfFrame();
        var heading = OriginalPos.position - TargetCamera.transform.position;
        var distance = heading.magnitude;
        while (distance > 5)
        {
            heading = OriginalPos.position - TargetCamera.transform.position;
            distance = heading.magnitude;
            //var direction = heading / distance; // This is now the normalized direction.
            //transform.parent.transform.Translate(direction*Time.deltaTime*30);
            float step = 500 * Time.deltaTime;
            TargetCamera.transform.position = Vector3.MoveTowards(TargetCamera.transform.position, OriginalPos.position, step);
            yield return new WaitForSecondsRealtime(0.01F);
        }
    }


    public void LectorTexto(int ID, List<TextosTutorial> ListaTexto)
    {
        TextosTutorial Datos = new TextosTutorial();
        if (ID != ListaTexto.Count)
        {
            Datos = ListaTexto[ID];
            Iniciar(Datos);
        }
        else
        {
            if (NowSelectTuto == TutoStates.Fases)
            {
                endPhases();
            }
            else
            {
                TargetID = 0;
                DataManager.instance.FadeCanvas(PanelTutorialDialog);
                DataManager.instance.FadeCanvas(PanelTutorial, true);
                StartCoroutine(ReOriginalPos());
            }
        }

    }

    public TextosTutorial NowData;
    public int Contador = 0;

    public void Iniciar(TextosTutorial Datos)
    {
        NowData = Datos;
        Titulo.text = Datos.Subtitulo;
        Dialogo.text = Datos.Dialogos[0].Dialogo;
        TargetID++;
        Contador++;
        DataManager.instance.FadeCanvas(PanelTutorialDialog, true);
    }

    public void NextDialogo()
    {
        StartCoroutine(WhaitTimeFrame());
        if (NowData.Dialogos.Count > Contador)
        {
            if (NowData.Dialogos[Contador].Tipo == TypeTutorial.Action)
            {
                // ESPERA Activador
                DataManager.instance.FadeCanvas(PanelTutorialDialog, false);
                Contador = 0;
                switch (NowSelectTuto)
                {
                    case TutoStates.Fases:
                        Secuenciafases();
                        break;
                    case TutoStates.Evolucion:
                        SecuenciaEvolucion();
                        break;
                    case TutoStates.Efectos:
                        break;
                    default:
                        break;
                }

            }
            else
            {
                Dialogo.text = NowData.Dialogos[Contador].Dialogo;
                Contador++;
            }
        }
        else
        {
            Contador = 0;
            NowData = new TextosTutorial();
            DataManager.instance.FadeCanvas(PanelTutorialDialog, false);
            Seguir(NowSelectTuto);
        }
    }


    public void SelectDigimonChild()
    {
        StaticRules.SelectDigimonChild();
    }
    public int FasesID = 0;
    public void Secuenciafases()
    {
        switch (FasesID)
        {
            case 0:
                // SelectChild
                StaticRules.SelectDigimonChild();
                break;
            case 1:
                // Selec Player 1
                WhoIsPlayer1.instance.Activar(PartidaManager.instance.Player1, PartidaManager.instance.Player2, StaticRules.instance.WhoFirstPlayer);
                break;
            case 2:
                // Cambio de Fase a Discard Phase
                StaticRules.SiguienteFase();
                break;
            case 3:
                // Activamos boton de Listo
                CanvasListo.SetActive(true);
                // pasamos a siguiente dialogo
                Iniciar();
                break;
            case 4:
                // Creamos una Carta 
                GameObject DigiCarta = Instantiate(CartaPrefab, MesaManager.instance.Campo1.OptionSlot1);
                DigiCarta.GetComponent<CartaDigimon>().AjustarSlot();
                DigiCarta.GetComponent<CartaDigimon>().cardNumber = 31;
                DigiCarta.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(60);
                DigiCarta.GetComponent<CartaDigimon>().Mostrar();
                MesaManager.instance.Campo1.OptionSlot1.GetComponent<OptionSlot>().OpCarta = DigiCarta.GetComponent<CartaDigimon>();
                MesaManager.instance.Campo1.OptionSlot1.GetComponent<OptionSlot>().Vacio = false;
                break;
            case 5:
                // Creamos una Child


                //Mandamos al dark area toda la mano 
                foreach (var item in PartidaManager.instance.Player1._Mano.Cartas)
                {
                    MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player1).GetComponent<DarkArea>().AddListDescarte(item, 0.3f);

                }

                GameObject Carta = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                Carta.GetComponent<CartaDigimon>().cardNumber = 32;
                Carta.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(1);
                PartidaManager.instance.Player1._Mano.RecibirCarta(Carta.GetComponent<CartaDigimon>(), true);
                Carta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = true;
                StartCoroutine(WhaitFrame(Carta));
                break;
            case 6:
                // creamos un chip
                break;
            case 7:
                // Activamos boton de Listo
                GameObject OpCarta = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                OpCarta.GetComponent<CartaDigimon>().cardNumber = 33;
                OpCarta.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(50);
                PartidaManager.instance.Player1._Mano.RecibirCarta(OpCarta.GetComponent<CartaDigimon>(), true);
                OpCarta.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = true;
                StartCoroutine(WhaitFrame(OpCarta));
                break;
            // pasamos a siguiente dialogo
            case 8:
                CanvasListo.SetActive(true);
                // pasamos a siguiente dialogo
                break;
            case 9:
                StaticRules.SiguienteFase();
                Iniciar();
                CanvasListo.SetActive(false);
                break;
            case 10:
                // nada 
                break;
            case 11:
                CanvasListo.SetActive(true);
                break;
            case 12:
                StaticRules.SiguienteFase();

                break;

        }
        FasesID++;
    }
    public void SecuenciaEvolucion()
    {
        switch (FasesID)
        {
            case 0:
                // poner cartas X , O
                GameObject CartaX = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                CartaX.GetComponent<CartaDigimon>().cardNumber = 55;
                CartaX.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(1);
                PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().X, CartaX.transform, StaticRules.Ajustar);

                GameObject CartaO = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                CartaX.GetComponent<CartaDigimon>().cardNumber = 56;
                CartaX.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(2);
                PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().O, CartaO.transform, StaticRules.Ajustar);
                Iniciar();
                break;
            case 1:
                // Quitar Cartas  X , O

                Transform DCartaO = MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().O.GetChild(0);
                Transform DCartaX = MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().X.GetChild(0);

                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player1).GetComponent<DarkArea>().AddListDescarte(DCartaO.GetComponent<CartaDigimon>(), 0.2f);
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea, PartidaManager.instance.Player1).GetComponent<DarkArea>().AddListDescarte(DCartaX.GetComponent<CartaDigimon>(), 0.2f);
                Iniciar();
                break;
            case 2:
     /////////  poner carta de evolucion  y roquin , ademas de poner fase preparation

                // estableser todo el duelo en preparation , meter roquin 
                StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
                StaticRules.instance.NowPhase = Phases.PreparationPhase;

                GameObject Carta = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                CartaDigimon DChild = Carta.GetComponent<CartaDigimon>();
                DChild.cardNumber = 32;
                DChild.DatosDigimon = DataManager.instance.GetDigicarta(1);
                DChild.Front.GetComponent<MovimientoCartas>().Mover = true;
                DChild.Front.GetComponent<MovimientoCartas>().Mover = true;
                DChild.Mostrar();
                PartidaManager.instance.SetMoveCard(MesaManager.instance.Campo1.DigimonSlot, DChild.transform, StaticRules.Ajustar);
                MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta = DChild;
                MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().DRoquin = DChild;

                GameObject CartaAdult = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                CartaAdult.GetComponent<CartaDigimon>().cardNumber = 32;
                CartaAdult.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(6);
                PartidaManager.instance.Player1._Mano.RecibirCarta(CartaAdult.GetComponent<CartaDigimon>(), true);
                CartaAdult.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = true;
                StartCoroutine(WhaitFrame(CartaAdult));
                break;
            case 3:
                // vaciamos mano 
                PartidaManager.instance.Player1.DeleteAllHand();
                
                //Agregamos al Greymon 

                GameObject Greymon = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                Greymon.GetComponent<CartaDigimon>().cardNumber = 33;
                Greymon.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(2);
                PartidaManager.instance.Player1._Mano.RecibirCarta(Greymon.GetComponent<CartaDigimon>(), true);
                Greymon.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = true;
                StartCoroutine(WhaitFrame(Greymon));

                // AGREGAMIS CARTAS RANDOM DE SACRIFICIO

                break;
            case 4:
                //
                // ACTIVAR  panel de colocar SIGUIENTE FASE
                MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().addDarkArea("Tutorial");
     




                break;
            case 5:
                //
                StaticRules.instance.NowPhase = Phases.PreparationPhase2;
                CanvasListo.SetActive(true);
                StaticRules.instance.PlayerFirstAtack = PartidaManager.instance.Player1;
                break;
            case 6:
                // Des digivol
                CanvasListo.SetActive(false);
                MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(ResetNetOcean);
                break;
            case 7:
                // agregar a mi mano
                MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().RobarInteligente();
                MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().RobarInteligente();
                MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().RobarInteligente();
                //Agregamos al 40%

                GameObject Porcentage = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                Porcentage.GetComponent<CartaDigimon>().cardNumber = 34;
                Porcentage.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(59);
                PartidaManager.instance.Player1._Mano.RecibirCarta(Porcentage.GetComponent<CartaDigimon>(), true);
                Porcentage.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = true;
                StartCoroutine(WhaitFrame(Porcentage));

                //Agregamos al SCULLGREYMOPN

                GameObject Skull = Instantiate(CartaPrefab, PartidaManager.instance.ManoPlayer1);
                Skull.GetComponent<CartaDigimon>().cardNumber = 35;
                Skull.GetComponent<CartaDigimon>().DatosDigimon = DataManager.instance.GetDigicarta(32);
                PartidaManager.instance.Player1._Mano.RecibirCarta(Skull.GetComponent<CartaDigimon>(), true);
                Skull.GetComponent<CartaDigimon>().Front.GetComponent<MovimientoCartas>().Mover = true;
                StartCoroutine(WhaitFrame(Skull));

                // salto de Face
                StaticRules.instance.NowPhase = Phases.PreparationPhase;
                StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
                StaticRules.instance.NowPreparationPhase = 0;
                PartidaManager.instance.ActivateHand(true);

                NowDigiEfectos = DigiEfectos.CartasCompletas;
               // Invoke("waithCardEfec", 1.1f);

               
                break;
        }
        FasesID++;



    }

    public void ResetNetOcean(string result)
    {
        MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().addDarkArea("Evoluciones");
        Invoke("Iniciar",1F);
    }

    public void ResetNet2(string result)
    {
        MesaManager.instance.Campo1.NetOcean.GetComponent<NetOcean>().addDarkArea("Evoluciones2");
        Invoke("TomarCartas", 1F);
    }

    public void TomarCartas()
    {
        // jalar carta para ambos 
        for (int i = PartidaManager.instance.Player1._Mano.Cartas.Count; i < 6; i++)
        {
            PartidaManager.instance.TomarCarta(PartidaManager.instance.ManoPlayer1, PartidaManager.instance.Player1, MesaManager.instance.Campo1.NetOcean);
        }
        StaticRules.instance.NowPhase = Phases.PreparationPhase;
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
        StaticRules.instance.NowPreparationPhase = 0;
    }

    public void CheckSkullGreymon(string result)
    {
        if (MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.id == 32)
        {
            Iniciar();
        }else
        {
            Debug.LogError("CheckFalse");
            // salto de Face
            StaticRules.instance.NowPhase = Phases.PreparationPhase;
            StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
            StaticRules.instance.NowPreparationPhase = 0;
            MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>().LostDigimon(ResetNet2);
            // Evolution Box Reset
            foreach (var item in MesaManager.instance.Campo1.EvolutionBox.GetComponent<EvolutionBox>().Cartas) 
            {
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item, 0.5f);
            }
            MesaManager.instance.Campo1.EvolutionBox.GetComponent<EvolutionBox>().Cartas.Clear();
            // EvolutionRequeriment Reset
            foreach (var item in MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().Cartas)
            {
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item, 0.5f);
            }
            MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().Cartas.Clear();
            // EvolutionRequeriment Reset x/o
            foreach (var item in MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().ListaXO)
            {
                MesaManager.instance.GetSlot(MesaManager.Slots.DarkArea).GetComponent<DarkArea>().AddListDescarte(item, 0.5f);
            }
            MesaManager.instance.Campo1.EvolutionRequerimentBox.GetComponent<EvolutionRequerimentBox>().ListaXO.Clear();

          

        }
    }

    public void waithCardEfect()
    {
        StaticRules.instance.NowPhase = Phases.PreparationPhase;
        StaticRules.instance.WhosPlayer = PartidaManager.instance.Player1;
        StaticRules.instance.NowPreparationPhase = 0;
        PartidaManager.instance.ActivateHand(true);
    }

    public void endPhases()
    {
        LevelLoader.instance.CargarEscena("Tutorial");
    }
    public void Backtutorial()
    {
        if (NowSelectTuto== TutoStates.Campos)
        {
            TargetID = 0;
            DataManager.instance.FadeCanvas(PanelTutorialDialog);
            DataManager.instance.FadeCanvas(PanelTutorial, true);
            StartCoroutine(ReOriginalPos());
        }
        else
        {
            LevelLoader.instance.CargarEscena("Tutorial");
        }
       
    }

    public IEnumerator WhaitFrame(GameObject Carta)
    {
        yield return new WaitForEndOfFrame();
        Carta.SetActive(false);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Carta.SetActive(true);
        PartidaManager.instance.InterAjuste(Carta.GetComponent<CartaDigimon>());
    }

    public IEnumerator WhaitTimeFrame()
    {
        Nextdialog.interactable = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Nextdialog.interactable = true;
    }
}
