using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Fighter
{

    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;
    protected Vector3 moveDelta;
    protected float ySpeed = 0.75f;
    protected float xSpeed = 1;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        //Reset Move Delta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed,0);

        //Rotate sprite to direction
        if (moveDelta.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (moveDelta.x < 0)
            transform.localScale = Vector3.one;

        //Making sure we can move in this direction, by casting a box there first, if the box return null then we can move 
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {

            //Moving sprite
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);

        }
        //Making sure we can move in this direction, by casting a box there first, if the box return null then we can move 
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {

            //Moving sprite
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);

        }
    }
}