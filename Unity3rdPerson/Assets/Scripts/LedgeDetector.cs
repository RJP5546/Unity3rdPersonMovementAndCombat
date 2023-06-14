using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnLedgeDetect;//stores the edge forwards vector and player closest point
    private void OnTriggerEnter(Collider other)
    {
        OnLedgeDetect?.Invoke(other.transform.forward, other.ClosestPointOnBounds(transform.position));
        //Gets the rotation of the ledge so the player faces the correct direction
        //ClosestPointOnBounds will get the closest point on the edge of the collider to the transform.position (the players hands)
    }
}
