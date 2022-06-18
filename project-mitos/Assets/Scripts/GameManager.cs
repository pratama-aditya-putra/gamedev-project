using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    private bool paused;

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> expTable;
    public List<Item> itemsPrefabs;
    public List<Item> items;

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
    public bool alive;

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

    //Add and remove item
    public void AddItem(Item newItem)
    {
        for(int i=0;i < items.Count;i++)
        {
            if (items[i].itemId == newItem.itemId)
            {
                items[i].amount++;
                return;
            }
        }
        items.Add(newItem);
    }

    public void RemoveItem(Item redItem)
    {
        for(int i = 0; i < items.Count;i++)
        {
            if (items[i].itemId == redItem.itemId)
            {
                if (items[i].amount > 1)
                {
                    items[i].amount--;
                    return;
                }
                else
                {
                    items.RemoveAt(i);
                }
            }
        }
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

    public void QuitGame()
    {
        Application.Quit();
    }

    //Death menu & respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        transition.GetComponent<Animator>().SetTrigger("In");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainDungeon");
        player.Respawn();
    }

    public void SaveState()
    {
        transition.GetComponent<Animator>().SetTrigger("Out");

        string s = "";

        s += "0" + "|";
        s += peso.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString() + "|";
        foreach(Item item in items)
        {
            s += item.itemId.ToString();
        }
        s += "|";

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState");
    }
    public void LoadState(Scene s, LoadSceneMode mode)
    {
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
        for(int i = 0; i < data[4].Length; i += 4)
        {
            temp += data[4][i] + data[4][i+1] + data[4][i + 2] + data[4][i + 3];
            foreach(Item item in itemsPrefabs)
            {
                if(item.itemId == int.Parse(temp))
                {
                    AddItem(item);
                }
            }
        }

        Debug.Log("LoadState Weapon Level = " + int.Parse(data[3]));

    }
}
