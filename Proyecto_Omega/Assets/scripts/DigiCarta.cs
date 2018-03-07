﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace DigiCartas
{
    [Serializable]
    public class Mazo
    {
        public List<DigiCarta> cartas;
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
        public string keyREvolucion;
        public string keyREvolucion2;
        public string keyREvolucion3;
        public string keyREvolucion4;
    }

    [Serializable]
    public class Cartas
    {
        public List<DigiCarta> DigiCartas;
    }

}