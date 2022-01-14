using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] WeaponController weapon;

    [SerializeField] Camera eye;
    [SerializeField] Transform originEyePivot;

    float originFOV;

    LineRenderer lineRenderer;

    private void Start()
    {
        originFOV = eye.fieldOfView;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material.color = Color.green;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        // 무기 초기값 전달.
        weapon.Setup(eye.transform);
    }

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

        Vector3 linePoint = eye.transform.position + (eye.transform.forward * 50f);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, eye.transform.position);
        lineRenderer.SetPosition(1, linePoint);
    }

    private void Aim()
    {
        // 오른쪽 버튼을 눌렀을 때. 에임하면서 카메라의 위치도 변경.
        if (Input.GetMouseButtonDown(1))
        {
            weapon.Aim(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            weapon.Aim(false);
        }

        // 마우스 오른쪽 키 입력에 따라 카메라 위치의 기준이 바뀐다.
        Vector3 eyeDestination = (Input.GetMouseButton(1)) ?
            weapon.AimCameraPivot.position : originEyePivot.position;

        // Lerp를 이용해 계산된 현 위치를 카메라의 위치에 대입.
        eye.transform.position =
            Vector3.Lerp(eye.transform.position, eyeDestination, 8f * Time.deltaTime);

        // 최종 시야각.
        float fov = originFOV + (Input.GetMouseButton(1) ? -10 : 0);
        eye.fieldOfView = Mathf.Lerp(eye.fieldOfView, fov, 8f * Time.deltaTime);
    }
}
