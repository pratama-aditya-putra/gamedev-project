using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    //Damage structure

    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7};
    public float[] pushForce = { 2.0f, 2.4f, 2.8f, 3.0f, 3.2f, 3.5f, 3.7f };

    //Weapon level
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //Swing mechanic
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter")
        {
            if (coll.name == "Player")
                return;

            //Create a new damage object and then send it to the collided object
            Damage dmg = new Damage() {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        //Change stats

    }
}
