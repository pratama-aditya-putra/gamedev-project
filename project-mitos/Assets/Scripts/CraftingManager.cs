using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    private Item currentItem;
    public Image costumCursor;

    public Slot[] craftingSlots;

    public List<Item> itemLists;
    public string[] recipes;
    public Item[] recipesResults;
    public Slot resultSlot;
    public List<Item> inventoryItem;
    public List<Text> inventoryItemAmount;

    private void Start()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        //Item mechanic for 
        for (int i = 0; i < 18; i++)
        {
            inventoryItem[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < GameManager.instance.items.Count; i++)
        {
            inventoryItem[i].GetComponent<Image>().sprite = GameManager.instance.items[i].GetComponent<Image>().sprite;
            inventoryItem[i].itemName = GameManager.instance.items[i].itemName;
            inventoryItem[i].itemId = GameManager.instance.items[i].itemId;
            inventoryItem[i].amount = GameManager.instance.items[i].amount;
            inventoryItemAmount[i].text = inventoryItem[i].amount.ToString();
            inventoryItem[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(currentItem != null)
            {
                costumCursor.gameObject.SetActive(false);
                Slot nearestSlot = null;
                float shortestDistance = float.MaxValue;

                foreach(Slot slot in craftingSlots)
                {
                    float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);

                    if(dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearestSlot = slot;
                    }
                }
                GameObject tempObject = Instantiate(currentItem.gameObject);
                /*tempObject.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                tempItem.itemName = currentItem.itemName;
                tempItem.itemId = currentItem.itemId;
                tempItem.amount = currentItem.amount;*/
                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                nearestSlot.item = tempObject.gameObject.GetComponent<Item>();
                itemLists[nearestSlot.index] = tempObject.gameObject.GetComponent<Item>();
                CheckForCreatedRecipes();
                currentItem = null;

                GameManager.instance.RemoveItem(nearestSlot.item);
                UpdateInventory();
            }
        }
    }

    public void CheckForCreatedRecipes()
    {
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

        for(int i = 0; i < 4; i++)
        {
            itemLists[i] = craftingSlots[i].item;
        }

        string currentRecipeString = "";
        foreach(Item item in itemLists)
        {
            if (item != null)
                currentRecipeString += item.itemName;
            else
                currentRecipeString += "null";
        }

        for(int i=0;i < recipes.Length; i++)
        {
            if (recipes[i] == currentRecipeString)
            {
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipesResults[i].GetComponent<Image>().sprite;
                resultSlot.item = recipesResults[i];
            }
        }

        Debug.Log(currentRecipeString);
    }

    public void OnClickResult(Slot slot)
    {
        GameManager.instance.AddItem(slot.item);
        slot.item = null;
        for(int i = 0; i < 4; i++)
        {
            Item temp = new Item();
            temp.itemId = 0;
            temp.itemName = "";
            temp.amount = 0;
            itemLists[i] = null;
            craftingSlots[i].item = temp;
            craftingSlots[i].gameObject.SetActive(false);
        }
        slot.gameObject.SetActive(false);
        GameManager.instance.SaveState();
        UpdateInventory();
    }

    public void OnClickSlot(Slot slot)
    {
        GameManager.instance.AddItem(slot.item);
        slot.item = null;
        itemLists[slot.index] = null;
        slot.gameObject.SetActive(false);
        UpdateInventory();
        CheckForCreatedRecipes();
    }

    public void OnMouseDownItem(Item item)
    {
        if(currentItem == null)
        {
            Debug.Log("Clicked");
            currentItem = item;
            costumCursor.gameObject.SetActive(true);
            costumCursor.sprite = currentItem.GetComponent<Image>().sprite;
        }
    }
}
