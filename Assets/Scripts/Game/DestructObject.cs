using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructObject : MonoBehaviour
{
    [SerializeField] GameObject destructObject;

    public void OnDestruct()
    {
        Instantiate(destructObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }


}
