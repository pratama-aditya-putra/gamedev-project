using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossButoIjo : Enemy
{
    public float distance = 0.4f;
    //public GameObject door;
    //public Sprite doorOpened;
    public Canvas bossHUD;
    public string setBossName;
    public Text bossName;
    public RectTransform bossHpBar;
    private bool isAlive = true;
    public Animator animator;
    public GameObject portal;
    public GameObject peti;
    public GameObject itemReward;

    private void Update()
    {
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (chasing == true)
        {
            bossHUD.gameObject.SetActive(true);
            bossName.text = setBossName;
        }
        else
        {
            bossHUD.gameObject.SetActive(false);
        }
        if (transform.position == startingPosition)
        {
            animator.ResetTrigger("Run");
            moveDelta = Vector3.zero;
        }
        if (moveDelta.x != 0 || moveDelta.y != 0)
        {
            animator.SetTrigger("Run");
            if (moveDelta.x < 0)
                transform.localScale = new Vector3(-sizeX, sizeY, 1);
        }
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);
        float ratio = (float)hitPoints / (float)maxHitpoints;
        bossHpBar.localScale = new Vector3(ratio, 1, 1);
    }

    protected override void Death()
    {
        base.Death();
        portal.gameObject.SetActive(true);
        peti.gameObject.SetActive(true);
        itemReward.gameObject.SetActive(true);
        bossHpBar.localScale = Vector3.one;
        bossHUD.gameObject.SetActive(false);
        isAlive = false;
        //door.layer = LayerMask.GetMask("Default");
        //door.GetComponent<BoxCollider2D>().enabled = false;
        //door.GetComponent<SpriteRenderer>().sprite = doorOpened;
    }
}
