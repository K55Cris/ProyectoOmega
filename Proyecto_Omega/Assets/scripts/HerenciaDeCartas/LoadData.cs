using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigiCartas;
public class LoadData : MonoBehaviour {

    public TextAsset jsonData;
    

	public void LoadDataDigiCartas()
    {
        DataManager.instance.LoadCartas(jsonData.text);
      
    }
}
