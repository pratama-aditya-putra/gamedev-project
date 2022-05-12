using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        JakaController controller = other.GetComponent<JakaController>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
