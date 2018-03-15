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

    public static void SetOptionSlot(GameObject Carta , bool Campo=false)
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
                Debug.Log(dist + " " + item.name);
                if (dist < 25)
                {
                    Carta.transform.parent = item;
                    Carta.GetComponent<CartaDigimon>().AjustarSlot();
                }
            }
        }
    }
}
