using UnityEngine;
using UnityEngine.UI;
public class NewCard : MonoBehaviour
{
    public Image ImgCard;
    public RecomnesaManager Manager;

    // Use this for initialization
    private void OnEnable()
    {
        if (PlayerManager.instance.Jugador.ALLCards)
            LevelLoader.instance.CargarEscena("Main Menu");
        else
            StartNewCard();
    }

    // Pucha el boton de la carta
    public void StartNewCard()
    {
        DataManager.instance.WinCard(SetResults);

    }
    public void SetResults(int ID, bool desbloqueado)
    {
        Sprite NewDigimon = DataManager.instance.GetSprite(ID);
        if (NewDigimon)
        {
            ImgCard.sprite = NewDigimon;
            Manager.GetCarta(ID, desbloqueado);
        }

    }
}
