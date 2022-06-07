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
    public GameObject crosshair;
    public float CROSSHAIR_DISTANCE = 1.0f;
    BoxCollider2D projectileCollider;

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
    }
    public void Respawn()
    {
        Heal(maxHitpoints);
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
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            Launch();
        }

        Aim();

    }
    private void FixedUpdate()
    {
        if (DialogueManager.isActive == true) return;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void OnLevelUp()
    {
        maxHitpoints += 2;
        hitPoints = maxHitpoints;
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
        Vector2 shootingDirection = crosshair.transform.localPosition;

        GameObject projectileObject = Instantiate(projectilePrefab, transform.position + new Vector3(lookDirection.x, lookDirection.y, 0) * 0.2f, Quaternion.identity);
        projectileObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("weapon");
        projectileCollider = projectileObject.GetComponent<BoxCollider2D>();
        projectileCollider.size = new Vector2(0.2155424f, 0.08285652f);
        projectileCollider.offset = new Vector2(0.005057238f, 0.00119327f);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        projectile.Launch(lookDirection, 40);
        Debug.Log(projectileObject.layer.ToString());
    }

    void Aim()
    {
        if (lookDirection != Vector2.zero)
        {
            crosshair.transform.localPosition = lookDirection * CROSSHAIR_DISTANCE;
        }
    }
}
