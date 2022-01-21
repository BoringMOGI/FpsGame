using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField] TMP_Text scoreText;            // ���� �ؽ�Ʈ.
    [SerializeField] TMP_Text remainingText;        // ���� �� �׽�Ʈ.

    [Header("Variable")]
    [SerializeField] float radiusX;
    [SerializeField] float radiusZ;
    [SerializeField] float createTime;              // Ÿ���� ���� �ֱ�.
    [SerializeField] int maxTargetCount;            // �ִ� ���� ����.

    [Header("Detail")]
    [SerializeField] RangeSimpleUI rangeSimpleUI;   // ����� ����â.
    [SerializeField] RangeDetailUI rangeDetailUI;   // ����� ����â.

    bool isStart = false;

    float timer = 0.0f;                 // �� �ʱ��� �귶���� ����ϴ� ����.
    int createCount = 0;                // �� ������ ��������� ����ϴ� ����.

    int currentScore = 0;               // ���� ���� ����.
    int remainingCount => maxTargetCount - createCount;     // ���� ����.

    CallbackEvent onCallback;           // �ݹ� �̺�Ʈ.
    RangeInfo rangeInfo;

    private void Start()
    {
        rangeInfo = new RangeInfo();                    // ����� ������ ��ü ����.

        rangeSimpleUI.Setup(rangeInfo);                 // ����� ����â �¾�.
        rangeDetailUI.Setup(rangeInfo, OnChangeInfo);   // ����� ����â �¾�.

        UpdateScoreUI();
    }
    private void Update()
    {
        if (!isStart)
            return;

        timer += Time.deltaTime;        // timer�� ���� �ð��� �帧�� ���� ���Ѵ�.
        if(timer >= createTime)         // timer�� ���� Ư�� �ð��� �Ǿ�����
        {
            CreateBoard();              // Ÿ���� ������ ��ġ�� ����.
            UpdateScoreUI();            // ���ھ� UI ������Ʈ.
            timer = 0.0f;               // timer���� �ʱ�ȭ.
            
            if(createCount >= maxTargetCount)
            {
                OnEndGame();
            }
        }
    }

    public void OnStartGame(CallbackEvent onCallback)
    {
        if (isStart)            // �̹� ���� ���̶��.
            return;             // �����Ѵ�.

        this.onCallback = onCallback;
        isStart = true;

        currentScore = 0;
        createCount = 0;
        timer = 0.0f;

        UpdateScoreUI();
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
        scoreText.text = currentScore.ToString();
        remainingText.text = remainingCount.ToString();
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
