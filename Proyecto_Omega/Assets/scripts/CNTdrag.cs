using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CNTdrag : MonoBehaviour {
    //constantes
    public static int OPTION_SLOT = 0;
    public static int DIGIMON_BOX = 1;

    public static GameObject objetoArrastrado;
    public static GameObject objetoQuieto;
    public static int posicionOpcion;
    public static int tipo;
    public static bool clikeado;
    public static bool estoyEnMesa;
    public static bool elegida;
    public static Component claseCartas;
    public static List<Sprite> arrayRookie;
    public static List<Sprite> arrayImage;
    public static OptionSlot optionSlot;
    public static List<string> deck;

    //valores de la mesa
    public static bool pointGauge;
    public static bool evoRequerimentBox;
    public static bool evolutionBox;
    public static bool supportBox;
    public static bool optionSlot1 = true;
    public static bool optionSlot2 = true;
    public static bool optionSlot3 = true;
    //public static bool digimonBox;
    //public static bool netOcean;
    //public static bool darkArea;
}
