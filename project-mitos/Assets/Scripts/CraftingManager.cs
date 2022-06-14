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
                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                nearestSlot.item = currentItem;
                itemLists[nearestSlot.index] = currentItem;
                currentItem = null;

                CheckForCreatedRecipes();
            }
        }
    }

    public void CheckForCreatedRecipes()
    {
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

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

    public void OnClickSlot(Slot slot)
    {
        slot.item = null;
        itemLists[slot.index] = null;
        slot.gameObject.SetActive(false);
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
