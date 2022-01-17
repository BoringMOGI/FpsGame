using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEffect : MonoBehaviour
{
    ParticleSystem particle;

    void Start()
    {
        // 파티클 컴포넌트 검색. 만약 없다면 스크립트 비활성화.
        particle = GetComponent<ParticleSystem>();
        if (particle == null)
            enabled = false;
    }

    void Update()
    {
        // 파티클 애니메이션을 재생중이지 않을 때 제거.
        if(particle.isPlaying == false)
            Destroy(gameObject);
    }
}
