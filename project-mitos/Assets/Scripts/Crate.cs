using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    private BoxCollider2D boxCollider2D;
    private ContactFilter2D filter;
    private Collider2D[] temp = new Collider2D[10];
    protected virtual void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        if(temp != null)
        {
            //Collison mechanic
            boxCollider2D.OverlapCollider(filter, temp);
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == null)
                    continue;

                OnCollide(temp[i]);

                temp[i] = null;
            }
        }

    }

    protected virtual void OnCollide(Collider2D coll)
    {
        if (coll.name == "weapon")
            Physics2D.IgnoreCollision(boxCollider2D, coll, true);
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }
}
