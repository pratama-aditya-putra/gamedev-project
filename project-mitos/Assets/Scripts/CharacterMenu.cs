using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text fields
    public Text levelText, hitpointText, pesosText, upgradeCost, xpText;

    //Logic field
    private int currentCharacterSelection = 0;
    public Image characterSelectionSPrite;
    public Image weaponSprite;
    public RectTransform xpBar;

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
        weaponSprite.sprite = GameManager.instance.weaponSprites[0];
        upgradeCost.text = "NOT IMPLEMENTED";

        //Meta
        levelText.text = "NOT IMPLEMENTED";
        hitpointText.text = GameManager.instance.player.hitPoints.ToString();
        pesosText.text = GameManager.instance.peso.ToString();

        //Xp bar
        xpText.text = "NOT IMPLEMENTED";
        xpBar.localScale = new Vector3(0.8f, 1, 1);
    }
}
