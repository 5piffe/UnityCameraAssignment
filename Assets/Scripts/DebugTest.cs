using System;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(Time.time);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.LogWarning(Time.time);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.LogError(Time.time);
        }
    }
}
