using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectDisappear : MonoBehaviour
{
    [SerializeField] float lifeTime;

    private void Start()
    {
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(lifeTime);

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        float maxTime = 1.0f;
        float current = 1.0f;

        /*
        while(meshRenderer != null)
        {
            Color color = meshRenderer.material.color;          // 메쉬의 색상을 가져온다.
            color.a = current / maxTime;                        // 알파 값 계산.

            meshRenderer.material.color = color;

            if ((current -= Time.deltaTime) <= 0.0f)
                break;
        }
        */

        Destroy(gameObject);
    }

}
