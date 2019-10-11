using UnityEngine;

public class BattlelPhase : MonoBehaviour
{
    public GameObject[] DigimonPrefabs;
    public GameObject[] DigimonWireframes;
    public Material DigimonShow;
    public ParticleSystem Aparecer;
    public Transform Espacio;
    // Use this for initialization
    public GameObject DigimonCombate;
    public GameObject DigimonReposo;
    void Start()
    {
        // Aparecer.Stop();
        // Comenzar(0, "");
    }
    public void Comenzar(int IDigimon, string Ataque)
    {
        Aparecer.Play();
        // Creamos las copias 
        DigimonCombate = Instantiate(DigimonPrefabs[IDigimon], Espacio);
        DigimonCombate.GetComponent<DigimonModel>().Cuerpo.GetComponent<SkinnedMeshRenderer>().enabled = false;
        DigimonReposo = Instantiate(DigimonPrefabs[IDigimon], Espacio);
        // ADIGNAMOS MATERIAL ESPECIAL
        DigimonReposo.GetComponent<DigimonModel>().Cuerpo.GetComponent<Renderer>().material = DigimonShow;
        Invoke("Aparicion", 3f);
    }
    public void Aparicion()
    {
        DigimonCombate.GetComponent<DigimonModel>().Cuerpo.GetComponent<SkinnedMeshRenderer>().enabled = true;
        Destroy(DigimonReposo, 1f);
    }
}

