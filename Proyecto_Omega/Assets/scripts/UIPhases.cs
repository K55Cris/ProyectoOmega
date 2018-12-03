using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIPhases : MonoBehaviour {

    public UIParticleSystem Phase;
    public List<ParticleSystem> Particulas;
    public List<Material> MaterialPhases;

    private void Start()
    {
        foreach (var item in Particulas)
        {
            item.Stop();
        }
    }

    public void ChangePhase(bool player)
    {
        Particulas[0].Play();
        Particulas[1].Play();
        Particulas[2].Play();
        bool Play = player;
        switch (StaticRules.NowPhase)
        {
            case DigiCartas.Phases.GameSetup:
                break;
            case DigiCartas.Phases.DiscardPhase:
                ChangeMaterial(0, Play);
                break;
            case DigiCartas.Phases.PreparationPhase:
                ChangeMaterial(1, Play);
                break;
            case DigiCartas.Phases.PreparationPhase2:
                ChangeMaterial(1, Play);
                break;
            case DigiCartas.Phases.EvolutionPhase:
                ChangeMaterial(2, Play);
                break;
            case DigiCartas.Phases.BattlePhase:
                ChangeMaterial(3, Play);
                break;
            default:
                break;
        }

    }

    
    public void ChangeMaterial(int material, bool player)
    {
        Phase.material = MaterialPhases[material];

        if (player)
        {
            Particulas[1].Stop();
        }
        else
        {
            Particulas[0].Stop();
        }
    }
}
