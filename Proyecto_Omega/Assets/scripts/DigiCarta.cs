using System.Collections;
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
        public string descripcion;
        public string TipoBatalla;
        public string Familia;
        public string Atributo;
        public string Tipo;
        public string Nivel;
        public string Borde;
        public string NombreAtaqueA;
        public int DanoAtaqueA;
        public string NombreAtaqueB;
        public int DanoAtaqueB;
        public string NombreAtaqueC;
        public int DanoAtaqueC;
        public int PerdidaVidaIII;
        public int PerdidaVidaIV;
        public int PerdidaVidaPerfect;
        public int PerdidaVidaUltimate;
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
