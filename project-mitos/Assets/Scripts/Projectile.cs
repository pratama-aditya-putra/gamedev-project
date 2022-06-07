using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Collidable
{
    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (transform.position.magnitude > 10.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2D.AddForce(direction * force);
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
                Destroy(gameObject);

        Debug.Log("Projectile Collision with " + coll.gameObject);

        //Create a new damage object and then send it to the collided object
        /*Damage dmg = new Damage()
        {
            damageAmount = damagePoint[weaponLevel],
            origin = transform.position,
            pushForce = pushForce[weaponLevel]
        };

        coll.SendMessage("ReceiveDamage", dmg);
    }*/
    }


    /*void OnCollisionEnter2D(Collision2D other)
    {
        MusuhController enemy = other.gameObject.GetComponent<MusuhController>();

        if (enemy != null)
        {
            enemy.ChangeHealth(-1);
        }

        Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);
    }*/
}
