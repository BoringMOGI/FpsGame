using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] BoardManager boradManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            boradManager.OnStartGame();
    }
}
