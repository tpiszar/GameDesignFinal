using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour {

    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;

    public ParticleSystem ps;
    public float timer = 0;
    Renderer _renderer;

    int shaderProperty;

    public bool oneTime;
    public bool particles;

	void Start ()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();

        if (particles)
        {
            if (!ps)
            {
                ps = GetComponentInChildren <ParticleSystem>();
            }
            var main = ps.main;
            main.duration = spawnEffectTime;
            ps.Play();
        }
    }
	
	void Update ()
    {
        if (timer < spawnEffectTime + pause)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (oneTime)
            {
                Destroy(this);
            }
            else
            {
                if (particles)
                {
                    ps.Play();
                }
                timer = 0;
            }
        }


        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate( Mathf.InverseLerp(0, spawnEffectTime, timer)));
        
    }
}
