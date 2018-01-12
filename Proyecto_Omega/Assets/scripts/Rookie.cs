using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rookie : MonoBehaviour, IPointerClickHandler
{
    public GameObject carta;
    public Button btn;
    public Image imagen;
    private UnityAction rookieLisener;
    private int clickCouter;
    private void Awake()
    {
        rookieLisener = new UnityAction(RoboRookie);
    }
    private void OnEnable()
    {
        EventManager.StartListening("RobarRookie", rookieLisener);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        clickCouter = eventData.clickCount;
    }
    private void RoboRookie()
    {
        if (clickCouter == 2)
        {
            //prueba para ahora
            EventTrigger.inicio = false;
            EventManager.TriggerEvent("Botonazo"); //Ubicacion EventTrigger
            EventManager.TriggerEvent("Roba"); //Ubicacion EventTrigger
            //EventTrigger.boton.interactable = true;
            //movimiento de carta
            GameObject rookie = Instantiate(carta);
            rookie.GetComponent<Renderer>().material.mainTexture = imagen.mainTexture;
            rookie.transform.position = new Vector3(241.4f, 0.2f, 177.2f);
            rookie.transform.Rotate(0, 180, 0);
            Destroy(GetComponent<Rookie>());
            Destroy(GameObject.Find("Seleccion De cartas Scroll"));
            GameObject.Find("Deck").GetComponent<Deck>().RobarEspecifico(imagen.mainTexture.name);
        }
    }
}
