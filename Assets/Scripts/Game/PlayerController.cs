using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] WeaponController weapon;

    [SerializeField] Camera eye;
    [SerializeField] Transform originEyePivot;

    float originFOV;
    bool isLockControl;     // ��Ʈ���� ���´�.


    private void Start()
    {
        originFOV = eye.fieldOfView;
        isLockControl = false;

        // ���� �ʱⰪ ����.
        weapon.Setup(eye.transform);

        CameraLook.Instance.OnMouseLock += OnSwitchLock;
    }

    void Update()
    {
        if (isLockControl)
            return;

        if(Input.GetMouseButton(0))
        {
            weapon.Fire(Input.GetMouseButtonDown(0));
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            weapon.Realod();
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            weapon.ChangeFireType();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            weapon.ThrowGrenade();
        }

        Aim();
    }

    private void OnSwitchLock(bool isLock)
    {
        isLockControl = !isLock;
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
