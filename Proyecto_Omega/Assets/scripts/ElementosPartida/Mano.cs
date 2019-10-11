using System.Collections.Generic;
using UnityEngine;

public class Mano : MonoBehaviour
{
    public List<CartaDigimon> Cartas = new List<CartaDigimon>();
    public int Limite;

    public void JugarCarta(CartaDigimon carta)
    {

        Cartas.Remove(carta);


        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial" && Tutorial.instance.NowSelectTuto == Tutorial.TutoStates.Evolucion)
        {
            if (Cartas.Count == 0 && Tutorial.instance.NowDigiEfectos == Tutorial.DigiEfectos.CartasCompletas)
            {
                StaticRules.instance.NowPhase = DigiCartas.Phases.PreparationPhase2;
                Tutorial.instance.CanvasListo.SetActive(true);
                StaticRules.instance.FinishEvolution = Tutorial.instance.CheckSkullGreymon;
            }
        }
    }

    public void DescartarCarta(CartaDigimon carta)
    {
        Cartas.Remove(carta);
    }

    public void RecibirCarta(CartaDigimon carta, bool PlayerOrIA = false)
    {
        Cartas.Add(carta);

        if (PlayerOrIA)
        {
            carta.Mostrar();
            //  carta.transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 180));
        }
        else
        {

        }

    }

    private void Reordenar()
    {

    }
}
