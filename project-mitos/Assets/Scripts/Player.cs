using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    float horizontal;
    float vertical;

    public GameObject projectilePrefab;
    BoxCollider2D projectileCollider;
    private float lastLaunch;
    private float launchCooldown = 0.8f;
    private Transform parent;
    public Rigidbody2D rigidbody2d;

    //Mana mechanic
    public float maxMana = 12;
    public float Mana = 12;
    public float manaRecovery = 0.5f;
    public float manaRecoverySpeed = 1.5f;
    public float lastManaRecovery;
    public float fireballManaCost = 3.0f;


    private bool isAlive = true;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitPointChange();
    }

    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
        GameManager.instance.SaveState();
    }
    public void Respawn()
    {
        Heal(maxHitpoints);
        Mana = maxMana;
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }

    private void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Move X", lookDirection.x);
        animator.SetFloat("Speed", move.magnitude);


        if (Mana < maxMana)
        {
            if (Time.time - lastManaRecovery > manaRecoverySpeed)
            {
                lastManaRecovery = Time.time;
                Mana += manaRecovery;
                if (Mana > maxMana)
                    Mana = maxMana;
                GameManager.instance.OnManaPointChange();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && Mana > fireballManaCost)
        {
            if (Time.time - lastLaunch > launchCooldown)
            {
                OnMagicSpell(fireballManaCost);
                lastLaunch = Time.time;
                Launch();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.instance.UsePotion();
        }
    }

    protected void OnMagicSpell(float cost)
    {
        if (Mana <= 0)
            return;
        Mana -= cost;
        GameManager.instance.OnManaPointChange();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.isActive == true) return;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void OnLevelUp()
    {
        maxHitpoints += 2;
        hitPoints = maxHitpoints;
        maxMana += 3;
        Mana = maxMana;
        manaRecovery += 0.1f;
        manaRecoverySpeed -= 0.02f;
    }
    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    public void Heal(int healingAmount)
    {
        if (hitPoints == maxHitpoints)
            return;
        hitPoints += healingAmount;
        if (hitPoints > maxHitpoints)
            hitPoints = maxHitpoints;
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position,Vector3.up * 20, 0.5f);
        GameManager.instance.OnHitPointChange();
    }

    void Launch()
    {
        Vector2 direction;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);

        direction = Worldpos - transform.position;
        direction.Normalize();


        GameObject projectileObject = Instantiate(projectilePrefab, transform.position + new Vector3(direction.x, direction.y, transform.position.z) * 0.2f, Quaternion.identity);
        projectileObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("weapon");
        projectileCollider = projectileObject.GetComponent<BoxCollider2D>();
        projectileCollider.size = new Vector2(0.2155424f, 0.08285652f);
        projectileCollider.offset = new Vector2(0.005057238f, 0.00119327f);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.transform.Rotate(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        projectile.Launch(direction * 4, 15);
    }

}
