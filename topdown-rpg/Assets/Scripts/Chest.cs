using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Colletible
{
    public Sprite emptyChest;
    public int pesoAmount = 10;
    protected override void OnCollect()
    {

        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
        }
        base.OnCollect();
        Debug.Log("Grant Pesos");
    }
}
