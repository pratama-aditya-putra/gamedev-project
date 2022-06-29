using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleItem : Colletible
{
    public Item item;

   /* protected override void Start()
    {
        base.Start();
        GetComponent<SpriteRenderer>().sprite = item.GetComponent<Image>().sprite;
    }*/

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GameManager.instance.inventory.AddItem(item);
            GameManager.instance.ShowText("+ " + item.itemName, 25, Color.yellow, transform.position, Vector3.up * 30, 0.8f);
            Destroy(gameObject);
        }
    }
}
