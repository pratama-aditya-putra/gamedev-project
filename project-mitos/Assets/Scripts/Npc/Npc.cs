using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public DialogueTrigger trigger;
    public Block block;
    /*
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") == true)
                trigger.StartDialogue();
        }*/
    public void Talked()
    {
        block.openBlock();
    }

    public void Update()
    {
        if (DialogueManager.isEnd == true)
            Talked();
    }
}
