using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    void Update()
    {
        if(GameObject.FindWithTag("Enemy")==null)
        {
            Debug.Log("clear"); // clear 확인 변수값 설정
        }
    }
    
}
