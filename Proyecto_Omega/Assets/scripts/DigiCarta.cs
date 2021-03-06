﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace DigiCartas
{
    [Serializable]
    public class Mazo
    {
        public List<CartaDigimon> cartas;
    }



    [Serializable]
    public class DigiCarta
    {
        public int id;
        public string Nombre;
        //Información de la carta. 
        public string descripcion;
        public string TipoBatalla;
        //Es la familia a la que pertenece el Digimon. Las familias pueden ser (NSp, NSo, WG, DS, ME, UK, DA y VB) y puede contener a mas de una.
        public string Familia;
        //Es el grupo al que pertenece el Digimon.
        public string Atributo;
        //Es el atributo del digimon. Pueden ser (Data, Vaccine, Virus, Free y Variable).
        public string Tipo;
        //Es el tipo del digimon.
        public string Nivel;
        public string Borde;
        //Nombre del ataque A.
        public string NombreAtaqueA;
        //Daño del ataque A.
        public int DanoAtaqueA;
        //Nombre del ataque B.
        public string NombreAtaqueB;
        //Daño del ataque B.
        public int DanoAtaqueB;
        //Nombre del ataque C.
        public string NombreAtaqueC;
        //Daño del ataque C.
        public int DanoAtaqueC;
        //Puntos perdidos por un Lv III
        public int PerdidaVidaIII;
        //Puntos perdidos por un Lv IV
        public int PerdidaVidaIV;
        //Puntos perdidos por un Lv PERFECT
        public int PerdidaVidaPerfect;
        //Puntos perdidos por un Lv Ultimate
        public int PerdidaVidaUltimate;
        //Las habilidades especiales del Digimon. Son las habilidades marcadas con un ◎.
        public string Habilidad;
        public string codigo;
        public int Capasidad;
        public List<string> ListaActivacion;
        public List<string> ListaCatrgoria;
        public List<string> ListaCosto;
        //Son los requisitos que deben cumplirse antes de poder evolucionar al digimon //Indica los requisitos que deben cumplirse para activar la carta.
        public List<string> ListaRequerimientos = new List<string>();
        public List<string> ListaEfectos = new List<string>();
        public string Limite;
        public bool IsSupport;
        public bool IsActivateHand;
        public List<string> ListaEfectosMostrar;
        public List<string> IDHabilidad;
    }




    [Serializable]
    public class Cartas
    {
        public List<DigiCarta> DigiCartas;
        public List<DigiCarta> CartaOption;
        public List<DigiCarta> SXDigiCartas;
        public List<DigiCarta> SXCartaOption;
    }

    [Serializable]
    public class DigiEvoluciones
    {
        public Transform DigiCarta;
        public bool isJoggres;
        public Efectos Namefecto;
        public bool IsIgnore;
    }


    [Serializable]
    public class ListaCartasMove
    {
        public int ID;
        public UnityAction<CartaDigimon> LoAction;
        public Transform Padre;
        public CartaDigimon CartaOption;
    }
    [Serializable]
    public class CartasBloqueadas
    {
        public Player jugador = new Player();
        public List<CartaDigimon> Cartasbloqueadas = new List<CartaDigimon>();
    }
    [Serializable]
    public class bLOCKCARDS
    {
        public CartasBloqueadas UNO;
        public CartasBloqueadas DOS;
    }
    [Serializable]
    public class Campos
    {
        public Transform OptionSlot1, OptionSlot2, OptionSlot3, DigimonSlot, NetOcean,
            DarkArea, EvolutionBox, SupportBox, Campo, PointGauge, EvolutionRequerimentBox, FronDigimon;
    }

    [Serializable]
    public class SavePlayer
    {
        public string Nombre;
        public List<int> IDCartasMazo;
        public List<DIDCarta> IDCartasDisponibles;
        public bool ALLCards;
        public List<Progreso> Progresos;
        public int Nivel = 0;
        public string Photo, Tablero, Funda, FondoTablero;
        public List<Coleccionables> MisColeccionables = new List<Coleccionables>();
    }


    [Serializable]
    public class QuickPlayer
    {
        public string Nombre;
        public List<int> IDCartasMazo;
        public int Nivel = 0;
        public string Photo, Tablero, Funda, FondoTablero;
    }

    [Serializable]
    public struct AudioSettings
    {
        public float Music;
        public float efects;
    }

    [Serializable]
    public class Progreso
    {
        public int ID;
        public bool Completo;
        public bool Cerca;
        public int Victorias;
        public int Derrotas;
    }
    [Serializable]
    public class DIDCarta
    {
        public int ID;
        public int Cantidad;
    }
    [Serializable]
    public class ListCard
    {
        public List<int> cards;
    }
    [Serializable]
    public class DecksIA
    {
        public List<int> mazo;
        public Dificultad DificultadMazo;
    }
    [Serializable]
    public class IADecks
    {
        public List<DecksIA> Decks;
    }
    [Serializable]
    public class ListEfectos
    {
        public List<int> cards;
    }

    public enum Dificultad
    {
        Facil, Normal, Dificil, Experto
    };

    public enum Habilidades
    {
        Heal=1,Sky=2, Underwater=3, underground = 4, Flame=5 , Freezing=6, Proud=7, Gale=8, Marksmanship=9, Fency=10, Strategy=11, Ardor=12, DiscardDarkOponetWin=13, MismoDestino=14
    };

    public enum EfectosActivos
    {
        allyChageAtack, dropCards, SetDarkArea, BuffAtack, doblePower, QuitDigimonBox, doublelostpoint, lostcardgame, checknivel, DiscardHand, Ignor4, IgnorAll, EnemyDesDigivolucionar
    };

    public enum Phases
    {
        GameSetup = 0, DiscardPhase = 1, WhaitDiscardPhase = 2, PreparationPhase = 3, PreparationPhase2 = 4, EvolutionPhase = 5,
        EvolutionPhase2 = 6, EvolutionRequirements = 7, EvolutionRequirements2=8,
        FusionRequirements = 9, AppearanceRequirements = 10,
        BattlePhase = 11, OptionBattlePhase = 12, PointCalculationPhase = 13, EndPhase = 14
    };

    public enum Campo
    {
        DigimonBox = 0, OptionSlot = 1, EvolutionSlot = 2, Requeriment = 3, Deck = 4,
        Netocean = 5, Support = 6,

    };
    public enum Efectos
    {
        SinRequerimientosAll = 0, SinRequerimientos4 = 1
    };
    public enum TypeTutorial
    {
        Dialogo = 0, Action = 1
    };
    [Serializable]
    public class TurnEfect
    {
        public Efectos Efecto;
        public Player Jugador;
    }
    [Serializable]
    public class ListaTutorialText
    {
        public List<TextosTutorial> ListaCompleta;
        public List<GameObject> LPos;

    }
    [Serializable]
    public class TextosTutorial
    {
        public List<TextosItem> Dialogos;
        public string Subtitulo;
    }

    [Serializable]
    public class TextosItem
    {
        public TypeTutorial Tipo;
        public string Dialogo;
    }

    [Serializable]
    public class Efecto
    {
        public EfectosActivos NameEfecto;
        public List<CartaDigimon> CartaAfecta = new List<CartaDigimon>();
        public CartaDigimon OptionCard;
        public Transform Origen;
        public Transform Destino;
        public string Ataque;
        public int cantidadCartas;
        public int buffo;
        public string where;
        public Phases Limite;
        public Player Jugador;
    }


    [Serializable]
    public class Coleccionables
    {
        public int ID;
        public string Nombre;
        public Sprite Image;
        public Color Rareza;
        public ColeccionablesType Tipo;
    }
    [Serializable]
    public enum ColeccionablesType
    {
        PerfilFoto = 0, FondoTablero = 1, Tablero = 2, Funda = 3
    };

    public enum RarezaType
    {
        Default = 0, Comun = 1, Raro = 2, Epico = 3, Legendario = 4, Unico = 5
    };
    [Serializable]
    public class ColeccionablesData
    {
        public int ID;
        public string Nombre;
        public string Image;
        public RarezaType Rareza;
        public ColeccionablesType Tipo;
    }

}