using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject blocker;
    public Npc npc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (npc.tag == "Penyihir")
        {
            if (DialogueManager.isEnd == true)
                openBlock();
        }
    }

    public void openBlock()
    {
        blocker.SetActive(false);
    }
}
