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

    [SerializeField] TargetBoard prefab;

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
        
        StartCoroutine(RangeProcess(10, 0.5f));        
    }
    private IEnumerator RangeProcess(int maxCreateCount, float showRate)
    {
        float timer = 0.0f;                 // �� �� �귶����?

        // �ʱ� �� ����.
        currentScore = 0;
        createCount = 0;
        this.maxCreateCount = maxCreateCount;

        // UI ����.
        UpdateScoreUI();

        while(createCount < maxCreateCount)     // ���� �� ���������� ����.
        {
            timer += Time.deltaTime;            // �ð� ���� ���ؼ�,
            if(timer >= showRate)               // ���� �ð��� �帥 �� ���� ����.
            {
                timer = 0.0f;

                CreateBoard();
                UpdateScoreUI();
            }

            yield return null;                  // ������Ʈ �Լ��� ȣ���� �����⸦ ��ٸ���.
        }

        yield return new WaitForSeconds(1f);
        
        OnEndGame();                        
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
    private void CreateBoard()
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
        newTarget.Setup(1);

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
