using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] TargetBoard prefab;
    [SerializeField] float radiusX;
    [SerializeField] float radiusZ;
    [SerializeField] float createTime;      // Ÿ���� ���� �ֱ�.
    [SerializeField] int maxTargetCount;    // �ִ� ���� ����.

    bool isStart = false;

    float timer = 0.0f;                 // �� �ʱ��� �귶���� ����ϴ� ����.
    int createCount = 0;                // �� ������ ��������� ����ϴ� ����.

    private void Update()
    {
        if (!isStart)
            return;

        timer += Time.deltaTime;        // timer�� ���� �ð��� �帧�� ���� ���Ѵ�.
        if(timer >= createTime)         // timer�� ���� Ư�� �ð��� �Ǿ�����
        {
            CreateBoard();              // Ÿ���� ������ ��ġ�� ����.
            timer = 0.0f;               // timer���� �ʱ�ȭ.

            createCount++;              // ���� ī��Ʈ 1 ����.
            if(createCount >= maxTargetCount)
            {
                isStart = false;
                createCount = 0;
            }
        }
    }

    public void OnStartGame()
    {
        if (isStart)            // �̹� ���� ���̶��.
            return;             // �����Ѵ�.

        isStart = true;

        createCount = 0;
        timer = 0.0f;
    }


    void CreateBoard()
    {
        // Ÿ���� ��ġ ���.
        Vector3 position = transform.position;
        position.x += Random.Range(-radiusX, radiusX);
        position.z += Random.Range(-radiusZ, radiusZ);

        // Ÿ�� ���� �� ��ġ,ȸ�� �� ����.
        TargetBoard newTarget = Instantiate(prefab);
        newTarget.transform.position = position;
        newTarget.transform.eulerAngles = Vector3.zero;

        // Ÿ���� Setup�Լ� ȣ��.
        newTarget.Setup(1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(radiusX * 2f, 0.2f, radiusZ * 2f));
    }
}
