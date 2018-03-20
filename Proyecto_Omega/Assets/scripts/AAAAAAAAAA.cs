using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AAAAAAAAAA : MonoBehaviour {
    public Button btn;
    private void Start()
    {
        btn.onClick.AddListener(Asd);
    }

    void Asd()
    {
        StaticRules.SiguienteFase();
        Debug.Log(StaticRules.NowPhase);
    }
}
