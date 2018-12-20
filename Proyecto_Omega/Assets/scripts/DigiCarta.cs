using System.Collections;
using System;
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
        public List<string> ListaRequerimientos;
        public List<string> ListaEfectos;
        public string Limite;
        public bool IsSupport;
        public bool IsActivateHand;
    }




    [Serializable]
    public class Cartas
    {
        public List<DigiCarta> DigiCartas;
        public List<DigiCarta> CartaOption;
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
        public Player jugador= new Player();
        public List<CartaDigimon> Cartasbloqueadas= new List<CartaDigimon>();
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
    public class ListEfectos
    {
        public List<int> cards;
    }

    public enum Dificultad
    {
        Facil, Normal, Dificil, Experto
    };

    public enum EfectosActivos
    {
        allyChageAtack, dropCards, SetDarkArea, BuffAtack, doblePower, QuitDigimonBox, doublelostpoint, lostcardgame, checknivel,DiscardHand, Ignor4, IgnorAll , EnemyDesDigivolucionar
    };

    public enum Phases
    {
        GameSetup = 0, DiscardPhase = 1, WhaitDiscardPhase = 2, PreparationPhase = 3,PreparationPhase2 = 4, EvolutionPhase = 5,
        EvolutionPhase2 = 6,EvolutionRequirements = 7, FusionRequirements =8, AppearanceRequirements = 9,
        BattlePhase = 10, OptionBattlePhase = 11, PointCalculationPhase = 12, EndPhase = 13
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

    [Serializable]
    public class Efecto
    {
        public EfectosActivos NameEfecto;
        public List<CartaDigimon> CartaAfecta= new List<CartaDigimon>();
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
}