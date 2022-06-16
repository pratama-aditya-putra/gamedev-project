using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    public GameObject Object;
    protected override void Death()
    {
        Destroy(gameObject);
        Object.SetActive(true);
    }
}
