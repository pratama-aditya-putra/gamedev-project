using UnityEngine;

public class Portal : Collidable
{
    public string sceneName;
    protected override void OnCollide(Collider2D coll)
    {
        if(coll.name == "Player" || coll.name == "Jaka")
        {
            //Teleport Player
            GameManager.instance.SaveState();
            if (gameObject.name == "PortalMain")
            {
                sceneName = "Dungeon1";
            }
            if (gameObject.name == "PortalDungeon1")
            {
                sceneName = "MainDungeon";
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
