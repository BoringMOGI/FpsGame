using System.Collections;
using UnityEngine;

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

    [System.Serializable]
    public struct DifficultInfo
    {
        public RangeInfo.DIFFICULT difficult;   // 난이도 타입.
        [Range(1, 30)] 
        public int maxCreateCount;              // 생성 수.
        [Range(0.1f, 2.0f)]
        public float createRate;                // 생성 주기.
        [Range(0.2f, 3.0f)]
        public float showTime;                  // 등장 유지 시간.
    }

    [SerializeField] TargetBoard prefab;

    [Header("Difficult")]
    [SerializeField] DifficultInfo[] difficultInfos;

    [Header("Range")]
    [SerializeField] float radiusX;
    [SerializeField] float radiusZ;

    [Header("Detail")]
    [SerializeField] RangeSimpleUI rangeSimpleUI;   // 사격장 정보창.
    [SerializeField] RangeDetailUI rangeDetailUI;   // 사격장 제어창.

    bool isStart = false;

    int createCount = 0;        // 생성 개수.
    int maxCreateCount = 0;     // 최대 개수.
    int currentScore = 0;       // 현재 점수. 

    CallbackEvent onCallback;   // 콜백 이벤트.
    RangeInfo rangeInfo;        // 사격장 정보.

    private void Start()
    {
        rangeInfo = new RangeInfo();                    // 사격장 데이터 객체 생성.

        rangeSimpleUI.Setup(rangeInfo);                 // 사격장 정보창 셋업.
        rangeDetailUI.Setup(rangeInfo, OnChangeInfo);   // 사격장 제어창 셋업.

        UpdateScoreUI();
    }



    public void OnStartGame(CallbackEvent onCallback)
    {
        if (isStart)            // 이미 시작 중이라면.
            return;             // 리턴한다.

        this.onCallback = onCallback;
        isStart = true;

        StartCoroutine(RangeProcess());        
    }
    private IEnumerator RangeProcess()
    {
        DifficultInfo info = GetCurrentDifficult();        // 현재 난이도 데이터.

        float timer = 0.0f;                 // 몇 초 흘렀는지?

        // 초기 값 생성.
        currentScore = 0;
        createCount = 0;
        maxCreateCount = info.maxCreateCount;

        float createRate = info.createRate;

        // UI 갱신.
        UpdateScoreUI();

        while(createCount < maxCreateCount)     // 적을 다 생성했으면 종료.
        {
            timer += Time.deltaTime;            // 시간 값을 더해서,
            if(timer >= createRate)               // 일정 시간이 흐른 뒤 적을 생성.
            {
                timer = 0.0f;

                CreateBoard(info);
                UpdateScoreUI();
            }

            yield return null;                  // 업데이트 함수가 호출이 끝나기를 기다린다.
        }

        yield return new WaitForSeconds(1f);
        
        OnEndGame();                        
    }

    
    private DifficultInfo GetCurrentDifficult()
    {
        RangeInfo.DIFFICULT diff = rangeInfo.difficulty;        // 현재 난이도 가져옴.
        for(int i = 0; i<difficultInfos.Length; i++)
        {
            if(difficultInfos[i].difficult == diff)
                return difficultInfos[i];
        }

        return default(DifficultInfo);
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
        RangeUI.Instance.UpdateUI(currentScore, maxCreateCount - createCount);
    }
    private void CreateBoard(DifficultInfo info)
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
        newTarget.Setup(info.showTime, rangeInfo.isBotArmor);

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
