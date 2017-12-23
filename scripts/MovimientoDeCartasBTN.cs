/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoDeCartasBTN : MonoBehaviour {
    public float anchoCartaW = 0;
    public float altoCartaH = 0;
    public GameObject modeloDeLaCarta;
    private float anchoTerreno = 500;
    private float altoTerreno = 500;
    private float profundidad = 300;

    private GameObject instanciaCarta;
    private float espacio = 0;
    private Texture[] arrayRookie;
    private int j = 0, n = 2;
    private float centroW;
    private float centroH;
    // Use this for initialization
    public void CalculaRookie()
    {
        //realmente debe revisar los que son rookie y meterlos en arrayrookie
        arrayRookie = CNTdrag.arrayTextura;
        //
        try
        {
            if (arrayRookie.Length % 3 == 0)
            {
                CNTdrag.totalPagRookie = (arrayRookie.Length / 3) - 1;
                CNTdrag.actualPagRookie = 0;
                CNTdrag.movimientoPagRookie = 0;
            }
            else
            {
                CNTdrag.totalPagRookie = arrayRookie.Length / 3;
                CNTdrag.actualPagRookie = 0;
                CNTdrag.movimientoPagRookie = 0;
            }
        }
        catch (System.NullReferenceException)
        {

        }
    }
    void Start () {
        centroW = (anchoTerreno / -2) + 60;
        centroH = altoTerreno / -2;
        //se calculan los rookie
        CalculaRookie();
        CambioPagina();
    }
	
    void CambioPagina()
    {
        try
        {
            for (int i = j; i <= n; i++)
            {
                if (arrayRookie[i])
                {
                    instanciaCarta = Instantiate(modeloDeLaCarta, new Vector3(), Quaternion.identity);
                    instanciaCarta.gameObject.GetComponent<Renderer>().material.mainTexture = arrayRookie[i];
                    instanciaCarta.name = "carta" + i;
                    instanciaCarta.transform.Rotate(90, 180, 0);
                    instanciaCarta.transform.Translate(centroW, centroH, -profundidad);
                    centroW -= 60;
                }
            }
            centroW = (anchoTerreno / -2) + 60;
            centroH = altoTerreno / -2;
        }
        catch (System.IndexOutOfRangeException)
        {
            centroW = (anchoTerreno / -2) + 60;
            centroH = altoTerreno / -2;
        }
    }
    void BorrarAnteriores(int x, int y)
    {
        try
        {
            //0 - 29
            for (int k = x; k <= y; k++)
            {
                Destroy(GameObject.Find("carta" + k));
            }
        }
        catch (System.IndexOutOfRangeException e)
        {
        }
    }
	// Update is called once per frame
	void Update () {
        switch (CNTdrag.movimientoPagRookie)
        {
            case 1:
                BorrarAnteriores(j,n);
                j = n+1;
                n += 3;
                CNTdrag.actualPagRookie += 1;
                CNTdrag.movimientoPagRookie = 0;
                CambioPagina();
                break;
            case -1:
                BorrarAnteriores(j,n);
                n -= 3;
                j = n-2;
                CNTdrag.actualPagRookie -= 1;
                CNTdrag.movimientoPagRookie = 0;
                CambioPagina();
                break;
        }
	}
}
*/