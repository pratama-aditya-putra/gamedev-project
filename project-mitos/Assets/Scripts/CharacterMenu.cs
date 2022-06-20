using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text fields
    public Text levelText, hitpointText, pesosText, upgradeCost, xpText, manapointText;

    //Logic field
    private int currentCharacterSelection = 0;
    public Image characterSelectionSPrite;
    public Image weaponSprite;
    public RectTransform xpBar;
    public List<Item> inventoryItem;
    public List<Text> inventoryItemAmount;
    private Item currentItem;
    public Image costumCursor;
    public Slot potionSlots;
    public Text potionAmount;

    private void Start()
    {
        UpdateMenu();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentItem != null)
            {
                costumCursor.gameObject.SetActive(false);
                if(currentItem.itemId > 2000)
                {
                    Slot nearestSlot = null;
                    nearestSlot = potionSlots;
                    /*tempObject.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                    tempItem.itemName = currentItem.itemName;
                    tempItem.itemId = currentItem.itemId;
                    tempItem.amount = currentItem.amount;*/
                    nearestSlot.item.gameObject.SetActive(true);
                    nearestSlot.item.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                    if (nearestSlot.item.itemId != 0)
                    {
                        if (nearestSlot.item.itemId == currentItem.itemId)
                            nearestSlot.item.amount += currentItem.amount;
                    }
                    else if(nearestSlot.item.itemId == 0)
                    {
                        nearestSlot.item.itemId = currentItem.itemId;
                        nearestSlot.item.itemName = currentItem.itemName;
                        nearestSlot.item.amount = currentItem.amount;
                        currentItem.gameObject.SetActive(false);
                    }
                    potionAmount.text = nearestSlot.item.amount.ToString();
                    GameManager.instance.SetPotion(nearestSlot.item);

                    for(int i=0;i< currentItem.amount; i++)
                        GameManager.instance.RemoveItem(nearestSlot.item);
                    UpdateMenu();
                    currentItem = null;
                }
            }
        }
    }

    public void OnClickSlot(Slot slot)
    {
        GameManager.instance.AddItem(slot.item);
        slot.item.itemId = 0;
        slot.item.itemName = "";
        slot.item.amount = 0;
        slot.item.gameObject.SetActive(false);
        GameManager.instance.potion.itemId = 0;
        GameManager.instance.potion.itemName = "";
        GameManager.instance.potion.amount = 0;
        UpdateMenu();
    }

    public void OnMouseDownItem(Item item)
    {
        if (currentItem == null)
        {
            currentItem = item;
            costumCursor.gameObject.SetActive(true);
            costumCursor.sprite = currentItem.GetComponent<Image>().sprite;
        }
    }

    //Weapon Upgrade 
    public void OnClickUpgrade()
    {
        if(GameManager.instance.TryUpdateWeapon())
        {
            UpdateMenu();
        }
    }

    //Update character information
    public void UpdateMenu()
    {
        //Item mechanic
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

        if(GameManager.instance.potion != null)
        {
            if(GameManager.instance.potion.itemId != 0)
                potionSlots.item.amount = GameManager.instance.potion.amount;
        }
        else
        {
            potionSlots.item.itemId = 0;
            potionSlots.item.amount = 0;
            potionSlots.item.itemName = "";
            potionSlots.item.gameObject.SetActive(false);
        }

        //Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCost.text = "MAX";
        else
            upgradeCost.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        //Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitPoints.ToString() + " / " + GameManager.instance.player.maxHitpoints.ToString();
        manapointText.text = GameManager.instance.player.Mana.ToString() + " / " + GameManager.instance.player.maxMana.ToString();
        pesosText.text = GameManager.instance.peso.ToString();

        //Xp bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (currLevel == GameManager.instance.expTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " Total experience points!"; // Display total xp
            xpBar.localScale = Vector3.one;
        }
        else
        {   
            int prevLevelXp = GameManager.instance.GetXptoLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXptoLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel + " / " + diff;
        }
    }
}
