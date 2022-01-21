using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RangeManager : MonoBehaviour
{
    [System.Serializable]
    public class RangeInfo
    {
        // 정보의 종류
        public enum INFO_TYPE
        {
            Difficult,
            Movement,
            Armor,
            Ammo,
        }
        public enum DIFFICULT
        {
            Easy,
            Normal,
            Hard,
            KILL_50,
            KILL_100,
        }
        public enum BOT_MOVEMENT
        {
            Move,
            Stand,
        }

        public DIFFICULT difficulty;        // 난이도.
        public BOT_MOVEMENT botMovement;    // 봇 움직임.
        public bool isBotArmor;             // 봇 아머 착용.
        public bool isInfinityAmmo;         // 무한 탄약.

        public RangeInfo()
        {
            difficulty = DIFFICULT.Normal;
            botMovement = BOT_MOVEMENT.Stand;
            isInfinityAmmo = false;
            isBotArmor = false;
        }
    }

    [SerializeField] TargetBoard prefab;
    [SerializeField] TMP_Text scoreText;            // 점수 텍스트.
    [SerializeField] TMP_Text remainingText;        // 남은 수 테스트.

    [Header("Variable")]
    [SerializeField] float radiusX;
    [SerializeField] float radiusZ;
    [SerializeField] float createTime;              // 타겟의 생성 주기.
    [SerializeField] int maxTargetCount;            // 최대 생성 개수.

    [Header("Detail")]
    [SerializeField] RangeSimpleUI rangeSimpleUI;   // 사격장 정보창.
    [SerializeField] RangeDetailUI rangeDetailUI;   // 사격장 제어창.

    bool isStart = false;

    float timer = 0.0f;                 // 몇 초까지 흘렀는지 기록하는 변수.
    int createCount = 0;                // 몇 개까지 만들었는지 기록하는 변수.

    int currentScore = 0;               // 내가 얻은 점수.
    int remainingCount => maxTargetCount - createCount;     // 남은 개수.

    CallbackEvent onCallback;           // 콜백 이벤트.
    RangeInfo rangeInfo;

    private void Start()
    {
        rangeInfo = new RangeInfo();                    // 사격장 데이터 객체 생성.

        rangeSimpleUI.Setup(rangeInfo);                 // 사격장 정보창 셋업.
        rangeDetailUI.Setup(rangeInfo, OnChangeInfo);   // 사격장 제어창 셋업.

        UpdateScoreUI();
    }
    private void Update()
    {
        if (!isStart)
            return;

        timer += Time.deltaTime;        // timer의 값을 시간의 흐름에 따라 더한다.
        if(timer >= createTime)         // timer의 값이 특정 시간이 되었으면
        {
            CreateBoard();              // 타겟을 랜덤한 위치에 생성.
            UpdateScoreUI();            // 스코어 UI 업데이트.
            timer = 0.0f;               // timer값을 초기화.
            
            if(createCount >= maxTargetCount)
            {
                OnEndGame();
            }
        }
    }

    public void OnStartGame(CallbackEvent onCallback)
    {
        if (isStart)            // 이미 시작 중이라면.
            return;             // 리턴한다.

        this.onCallback = onCallback;
        isStart = true;

        currentScore = 0;
        createCount = 0;
        timer = 0.0f;

        UpdateScoreUI();
    }

    // 사격장 정보 변경.
    private void OnChangeInfo(RangeInfo.INFO_TYPE infoType, int index)
    {
        switch(infoType)
        {
            case RangeInfo.INFO_TYPE.Difficult:
                rangeInfo.difficulty = (RangeInfo.DIFFICULT)index;
                break;
            case RangeInfo.INFO_TYPE.Movement:
                rangeInfo.botMovement = (RangeInfo.BOT_MOVEMENT)index;
                break;
            case RangeInfo.INFO_TYPE.Armor:
                rangeInfo.isBotArmor = (index == 0);
                break;
            case RangeInfo.INFO_TYPE.Ammo:
                rangeInfo.isInfinityAmmo = (index == 0);
                break;
        }
    }

    // 사격장 제어 ===============================================
    private void UpdateScoreUI()
    {
        scoreText.text = currentScore.ToString();
        remainingText.text = remainingCount.ToString();
    }
    private void CreateBoard()
    {
        // 타겟의 위치 계산.
        Vector3 position = transform.position;
        position.x += Random.Range(-radiusX, radiusX);
        position.z += Random.Range(-radiusZ, radiusZ);

        // 타겟 생성 및 위치,회전 값 대입.
        TargetBoard newTarget = Instantiate(prefab);
        newTarget.transform.position = position;
        newTarget.transform.eulerAngles = Vector3.zero;

        // 타겟에게 이벤트 등록.
        newTarget.OnHitEvent += OnHitTarget;

        // 타겟의 Setup함수 호출.
        newTarget.Setup(1);

        // 생성 카운트 1 증가.
        createCount++;              
    }
    private void OnHitTarget()
    {
        // 타겟이 맞았다. 점수를 1 증가시키고 UI를 그려라.
        currentScore += 1;
        UpdateScoreUI();
    }
    private void OnEndGame()
    {
        isStart = false;
        onCallback?.Invoke();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(radiusX * 2f, 0.2f, radiusZ * 2f));
    }
}
