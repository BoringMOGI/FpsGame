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

        // ���� �ʱⰪ ����.
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
        // ������ ��ư�� ������ ��. �����ϸ鼭 ī�޶��� ��ġ�� ����.
        if (Input.GetMouseButtonDown(1))
        {
            weapon.Aim(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            weapon.Aim(false);
        }

        // ���콺 ������ Ű �Է¿� ���� ī�޶� ��ġ�� ������ �ٲ��.
        Vector3 eyeDestination = (Input.GetMouseButton(1)) ?
            weapon.AimCameraPivot.position : originEyePivot.position;

        // Lerp�� �̿��� ���� �� ��ġ�� ī�޶��� ��ġ�� ����.
        eye.transform.position =
            Vector3.Lerp(eye.transform.position, eyeDestination, 8f * Time.deltaTime);

        // ���� �þ߰�.
        float fov = originFOV + (Input.GetMouseButton(1) ? -10 : 0);
        eye.fieldOfView = Mathf.Lerp(eye.fieldOfView, fov, 8f * Time.deltaTime);
    }
}
