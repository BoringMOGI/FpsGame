using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] WeaponController weapon;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            weapon.Fire();
        }
    }
}
