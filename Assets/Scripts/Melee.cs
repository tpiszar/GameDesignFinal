using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float range = 100f;
    public float attkRate;
    float nextAttk = 0;
    Collider collide;

    public float impact;
    public int meleeDmg;

    public Animation attkAnim;

    List<Collider> hits = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        collide = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        nextAttk += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && nextAttk >= attkRate)
        {
            collide.enabled = true;
            nextAttk = 0;
            attkAnim.Play();
            Invoke("disableCollider", attkRate);
        }
    }

    void disableCollider()
    {
        hits.Clear();
        collide.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hits.Contains(other))
        {
            return;
        }
        hits.Add(other);
        FlyingEnemy flyEn = other.GetComponent<FlyingEnemy>();
        if (flyEn)
        {
            flyEn.knockBack(impact, transform);
            flyEn.TakeDamage(meleeDmg);
        }
        else
        {
            GroundEnemy groEn = other.GetComponentInParent<GroundEnemy>();
            if (groEn)
            {
                groEn.knockBack(impact, transform);
                groEn.TakeDamage(meleeDmg);
            }
            else
            {
                Rock rock = other.GetComponentInParent<Rock>();
                if (rock)
                {
                    rock.Hit(meleeDmg);
                }
            }
        }
    }
}
