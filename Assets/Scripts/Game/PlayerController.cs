using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] WeaponController weapon;

    [SerializeField] Camera eye;
    [SerializeField] Transform originEyePivot;

    
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            weapon.Fire();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            weapon.Realod();
        }

        Aim();
    }

    private void Aim()
    {
        // 오른쪽 버튼을 눌렀을 때. 에임하면서 카메라의 위치도 변경.
        if(Input.GetMouseButtonDown(1))
        {
            weapon.Aim(true);

            eye.transform.position = weapon.AimCameraPivot.position;
            eye.fieldOfView -= 10;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            weapon.Aim(false);
            
            eye.transform.position = originEyePivot.position;
            eye.fieldOfView += 10;
        }
    }
}
