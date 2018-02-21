using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaDigimon : Carta {
    //Son los requisitos que deben cumplirse antes de poder evolucionar al digimon.
    private List<string> evolutionRequirement;
    //Es el nivel del digimon (Nvl III, IV, adulto, etc.).
    private string level;
    //Es el tipo del digimon.
    private string type;
    //Es el atributo del digimon. Pueden ser (Data, Vaccine, Virus, Free y Variable).
    private string attribute;
    //Es el grupo al que pertenece el Digimon.
    private string group;
    //Es la familia a la que pertenece el Digimon. Las familias pueden ser (NSp, NSo, WG, DS, ME, UK, DA y VB) y puede contener a mas de una.
    private List<string> field;
    //Información de la carta. 
    private string information;
    //Nombre del ataque A.
    private string attackA;
    //Daño del ataque A.
    private int damageAttackA;
    //Nombre del ataque B.
    private string attackB;
    //Daño del ataque B.
    private int damageAttackB;
    //Nombre del ataque C.
    private string attackC;
    //Daño del ataque C.
    private int damageAttackC;
    //Puntos perdidos por un Lv III
    private int lostPointsIII;
    //Puntos perdidos por un Lv IV
    private int lostPointsIV;
    //Puntos perdidos por un Lv Perfect
    private int lostPointsPerfect;
    //Puntos perdidos por un Lv Ultimate
    private int lostPointsUltimate;
    //Las habilidades especiales del Digimon. Son las habilidades marcadas con un ◎.
    private string abilitieSpecial;
    //Las habilidades de support del Digimon. Son las habilidades marcadas con un ■.
    private string abilitieSupport;

    //GaS
    public List<string> EvolutionRequirement
    {
        get
        {
            return evolutionRequirement;
        }

        set
        {
            evolutionRequirement = value;
        }
    }

    public string Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public string Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public string Attribute
    {
        get
        {
            return attribute;
        }

        set
        {
            attribute = value;
        }
    }

    public string Group
    {
        get
        {
            return group;
        }

        set
        {
            group = value;
        }
    }

    public List<string> Field
    {
        get
        {
            return field;
        }

        set
        {
            field = value;
        }
    }

    public string Information
    {
        get
        {
            return information;
        }

        set
        {
            information = value;
        }
    }

    public string AttackA
    {
        get
        {
            return attackA;
        }

        set
        {
            attackA = value;
        }
    }

    public int DamageAttackA
    {
        get
        {
            return damageAttackA;
        }

        set
        {
            damageAttackA = value;
        }
    }

    public string AttackB
    {
        get
        {
            return attackB;
        }

        set
        {
            attackB = value;
        }
    }

    public int DamageAttackB
    {
        get
        {
            return damageAttackB;
        }

        set
        {
            damageAttackB = value;
        }
    }

    public string AttackC
    {
        get
        {
            return attackC;
        }

        set
        {
            attackC = value;
        }
    }

    public int DamageAttackC
    {
        get
        {
            return damageAttackC;
        }

        set
        {
            damageAttackC = value;
        }
    }

    public int LostPointsIII
    {
        get
        {
            return lostPointsIII;
        }

        set
        {
            lostPointsIII = value;
        }
    }

    public int LostPointsIV
    {
        get
        {
            return lostPointsIV;
        }

        set
        {
            lostPointsIV = value;
        }
    }

    public int LostPointsPerfect
    {
        get
        {
            return lostPointsPerfect;
        }

        set
        {
            lostPointsPerfect = value;
        }
    }

    public int LostPointsUltimate
    {
        get
        {
            return lostPointsUltimate;
        }

        set
        {
            lostPointsUltimate = value;
        }
    }

    public string AbilitieSpecial
    {
        get
        {
            return abilitieSpecial;
        }

        set
        {
            abilitieSpecial = value;
        }
    }

    public string AbilitieSupport
    {
        get
        {
            return abilitieSupport;
        }

        set
        {
            abilitieSupport = value;
        }
    }
}
