using UnityEngine;
using UnityEngine.UI;

public class Rookie : MonoBehaviour
{
    private bool unClick = false;
    private bool enTiempo;
    private float tiempoParaElDobleClick;
    public float delayDelDobleClick = 1;
    public GameObject carta;
    public Button btn;
    public Image imagen;
    private void OnMouseDown()
    {
        if (!unClick)
        {
            unClick = true;
            tiempoParaElDobleClick = Time.time;
        }
        else
        {
            //movimiento de carta
            GameObject rookie = Instantiate(carta);
            rookie.GetComponent<Renderer>().material.mainTexture = imagen.mainTexture;
            rookie.transform.position = new Vector3(241.4f, 0.2f, 177.2f);
            rookie.transform.Rotate(0, 180, 0);
            unClick = false;
            CNTdrag.elegida = true;
            Destroy(GetComponent<Rookie>());
            Destroy(GameObject.Find("Seleccion De cartas Scroll"));
            //GameObject.Find("Deck").GetComponent<Deck>().RobarEspecifico("");
        }
    }
    private void OnMouseUp()
    {
    }
    // Use this for initialization
    void Start()
    {
        CNTdrag.elegida = false;
        btn.onClick.AddListener(OnMouseDown);
    }

    // Update is called once per frame
    void Update()
    {
        if (unClick)
        {
            if ((Time.time - tiempoParaElDobleClick) > delayDelDobleClick)
            {
                unClick = false;
            }
        }
        if (CNTdrag.elegida)
        {
            {
                //al deck pero como es un arreglo las borro
                Destroy(gameObject, 0);
            }

        }
    }
}
