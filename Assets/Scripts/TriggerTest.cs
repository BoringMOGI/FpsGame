using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger : " + other.name);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision : " + collision.gameObject.name);
    }

}
