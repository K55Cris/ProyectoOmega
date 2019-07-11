using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
public class PartidaManager : MonoBehaviour {

    public Player Player1;
    public Player Player2;
    public GameObject CartaPrefap;
    public Transform ManoPlayer1;
    public Transform ManoPlayer2;
    public GameObject MenuPausa;
    public UIPhases MenuPhases;
    public Button Listo,ListoOption;
    public Image PhasesPanel,FondoFinal,FondoPlayer;
    public Text PhasesText;
    public static PartidaManager instance;
    public string Player1Atack = "A";
    public string Player2Atack = "A";
    public Player WinTurno;
    public int EfectosCadena = 0;
    public bool CambioFase = true;
    public Sprite ListoOff, ListoON;
    public CanvasGroup CanvasFinal;
    public TextMeshProUGUI EtiquetaVictoria;
    private void Awake()
    {
        instance = this;
    }
    public Player GetEnemy()
    {
        if (StaticRules.instance.WhosPlayer == Player1)
            return Player2;
        else
            return Player1;
    }
    public void Update()
    {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
            Pausa();
            }
    }
    public void Salir()
    {
        // penalizar
        Time.timeScale = 1;
        SoundManager.instance.musicSource.UnPause();
        SoundManager.instance.sfxSource.UnPause();
        LevelLoader.instance.CargarEscena("Main Menu");
      
    }

    public void Pausa()
    {
        MenuPausa.SetActive(true);
        Time.timeScale = 0;
        SoundManager.instance.musicSource.Pause();
        SoundManager.instance.sfxSource.Pause();
    }
    public void Reanudar()
    {
        Time.timeScale = 1;
        MenuPausa.SetActive(false);
        SoundManager.instance.musicSource.UnPause();
        SoundManager.instance.sfxSource.UnPause();
    }

    public string GetAtackUse(Player Jugador)
    {
        if (Player1 == Jugador)
            return Player1Atack;
        else
            return Player2Atack;
    }

    public string WhoAtackUse(Transform DigimonBoxSlot)
    {
        if (DigimonBoxSlot == MesaManager.instance.Campo1.DigimonSlot)
        {
            return MesaManager.instance.Campo2.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.TipoBatalla;
        }
        else
        {
            return MesaManager.instance.Campo1.DigimonSlot.GetComponent<DigimonBoxSlot>()._DigiCarta.DatosDigimon.TipoBatalla;
        }
    }

    
    private void Start()
    {
 
        Player1.Nombre = PlayerManager.instance.Jugador.Nombre;
        Player1.NombreCuenta.text= PlayerManager.instance.Jugador.Nombre;
        Player1.IDCartasMazo = PlayerManager.instance.Jugador.IDCartasMazo;
        Player1.Photo.sprite = PlayerManager.instance.ImagePhoto;
        if(Player2.Photo)
        Player2.Photo.sprite = DataManager.instance.IAPhotos.Find(p => p.name == PlayerManager.instance.IaPlaying.ToString());
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!= "Tutorial")
        {

            // Musica de Duelo
            SoundManager.instance.PlayMusic(Sound.Duelo);
            StaticRules.SelectDigimonChild();

        }
        else
        {
           // tutorial
        }
       
    }


    public void CargarMazos(List<int> Deck, Transform Espacio, Player _Player)
    {
        List<DigiCarta> DatosDigi = DataManager.instance.TodasLasCartas;
        int contador = 1;
        List<CartaDigimon> MazoPlayer = new List<CartaDigimon>();
        foreach (var carta in Deck)
        {
            GameObject DigiCarta = Instantiate(CartaPrefap, Espacio);
            DigiCarta.GetComponent<CartaDigimon>().AjustarSlot();
            DigiCarta.GetComponent<CartaDigimon>().cardNumber = contador;
            DigiCarta.GetComponent<CartaDigimon>().DatosDigimon = DatosDigi.Find(x => x.id == carta);
            MazoPlayer.Add(DigiCarta.GetComponent<CartaDigimon>());
            contador++;
        }
        _Player.Deck.cartas = MazoPlayer;
        Transform netocean = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, _Player);
        netocean.GetComponent<NetOcean>().Cartas = MazoPlayer;
    }
    public void cargarManos(Transform Mano, Player jugador, Transform Deck)
    {
        StartCoroutine(CrearYColocar(Mano, jugador, Deck));
    }

    IEnumerator CrearYColocar(Transform Mano, Player jugador, Transform Deck)
    {

        for (int i = 1; i < 7; i++)
        {
            yield return new WaitForSecondsRealtime(0.3f);

            CartaDigimon Carta =  MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, jugador).GetComponent<NetOcean>().Robar();
            PartidaManager.instance.SetMoveCard(Mano, Carta.transform, InterAjuste);
            if (jugador == Player1)
                jugador._Mano.RecibirCarta(Carta, true);
            else
                jugador._Mano.RecibirCarta(Carta);

            
           // Carta.transform.localPosition = Vector3.zero;
            //Carta.transform.localScale = new Vector3(25, 40, 0.015f);
        }
    }

    public void TomarCarta(Transform Mano, Player jugador, Transform Deck)
    {

        CartaDigimon Carta = MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean, jugador).GetComponent<NetOcean>().Robar();
        if (Carta)
        { 
        PartidaManager.instance.SetMoveCard(Mano, Carta.transform, InterAjuste);
        if (jugador == Player1)
            jugador._Mano.RecibirCarta(Carta.GetComponent<CartaDigimon>(), true);
        else
            jugador._Mano.RecibirCarta(Carta.GetComponent<CartaDigimon>());
        }
    }

    public void InterAjuste(CartaDigimon Carta)
    {
        Carta.transform.localPosition = Vector3.zero;
        Carta.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Carta.Mostrar();
        Carta.transform.localScale = new Vector3(25, 40, 0.015f);
    }


    public static void Barajear(Transform Deck)
    {
        NetOcean NDeck = Deck.GetComponent<NetOcean>();

        int k = 0;
        while (k < 45)
        {
            int val = Random.Range(0, 29);
            CartaDigimon _Carta = NDeck.Cartas[val];
            NDeck.Cartas.Remove(_Carta);
            NDeck.Cartas.Add(_Carta);
            k++;
        }
    }
    public void BotonListo()
    {
        // Tutorial

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (StaticRules.instance.NowPhase == Phases.DiscardPhase)
            {
                Tutorial.instance.CanvasListo.SetActive(false);
                Tutorial.instance.Iniciar();
                StaticRules.SiguienteFase();
                return;
            }
            else
            {
               if(Tutorial.instance.NowDigiEfectos != Tutorial.DigiEfectos.CartasCompletas)
                {
                    Tutorial.instance.Iniciar();
                }
             
                StaticRules.SiguienteFase();
                return;
            }
        }

        StaticRules.SiguienteFase();

       
    }
    public Transform GetHand()
    {
        if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
        {
            return ManoPlayer1;
        }
        else
        {
            return ManoPlayer2;
        }
    }
    public void SetMoveCard(Transform Padre,Transform Carta, UnityAction<CartaDigimon> Loaction)
    {
        if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
        {
            Player1.moveCard(Padre, Carta.GetComponent<CartaDigimon>(),Loaction);
        }
        else
        {
            Player2.moveCard(Padre, Carta.GetComponent<CartaDigimon>(), Loaction);
        }
    }
    public void SetMoveHand(Transform Padre, Transform Carta, UnityAction<CartaDigimon> Loaction)
    {
        if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
        {
            Player1.moveHand(Padre, Carta.GetComponent<CartaDigimon>(), Loaction);
        }
        else
        {
            Player2.moveHand(Padre, Carta.GetComponent<CartaDigimon>(), Loaction);
        }
    }

    public void TomarOtraCarta()
    {
        MesaManager.instance.GetSlot(MesaManager.Slots.NetOcean).GetComponent<NetOcean>().RobarInteligente();
    }
    public void CambioDePhase(bool swi)
    {
        Listo.interactable = swi;
        if (swi)
            Listo.GetComponent<Image>().sprite = ListoON;
        else
            Listo.GetComponent<Image>().sprite = ListoOff;
        CambioFase = false;
        TiempoEsperaCambioPhase();
    }

    public void TiempoEsperaCambioPhase()
    {
        StartCoroutine(WhaitChangeFase());
    }
    public void Cambio(string PhaseName)
    {
        PhasesText.text = PhaseName;
        PhasesPanel.transform.GetComponent<Animator>().Play("StartPhase");
    }
    public void cambioEcena()
    {
        Invoke("change", 4f);
    }
    public void change()
    {
        LevelLoader.instance.CargarEscena("Main Menu");
    }
    public void ActivateHand(bool sw)
    {
        foreach (CartaDigimon item in PartidaManager.instance.Player1._Mano.Cartas)
        {
            item.Front.GetComponent<MovimientoCartas>().Mover = sw;
        }
    }


    public void RecurisvoEfecto(UnityAction<string> loAction)
    {
        StartCoroutine(CadenaEfectos(loAction));
    }
    public void RecurisvoEfectoID(UnityAction<string> loAction,string ID)
    {
        StartCoroutine(CadenaEfectosID(loAction,ID));
    }
    public IEnumerator CadenaEfectos(UnityAction<string> loAction)
    {
 
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);
        loAction.Invoke("Siguiente cadena");
    }
    public IEnumerator CadenaEfectosID(UnityAction<string> loAction,string ID)
    {

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);
        loAction.Invoke(ID);
    }

    public IEnumerator WhaitChangeFase()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        CambioFase = true;
    }
    private string ecena = "";
    public void Finishoptionbattle()
    {
        if (ListoOption)
        {
            ListoOption.gameObject.SetActive(false);
        }
        Listo.gameObject.SetActive(true);
        IA.instance.TurnoIA(false);

    }
    public void finishGame()
    {
        // prender menu de Victoria
        // Cargamos me3nu de recompensas
        LevelLoader.instance.CargarEscena(ecena);
    }
  
    public void ShowVictori()
    {
        DataManager.instance.FadeCanvas(CanvasFinal, true);
        ecena = "Recompensa";
        EtiquetaVictoria.text = "Victory";
        FondoFinal.color = Color.green;
       
    }
    public void ShowLose()
    {
        DataManager.instance.FadeCanvas(CanvasFinal, true);
        ecena = "MainMenu";
        EtiquetaVictoria.text = "Lost";
        FondoFinal.color = Color.red;
    }

    public void Victory(bool Desicion)
    {
        FondoPlayer.gameObject.SetActive(false);

        if (Desicion)
            Invoke("ShowVictori", 4f);
        else
            Invoke("ShowLose", 4f);
    }
 
}