using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesa : MonoBehaviour {
    public DigimonBox DigimonBox;
    public OptionSlot OptionSlot1;
    public OptionSlot OptionSlot2;
    public OptionSlot OptionSlot3;
    public EvolutionBox EvolutionBox;
    public EvolutionRequerimentBox EvolutionRequerimentBox;
    public DarkArea DarkArea;
    public NetOcean NetOcean;
    public SupportBox SupportBox;

    public void VaciarMesa()
    {
        DigimonBox.VaciarSlot();
        OptionSlot1.VaciarSlot();
        OptionSlot2.VaciarSlot();
        OptionSlot3.VaciarSlot();
        EvolutionBox.VaciarSlot();
        EvolutionRequerimentBox.VaciarSlot();
        SupportBox.VaciarSlot();
    }
}
