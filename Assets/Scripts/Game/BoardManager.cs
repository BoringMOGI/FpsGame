using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] TargetBoard prefab;
    [SerializeField] float radiusX;
    [SerializeField] float radiusZ;
    [SerializeField] float createTime;      // 타겟의 생성 주기.
    [SerializeField] int maxTargetCount;    // 최대 생성 개수.

    bool isStart = false;

    float timer = 0.0f;                 // 몇 초까지 흘렀는지 기록하는 변수.
    int createCount = 0;                // 몇 개까지 만들었는지 기록하는 변수.

    private void Update()
    {
        if (!isStart)
            return;

        timer += Time.deltaTime;        // timer의 값을 시간의 흐름에 따라 더한다.
        if(timer >= createTime)         // timer의 값이 특정 시간이 되었으면
        {
            CreateBoard();              // 타겟을 랜덤한 위치에 생성.
            timer = 0.0f;               // timer값을 초기화.

            createCount++;              // 생성 카운트 1 증가.
            if(createCount >= maxTargetCount)
            {
                isStart = false;
                createCount = 0;
            }
        }
    }

    public void OnStartGame()
    {
        if (isStart)            // 이미 시작 중이라면.
            return;             // 리턴한다.

        isStart = true;

        createCount = 0;
        timer = 0.0f;
    }


    void CreateBoard()
    {
        // 타겟의 위치 계산.
        Vector3 position = transform.position;
        position.x += Random.Range(-radiusX, radiusX);
        position.z += Random.Range(-radiusZ, radiusZ);

        // 타겟 생성 및 위치,회전 값 대입.
        TargetBoard newTarget = Instantiate(prefab);
        newTarget.transform.position = position;
        newTarget.transform.eulerAngles = Vector3.zero;

        // 타겟의 Setup함수 호출.
        newTarget.Setup(1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(radiusX * 2f, 0.2f, radiusZ * 2f));
    }
}
