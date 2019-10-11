using UnityEngine;

public class SpawnEffect : MonoBehaviour
{

    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;
    public float cantidad;

    float timer = 0;
    public Renderer _renderer;

    int shaderProperty;


    void Update()
    {
        if (timer < spawnEffectTime + pause)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        cantidad = fadeIn.Evaluate(Mathf.InverseLerp(-1, spawnEffectTime, timer));

        _renderer.material.SetFloat("_DeltaTime", cantidad);

    }
}
