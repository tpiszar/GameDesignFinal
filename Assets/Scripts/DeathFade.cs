using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFade : MonoBehaviour
{
    public SkinnedMeshRenderer[] meshes;
    public SpawnEffect[] spawnEffs;
    public Material[] fadeMats;

    public float fadeDelay;
    float timer = 0;

    public float fadeDur;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = fadeMats[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > fadeDelay)
        {
            foreach (SpawnEffect eff in spawnEffs)
            {
                eff.spawnEffectTime = fadeDur;
                eff.enabled = true;
            }
            Destroy(gameObject, fadeDur);
            Destroy(this);
        }
    }
}
