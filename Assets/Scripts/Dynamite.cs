using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public float explodeTime;
    float timer = 0;

    public float damagePerc;
    public float range;

    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > explodeTime)
        {
            GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity);
            explode.transform.localScale = Vector3.one * 3.5f;

            Collider[] cols = Physics.OverlapSphere(transform.position, range);
            foreach (Collider col in cols)
            {
                GroundEnemy GrEn = col.gameObject.GetComponentInParent<GroundEnemy>();
                if (GrEn)
                {
                    GrEn.knockBack(100, transform.position, 1);
                    GrEn.TakeDamage((int)(GrEn.maxHealth * damagePerc));

                }
                else
                {
                    FlyingEnemy FlyEn = col.gameObject.GetComponentInParent<FlyingEnemy>();
                    if (FlyEn)
                    {
                        FlyEn.knockBack(100, transform.position, 1);
                        FlyEn.TakeDamage((int)(FlyEn.maxHealth * damagePerc));
                    }
                    else
                    {
                        Rock rock = col.gameObject.GetComponent<Rock>();
                        if (rock)
                        {
                            rock.Hit((int)(rock.maxHealth * 0.75f));
                        }
                    }
                }

                Destroy(this.gameObject);
            }
        }
    }
}
