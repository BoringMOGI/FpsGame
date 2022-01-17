using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEffect : MonoBehaviour
{
    ParticleSystem particle;

    void Start()
    {
        // ��ƼŬ ������Ʈ �˻�. ���� ���ٸ� ��ũ��Ʈ ��Ȱ��ȭ.
        particle = GetComponent<ParticleSystem>();
        if (particle == null)
            enabled = false;
    }

    void Update()
    {
        // ��ƼŬ �ִϸ��̼��� ��������� ���� �� ����.
        if(particle.isPlaying == false)
            Destroy(gameObject);
    }
}
