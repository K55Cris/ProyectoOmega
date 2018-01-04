using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace DigiCartas
{
    [Serializable]
    public class DigiCarta
    {
        public int id; //Nro de Carta
        public string Descripcion;
        public string Habilidad;
        public int dañoBase;
        public char TipoBatalla;
        public string Familia;
        public string Atributo;
        public string Tipo;
        public string Nivel;
        public string Borde;
        public int AtaqueA;
        public int AtaqueB;
        public string AtaqueC; // ya que este ataque tienen habilidades de efecto de a =0 se queda como string para ser evalauado en las reglas y
                               // cuando se asigne su uso se aplica su efecto por el momento queda asi ("Guard(A-0)")

        // ACA  sera mas complicado pero se tendra que ser un swich para evaluar cada tipo de requerimiento por invidual en momentoque se consulte la 
        // evolucion, esta peticion se manda el keycorrepondiente a Regtlas para evualar si se puede 
        public string keyREvolucion;
        public string keyREvolucion2;
        public string keyREvolucion3;
        public string keyREvolucion4;
    }

    [Serializable]
    public class Mazo
    {
        public List<DigiCarta> cartas;
    }
}
