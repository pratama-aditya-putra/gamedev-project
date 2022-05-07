using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private RaycastHit2D hit;
    private Vector3 moveDelta;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        boxCollider.size = new Vector2(0.09732852f, 0.1191749f);
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //Reset Move Delta
        moveDelta = new Vector3(x,y,0);

        //Rotate sprite to direction
        if(moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if(moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Making sure we can move in this direction, by casting a box there first, if the box return null then we can move 
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0,moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {

            //Moving sprite
            transform.Translate(0,moveDelta.y * Time.deltaTime,0);

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
