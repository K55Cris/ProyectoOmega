/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosicionBTNSig : MonoBehaviour {
    public Button button;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3 ((Screen.width)*0.75f, (Screen.height)*0.15f, 0);
        button.onClick.AddListener(OnMouseDown);
	}

    private void OnMouseDown()
    {
        CNTdrag.movimientoPagRookie = 1;
    }
    // Update is called once per frame
    void Update () {
        if (CNTdrag.actualPagRookie == CNTdrag.totalPagRookie)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
*/