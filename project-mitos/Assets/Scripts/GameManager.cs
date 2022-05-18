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
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> expTable;

    //References
    public Player player;
    //public Weapon weapon;

    public FloatingTextManager floatingTextManager;

    //Floating text manager
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg,  fontSize,  color,  position,  motion,  duration);
    }

    //Logic
    public int peso;
    public int experience;

    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += peso.ToString() + "|";
        s += experience.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState");
    }
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //
        peso = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        //Change weapon skin
        Debug.Log("LoadState");
    }
}
