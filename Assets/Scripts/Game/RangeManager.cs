using System.Collections;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    [System.Serializable]
    public class RangeInfo
    {
        // ������ ����
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

        public DIFFICULT difficulty;        // ���̵�.
        public BOT_MOVEMENT botMovement;    // �� ������.
        public bool isBotArmor;             // �� �Ƹ� ����.
        public bool isInfinityAmmo;         // ���� ź��.

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
        public RangeInfo.DIFFICULT difficult;   // ���̵� Ÿ��.
        [Range(1, 30)] 
        public int maxCreateCount;              // ���� ��.
        [Range(0.1f, 2.0f)]
        public float createRate;                // ���� �ֱ�.
        [Range(0.2f, 3.0f)]
        public float showTime;                  // ���� ���� �ð�.
    }

    [SerializeField] TargetBoard prefab;

    [Header("Difficult")]
    [SerializeField] DifficultInfo[] difficultInfos;

    [Header("Range")]
    [SerializeField] float radiusX;
    [SerializeField] float radiusZ;

    [Header("Detail")]
    [SerializeField] RangeSimpleUI rangeSimpleUI;   // ����� ����â.
    [SerializeField] RangeDetailUI rangeDetailUI;   // ����� ����â.

    bool isStart = false;

    int createCount = 0;        // ���� ����.
    int maxCreateCount = 0;     // �ִ� ����.
    int currentScore = 0;       // ���� ����. 

    CallbackEvent onCallback;   // �ݹ� �̺�Ʈ.
    RangeInfo rangeInfo;        // ����� ����.

    private void Start()
    {
        rangeInfo = new RangeInfo();                    // ����� ������ ��ü ����.

        rangeSimpleUI.Setup(rangeInfo);                 // ����� ����â �¾�.
        rangeDetailUI.Setup(rangeInfo, OnChangeInfo);   // ����� ����â �¾�.

        UpdateScoreUI();
    }



    public void OnStartGame(CallbackEvent onCallback)
    {
        if (isStart)            // �̹� ���� ���̶��.
            return;             // �����Ѵ�.

        this.onCallback = onCallback;
        isStart = true;

        StartCoroutine(RangeProcess());        
    }
    private IEnumerator RangeProcess()
    {
        DifficultInfo info = GetCurrentDifficult();        // ���� ���̵� ������.

        float timer = 0.0f;                 // �� �� �귶����?

        // �ʱ� �� ����.
        currentScore = 0;
        createCount = 0;
        maxCreateCount = info.maxCreateCount;

        float createRate = info.createRate;

        // UI ����.
        UpdateScoreUI();

        while(createCount < maxCreateCount)     // ���� �� ���������� ����.
        {
            timer += Time.deltaTime;            // �ð� ���� ���ؼ�,
            if(timer >= createRate)               // ���� �ð��� �帥 �� ���� ����.
            {
                timer = 0.0f;

                CreateBoard(info);
                UpdateScoreUI();
            }

            yield return null;                  // ������Ʈ �Լ��� ȣ���� �����⸦ ��ٸ���.
        }

        yield return new WaitForSeconds(1f);
        
        OnEndGame();                        
    }

    
    private DifficultInfo GetCurrentDifficult()
    {
        RangeInfo.DIFFICULT diff = rangeInfo.difficulty;        // ���� ���̵� ������.
        for(int i = 0; i<difficultInfos.Length; i++)
        {
            if(difficultInfos[i].difficult == diff)
                return difficultInfos[i];
        }

        return default(DifficultInfo);
    }


    // ����� ���� ����.
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

    // ����� ���� ===============================================
    private void UpdateScoreUI()
    {
        RangeUI.Instance.UpdateUI(currentScore, maxCreateCount - createCount);
    }
    private void CreateBoard(DifficultInfo info)
    {
        // Ÿ���� ��ġ ���.
        Vector3 position = transform.position;
        position.x += Random.Range(-radiusX, radiusX);
        position.z += Random.Range(-radiusZ, radiusZ);

        // Ÿ�� ���� �� ��ġ,ȸ�� �� ����.
        TargetBoard newTarget = Instantiate(prefab);
        newTarget.transform.position = position;
        newTarget.transform.eulerAngles = Vector3.zero;

        // Ÿ�ٿ��� �̺�Ʈ ���.
        newTarget.OnHitEvent += OnHitTarget;

        // Ÿ���� Setup�Լ� ȣ��.
        newTarget.Setup(info.showTime, rangeInfo.isBotArmor);

        // ���� ī��Ʈ 1 ����.
        createCount++;              
    }
    private void OnHitTarget()
    {
        // Ÿ���� �¾Ҵ�. ������ 1 ������Ű�� UI�� �׷���.
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
