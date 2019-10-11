using System.Collections.Generic;

public class CartaOption : Carta
{
    //Es la capacidad de la carta.
    private int capacity;
    //Cualquier información adicional sobre como jugar la carta.
    private List<string> activationTiming;
    //Es la categoría a la que pertenece la carta.
    private List<string> category;
    //Indica el costo que debe pagarse antes de activar la carta. 
    private List<string> cost;
    //Indica los requisitos que deben cumplirse para activar la carta.
    private List<string> requirements;
    //Indica los efectos de la carta.
    private List<string> effects;
    //Indica cuanto tiempo permanece en juego la carta antes de ser enviada al Dark Area, o indicador de puntos.
    private string limit;

    //GaS
    public int Capacity
    {
        get
        {
            return capacity;
        }

        set
        {
            capacity = value;
        }
    }

    public List<string> ActivationTiming
    {
        get
        {
            return activationTiming;
        }

        set
        {
            activationTiming = value;
        }
    }

    public List<string> Category
    {
        get
        {
            return category;
        }

        set
        {
            category = value;
        }
    }

    public List<string> Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
    }

    public List<string> Requirements
    {
        get
        {
            return requirements;
        }

        set
        {
            requirements = value;
        }
    }

    public List<string> Effects
    {
        get
        {
            return effects;
        }

        set
        {
            effects = value;
        }
    }

    public string Limit
    {
        get
        {
            return limit;
        }

        set
        {
            limit = value;
        }
    }

}
