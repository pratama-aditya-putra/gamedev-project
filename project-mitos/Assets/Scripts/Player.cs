using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    float horizontal;
    float vertical;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
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
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        UpdateMotor(new Vector3(x, y, 0));
    }
}
