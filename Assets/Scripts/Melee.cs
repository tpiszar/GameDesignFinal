using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float range = 100f;
    public Collider collide;

    public float impact;
    public int meleeDmg;

    List<Collider> hits = new List<Collider>();

    bool next = true;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //collide = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && next)
        {
            next = false;
            hits.Clear();
            animator.SetTrigger("Melee");
        }
    }

    public void NextAttk()
    {
        next = true;
    }

    void enableCollider()
    {
        collide.enabled = true;
    }

    void disableCollider()
    {
        collide.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || hits.Contains(other))
        {
            return;
        }
        hits.Add(other);
        FlyingEnemy flyEn = other.GetComponent<FlyingEnemy>();
        if (flyEn)
        {
            flyEn.knockBack(impact, transform.parent.position);
            flyEn.TakeDamage(meleeDmg);
        }
        else
        {
            GroundEnemy groEn = other.GetComponentInParent<GroundEnemy>();
            if (groEn)
            {
                groEn.knockBack(impact, transform.parent.position);
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
