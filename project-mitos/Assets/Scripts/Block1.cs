using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block1 : MonoBehaviour
{
    public GameObject blocker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Talked()
    {
        blocker.SetActive(false);
    }
}
