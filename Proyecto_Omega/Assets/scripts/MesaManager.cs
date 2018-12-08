using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class MesaManager : MonoBehaviour {
    public Campos Campo1;
    public Campos Campo2;
    // Use this for initialization
    public static MesaManager instance;

    private void Awake()
    {
        instance = this;
    }
    public enum Slots
    {
        OptionSlot1 = 0, OptionSlot2 = 1, OptionSlot3 = 2, DigimonSlot = 3, NetOcean = 4, DarkArea = 5,
        EvolutionBox = 6, SupportBox = 7, Campo = 8, PointGauge = 9, EvolutionRequerimentBox = 10, frontSlot=11,
    }
   
    public Transform GetSlot(Slots slot,Player player= null)
    {
        Player _player = StaticRules.instance.WhosPlayer;
        if (player)
            _player = player;


     if(PartidaManager.instance.Player1== _player)
        {
            switch (slot)
            {
                case Slots.OptionSlot1:
                    return Campo1.OptionSlot1;
                case Slots.OptionSlot2:
                    return Campo1.OptionSlot2;
                case Slots.OptionSlot3:
                    return Campo1.OptionSlot3;
                case Slots.DigimonSlot:
                    return Campo1.DigimonSlot;
                case Slots.NetOcean:
                    return Campo1.NetOcean;
                case Slots.DarkArea:
                    return Campo1.DarkArea;
                case Slots.EvolutionBox:
                    return Campo1.EvolutionBox;
                case Slots.SupportBox:
                    return Campo1.SupportBox;
                case Slots.Campo:
                    return Campo1.Campo;
                case Slots.PointGauge:
                    return Campo1.PointGauge;
                case Slots.EvolutionRequerimentBox:
                    return Campo1.EvolutionRequerimentBox;
                case Slots.frontSlot:
                    return Campo1.FronDigimon;
            }
        }
        else
        {
            switch (slot)
            {
                case Slots.OptionSlot1:
                    return Campo2.OptionSlot1;
                case Slots.OptionSlot2:
                    return Campo2.OptionSlot2;
                case Slots.OptionSlot3:
                    return Campo2.OptionSlot3;
                case Slots.DigimonSlot:
                    return Campo2.DigimonSlot;
                case Slots.NetOcean:
                    return Campo2.NetOcean;
                case Slots.DarkArea:
                    return Campo2.DarkArea;
                case Slots.EvolutionBox:
                    return Campo2.EvolutionBox;
                case Slots.SupportBox:
                    return Campo2.SupportBox;
                case Slots.Campo:
                    return Campo2.Campo;
                case Slots.PointGauge:
                    return Campo2.PointGauge;
                case Slots.EvolutionRequerimentBox:
                    return Campo2.EvolutionRequerimentBox;
                case Slots.frontSlot:
                    return Campo2.FronDigimon;
            }
        }
        return null;
    }
    public CartaDigimon GetDigimonSlot()
    {
        CartaDigimon DigimonCombatiente = GetSlot(MesaManager.Slots.DigimonSlot).GetComponent<DigimonBoxSlot>()._DigiCarta;
        return DigimonCombatiente;
    }

    public Transform GetOptionSlotForCard(DigiCarta Dcarta)
    {
        if (PartidaManager.instance.Player1 == StaticRules.instance.WhosPlayer)
        {
            if (Campo1.OptionSlot1.GetComponent<OptionSlot>().OpCarta && Campo1.OptionSlot1.GetComponent<OptionSlot>().OpCarta.DatosDigimon.id == Dcarta.id)
                return Campo1.OptionSlot1;
            if (Campo1.OptionSlot2.GetComponent<OptionSlot>().OpCarta && Campo1.OptionSlot2.GetComponent<OptionSlot>().OpCarta.DatosDigimon.id == Dcarta.id)
                return Campo1.OptionSlot2;
            if (Campo1.OptionSlot3.GetComponent<OptionSlot>().OpCarta && Campo1.OptionSlot3.GetComponent<OptionSlot>().OpCarta.DatosDigimon.id == Dcarta.id)
                return Campo1.OptionSlot3;
        }
        else
        {
            if (Campo2.OptionSlot1.GetComponent<OptionSlot>().OpCarta.DatosDigimon.id == Dcarta.id)
                return Campo1.OptionSlot1;
            if (Campo2.OptionSlot2.GetComponent<OptionSlot>().OpCarta.DatosDigimon.id == Dcarta.id)
                return Campo1.OptionSlot2;
            if (Campo2.OptionSlot3.GetComponent<OptionSlot>().OpCarta.DatosDigimon.id == Dcarta.id)
                return Campo1.OptionSlot3;
        }
        return null;
    }

    public static Transform SetOptionSlot(GameObject Carta , bool Campo=false)
    {
        if (Campo)
        {
            // Para la IA
        }
        else
        {
            Vector3 LocalPos = Carta.transform.position;
            foreach (Transform item in MesaManager.instance.Campo1.Campo)
            {
                Vector3 PosCampo = item.position;
                
                float dist = Vector3.Distance(LocalPos, PosCampo);
                Debug.Log(dist+":"+item);
                if (dist < 32)
                {
                    Debug.Log(item);
                    return item;
          
                }
            }
        }
        return null;
    }
    public Player WhatSlotPlayer(Transform Slot, Slots slot) 
    {
        Player jugador1 = PartidaManager.instance.Player1;
        Player jugador2 = PartidaManager.instance.Player2;

        switch (slot)
        {
            case Slots.DigimonSlot:
                if (Campo1.DigimonSlot == Slot)
                {
                    return jugador1;
                }
                else if (Campo2.DigimonSlot == Slot)
                {
                    return jugador2;
                }
                else
                {
                    return null;
                }
            case Slots.DarkArea:
                if (Campo1.DarkArea== Slot)
                {
                    return jugador1;
                }
                else if (Campo2.DarkArea == Slot)
                {
                    return jugador2;
                }
                else
                {
                    return null;
                }
            case Slots.EvolutionRequerimentBox:
                if (Campo1.EvolutionRequerimentBox == Slot)
                {
                    return jugador1;
                }
                else if (Campo2.EvolutionRequerimentBox == Slot)
                {
                    return jugador2;
                }
                else
                {
                    return null;
                }
            case Slots.NetOcean:
                if (Campo1.NetOcean == Slot)
                {
                    return jugador1;
                }
                else if (Campo2.NetOcean == Slot)
                {
                    return jugador2;
                }
                else
                {
                    return null;
                }
        }
        return null;
    }
 
}
