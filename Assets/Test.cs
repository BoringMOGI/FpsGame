using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private void OnEnable()
    {
        Debug.Log("ON ENABLE");
    }
    private void Awake()
    {
        Debug.Log("AWAKE!!");
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START!!");

        Application.targetFrameRate = 90;
        StartCoroutine(TestCoroutine());
    }

    int fixedCount;
    int updateCount;

    private void FixedUpdate()
    {
        fixedCount++;
    }
    void Update()
    {
        Debug.Log("UPDATE!!!!!!!!!!!!!!!!!");
    }
    private void LateUpdate()
    {
    }

    IEnumerator TestCoroutine()
    {
        yield return new WaitForFixedUpdate();  // ���� ������Ʈ -> ����ó�� -> (���� ����)
        yield return null;                      // ���� ������Ʈ -> ����ó�� -> ������Ʈ -> (���� ����)
        yield return new WaitForEndOfFrame();   // ���� ������Ʈ -> ����ó�� -> ������Ʈ -> �Է� -> ���� -> (���Ŀ� ����)

        yield return new WaitForSeconds(1);     // n�� �Ŀ� ����.

        yield return StartCoroutine(TestCoroutine2());  // �ش� �ڷ�ƾ�� ������ ����.

        Debug.Log("�׽�Ʈ ��");
    }
    IEnumerator TestCoroutine2()
    {
        Debug.Log("�׽�Ʈ2 ����!");

        yield return new WaitForSeconds(3);

        Debug.Log("�׽�Ʈ2 ��");
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(0,0,100,100), string.Format("fixed:{0}, update:{1}", fixedCount, updateCount));
    }

    private void OnApplicationFocus(bool focus)
    {
        // ���� ȭ���� ���� �������� ��.
        // ����� : ȭ���� �������ִٰ� �ٽ� �÷�����.
        Debug.Log("OnApplicationFocus : " + focus);
    }
    private void OnApplicationPause(bool pause)
    {
        // ������ ����϶� �ٸ� â�� ����������
        // ȭ���� ��������.
        Debug.Log("OnApplicationPause : " + pause);
    }
    private void OnApplicationQuit()
    {
        // ���α׷��� ����������.
        // ���� ����������.

        Debug.Log("OnApplicationQuit");
    }
}
