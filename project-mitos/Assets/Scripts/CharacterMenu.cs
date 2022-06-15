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

    //Character Selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            //If no more character
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChange();
        }
        else
        {
            currentCharacterSelection++;

            //If no more character
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChange();
        }
    }

    private void OnSelectionChange()
    {
        characterSelectionSPrite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
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

        //Item mechanic
        for(int i=0;i < GameManager.instance.items.Count; i++)
        {
            inventoryItem[i].GetComponent<Image>().sprite = GameManager.instance.items[i].GetComponent<Image>().sprite;
            inventoryItem[i].itemName = GameManager.instance.items[i].itemName;
            inventoryItem[i].amount = GameManager.instance.items[i].amount;
            inventoryItemAmount[i].text = inventoryItem[i].amount.ToString();
            inventoryItem[i].gameObject.SetActive(true);
        }
    }
}
