using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using DigiCartas;
public class Tutorial : MonoBehaviour {

    public TextMeshProUGUI Descripcion,Titulo,Dialogo;
    public GameObject Panel;
    public Transform OriginalPos;
    public CanvasGroup PanelTutorial;
    public CanvasGroup PanelTutorialDialog;
    public GameObject TargetCamera;
    public GameObject CanvasListo;
    public ListaTutorialText LtexCampos;

    public ListaTutorialText LtexFases;

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

    public int TargetID =0;
    public enum TutoStates
    {
        Campos = 0, Fases = 1, Evolucion, Efectos
    };

    public bool WhaitAction;

    public TutoStates NowSelectTuto;

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
        NowSelectTuto = TutoStates.Fases;
        Invoke("Iniciar", .6f);
    }
    public void Evolucion()
    {
        // Cargar Tutorial de Campos
        DataManager.instance.FadeCanvas(PanelTutorial);
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
                if(TargetID>=LtexCampos.LPos.Count)
                StartCoroutine(Transicion(LtexCampos.LPos[LtexCampos.LPos.Count-1].transform, TargetID, LtexCampos.ListaCompleta,LectorTexto));
                else
                {
                    StartCoroutine(Transicion(LtexCampos.LPos[TargetID].transform, TargetID,LtexCampos.ListaCompleta ,LectorTexto));
                }
                break;
            case TutoStates.Fases:

                if (LtexFases.ListaCompleta[TargetID].Dialogos[Contador].Tipo!= TypeTutorial.Action)
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
                break;
            case TutoStates.Evolucion:
                break;
            case TutoStates.Efectos:
                break;
            default:
                break;
        }
    }

    public IEnumerator Transicion(Transform Destino, int ID, List<TextosTutorial> lista , UnityAction<int, List<TextosTutorial>> LoAction)
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
        LoAction.Invoke(ID,lista);

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


    public void LectorTexto(int ID, List<TextosTutorial> ListaTexto )
    {
        TextosTutorial Datos = new TextosTutorial();
        if (ID != ListaTexto.Count)
        {
            Datos = ListaTexto[ID];
            Iniciar(Datos);
        }
        else
        {
            TargetID = 0;
            DataManager.instance.FadeCanvas(PanelTutorialDialog);
            DataManager.instance.FadeCanvas(PanelTutorial, true);
            StartCoroutine(ReOriginalPos());
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
        DataManager.instance.FadeCanvas(PanelTutorialDialog,true);
    }

    public void NextDialogo()
    {
        if (NowData.Dialogos.Count > Contador)
        {
            if (NowData.Dialogos[Contador].Tipo == TypeTutorial.Action)
            {
                // ESPERA Activador
                DataManager.instance.FadeCanvas(PanelTutorialDialog, false);
                Contador=0;
                Secuenciafases();
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
               
                break;
        }
        FasesID++;
    }

}
