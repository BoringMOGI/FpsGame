using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] TargetBoard prefab;
    [SerializeField] float radiusX;
    [SerializeField] float radiusZ;

    private void Start()
    {
        CreateBoard();
    }

    void CreateBoard()
    {
        Vector3 position = transform.position;
        
        position.x += Random.Range(-radiusX, radiusX);
        position.z += Random.Range(-radiusZ, radiusZ);

        TargetBoard newTarget = Instantiate(prefab);
        newTarget.transform.position = position;
        newTarget.transform.eulerAngles = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(radiusX * 2f, 0.2f, radiusZ * 2f));
    }
}
