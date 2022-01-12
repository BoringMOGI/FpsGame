using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<PoolType> : MonoBehaviour
    where PoolType : MonoBehaviour, ObjectPool<PoolType>.IPool
{
    public interface IPool
    {
        void Setup(OnReturnPoolEvent OnReturnPool);
    }

    public delegate void OnReturnPoolEvent(PoolType type);
        
    [SerializeField] protected PoolType prefab;               // 풀링 오브젝트 프리팹.
    [SerializeField] protected int createCount;               // 최초 생성 개수.

    Transform storageParent;                        // 저장소 부모 오브젝트.
    Stack<PoolType> storage = new Stack<PoolType>();

    protected void Awake()
    {
        storageParent = new GameObject("Pool Storage").transform;
        storageParent.SetParent(transform);

        storageParent.gameObject.SetActive(false);  // 저장소 오브젝트 비활성화.
        CreatePool(createCount);
    }

    private void CreatePool(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            PoolType newPool = Instantiate(prefab, storageParent);
            newPool.Setup(OnReturnPool);
            storage.Push(newPool);
        }
    }


    public PoolType GetPool()
    {
        if (storage.Count <= 0)
            CreatePool();

        PoolType pop = storage.Pop();               // 저장소에서 하나 꺼낸다.
        pop.transform.SetParent(transform);         // 오브젝트의 부모를 변경.

        return pop;                                 // 오브젝트 반환.
    }
    public void OnReturnPool(PoolType pool)
    {
        pool.transform.SetParent(storageParent);    // 돌아온 풀링 오브젝트의 부모 변경.
        storage.Push(pool);                         // stack 대기열에 추가.
    }
}
