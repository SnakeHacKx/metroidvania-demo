using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    void Start()
    {
        print("START");
        this.transform.position = GameManager.SharedInstance.lastCheckpointPos;
    }
}
