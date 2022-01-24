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
        yield return new WaitForFixedUpdate();  // 물리 업데이트 -> 물리처리 -> (이후 실행)
        yield return null;                      // 물리 업데이트 -> 물리처리 -> 업데이트 -> (이후 실행)
        yield return new WaitForEndOfFrame();   // 물리 업데이트 -> 물리처리 -> 업데이트 -> 입력 -> 렌더 -> (이후에 실행)

        yield return new WaitForSeconds(1);     // n초 후에 실행.

        yield return StartCoroutine(TestCoroutine2());  // 해당 코루틴이 끝나면 실행.

        Debug.Log("테스트 끝");
    }
    IEnumerator TestCoroutine2()
    {
        Debug.Log("테스트2 시작!");

        yield return new WaitForSeconds(3);

        Debug.Log("테스트2 끝");
    }

    private void OnGUI()
    {
        GUI.TextArea(new Rect(0,0,100,100), string.Format("fixed:{0}, update:{1}", fixedCount, updateCount));
    }

    private void OnApplicationFocus(bool focus)
    {
        // 내가 화면을 보기 시작했을 때.
        // 모바일 : 화면이 내려가있다가 다시 올렸을때.
        Debug.Log("OnApplicationFocus : " + focus);
    }
    private void OnApplicationPause(bool pause)
    {
        // 윈도우 모드일때 다른 창을 선택했을때
        // 화면을 내려을때.
        Debug.Log("OnApplicationPause : " + pause);
    }
    private void OnApplicationQuit()
    {
        // 프로그램을 종료했을떄.
        // 앱을 종료했을때.

        Debug.Log("OnApplicationQuit");
    }
}
