using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(transition);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("Awake");
    }

    private bool paused;

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> expTable;
    public Inventory inventory;

    //References
    public Player player;
    public Weapon weapon;
    public Animator deathMenuAnim;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitPointBar;
    public RectTransform manaPointBar;
    public GameObject hud;
    public GameObject menu;
    public GameObject transition;
    public Button pauseButton;
    public bool alive;

    public string deadEnemies = "";
    public string collectedItems = "";

    //Floating text manager
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg,  fontSize,  color,  position,  motion,  duration);
    }

    //Try update weapon
    public bool TryUpdateWeapon()
    {
        //Is weapon is the max level?
        if(weaponPrices.Count <= weapon.weaponLevel)
            return false;
        if (peso >= weaponPrices[weapon.weaponLevel])
        {
            peso -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            Debug.Log("Berhasil upgrade senjata");
            return true;
        }

        return false;
    }

    //Hitpoint bar
    public void OnHitPointChange()
    {
        float ratio = (float)player.hitPoints / (float)player.maxHitpoints;
        hitPointBar.localScale = new Vector3(ratio,1,1);
    }

    //Manapoint bar
    public void OnManaPointChange()
    {
        float ratio = (float)player.Mana / (float)player.maxMana;
        manaPointBar.localScale = new Vector3(ratio, 1, 1);
    }

    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;
        
        while (experience >= add)
        {
            add += expTable[r];
            r++;

            if(r == expTable.Count)//Max level
            {
                return r;
            }
        }

        return r;
    }

    public int GetXptoLevel(int level)
    {
        int r = 0;
        int xp = 0;
        while (r < level)
        {
            xp += expTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if(currentLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }

    public void OnLevelUp()
    {
        player.OnLevelUp();
        GameManager.instance.OnHitPointChange();
        GameManager.instance.OnManaPointChange();
    }

    //Logic
    public int peso;
    public int experience;

    //On scene loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    public void PauseGame()
    {
        pauseButton.interactable = false;
        Time.timeScale = 0.0f;

    }
    public void ResumeGame()
    {
        pauseButton.interactable = true;
        Time.timeScale = 1.0f;
    }
    public void QuitGame()
    {
        GameManager.instance.SaveState();
        Application.Quit();
    }

    public void UsePotion()
    {
        if (inventory.potion.itemId == 2001)
            player.Heal(3);
        if (inventory.potion.itemId == 2002)
            player.Mana += 2;
        inventory.potion.amount--;
        if (inventory.potion.amount <= 0)
        {
            inventory.potion.itemId = 0;
            inventory.potion.itemName = "";
            inventory.potion.amount = 0;
            inventory.potion.gameObject.SetActive(false);
            //menu.GetComponent<CharacterMenu>().UpdateMenu();
        }
        return;
    }

    //Death menu & respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        Time.timeScale = 1.0f;
        transition.GetComponent<Animator>().SetTrigger("In");
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Jungle" || scene.name == "Jungle 1")
            UnityEngine.SceneManagement.SceneManager.LoadScene("Jungle");
        else if (scene.name == "MainDungeon" || scene.name == "Dungeon1")
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainDungeon");
        else if (scene.name == "Underwater 1" || scene.name == "Underwater")
            UnityEngine.SceneManagement.SceneManager.LoadScene("Underwater");
        player.Respawn();
    }

    public void SaveState()
    {
        transition.GetComponent<Animator>().SetTrigger("Out");

        string s = "";

        s += "0" + "|";
        //s += peso.ToString() + "|";
        //s += experience.ToString() + "|";
        //s += weapon.weaponLevel.ToString() + "|";
        s += "1000|";
        s += "500|";
        s += "6|";
        foreach (Item item in inventory.GetItemList())
        {
            s += item.itemId.ToString();
            s += item.amount.ToString() + "|";
        }

        string temp;
        if(deadEnemies != "")
        {
            if (!PlayerPrefs.HasKey("DeadEnemies"))
                PlayerPrefs.SetString("DeadEnemies", deadEnemies);
            else
            {
                //Debug.Log(PlayerPrefs.GetString("DeadEnemies"));
                temp = PlayerPrefs.GetString("DeadEnemies");
                deadEnemies = temp + deadEnemies;
                PlayerPrefs.SetString("DeadEnemies", deadEnemies);
                deadEnemies = "";
            }
        }

        string temp1;
        if (collectedItems != "")
        {
            if (!PlayerPrefs.HasKey("CollectedItems"))
                PlayerPrefs.SetString("CollectedItems", collectedItems);
            else
            {
                //Debug.Log(PlayerPrefs.GetString("CollectedItems"));
                temp1 = PlayerPrefs.GetString("CollectedItems");
                collectedItems = temp1 + collectedItems;
                PlayerPrefs.SetString("CollectedItems", collectedItems);
                collectedItems = "";
            }
        }
        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState");
    }
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        Debug.Log("Loaded");
        //PlayerPrefs.SetString("DeadEnemies", "");
        //deadEnemies = PlayerPrefs.GetString("DeadEnemies");
        //Debug.Log(PlayerPrefs.GetString("DeadEnemies"));
        //PlayerPrefs.SetString("CollectedItems", "");
        //collectedItems = PlayerPrefs.GetString("CollectedItems");
        // Debug.Log(PlayerPrefs.GetString("CollectedItems"));
        deadEnemies = "";
        collectedItems = "";
        transition.GetComponent<Animator>().SetTrigger("In");
        string temp = "";
        SceneManager.sceneLoaded -= LoadState;
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //
        peso = int.Parse(data[1]);

        //Experience logic
        experience = int.Parse(data[2]);
        player.SetLevel(GetCurrentLevel());

        //Change weapon skin
        weapon.SetWeaponLevel(int.Parse(data[3]));

        //Inventory
        int itemId;
        int itemAmount;
        if(data.Length >= 4)
        {
            for (int i = 4; i < data.Length; i += 4)
            {
                itemId = int.Parse(data[i].Substring(0, 4));
                itemAmount = int.Parse(data[i].Substring(4, 1));
                inventory.AddItem(new Item { itemId = itemId, amount = itemAmount, itemName = inventory.GetItemName(itemId) });
                Debug.Log(itemAmount + " " + itemId);
            }
        }

        /*
        foreach (Item item in itemsPrefabs)
        {
            if (item.itemId == int.Parse(data[5]))
            {
                potion = item;
                Debug.Log("Potion = " + potion.itemId.ToString());
            }
        }*/

        Debug.Log("LoadState Weapon Level = " + int.Parse(data[3]));

    }
}
