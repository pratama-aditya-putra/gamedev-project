using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> itemList;
    public List<Sprite> itemsIcon;
    public Item potion;

    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item{itemId = 1001, amount = 1, itemName = "Beras" });
        AddItem(new Item { itemId = 1002, amount = 3, itemName = "Brotowali" });
        AddItem(new Item { itemId = 1003, amount = 1, itemName = "Cabai" });
        AddItem(new Item { itemId = 2001, amount = 1, itemName = "Jamu Beras" });
    }


    //Add and remove item
    public void AddItem(Item newItem)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemId == newItem.itemId)
            {
                itemList[i].amount += newItem.amount;
                return;
            }
        }
        itemList.Add(newItem);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void RemoveItem(Item redItem)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemId == redItem.itemId)
            {
                if (itemList[i].amount > 1)
                {
                    itemList[i].amount--;
                    return;
                }
                else
                {
                    itemList.RemoveAt(i);
                }
            }
        }
    }

    public Sprite GetItemIcon(int itemId)
    {
        if (itemId < 2000)
            return itemsIcon[itemId - 1001];
        else
            return itemsIcon[itemId - 2001 + 3];
    }

}
