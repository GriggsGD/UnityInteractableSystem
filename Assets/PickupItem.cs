using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public void Pickup()
    {
        Destroy(gameObject);
    }
}
