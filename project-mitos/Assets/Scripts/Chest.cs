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
            GameManager.instance.peso += pesoAmount;
            GameManager.instance.ShowText("+" + pesoAmount + " pesos!",25,Color.yellow,transform.position,Vector3.up * 50,0.8f);
        }
    }
}
