using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRotator : MonoBehaviour
{
    public float RotationSpeed = 10;
    public float HoldInterval = 1;

    float CurrentAngle = 0;
    float LastHold = 0;

    void Start()
    {
        LastHold = Time.time;
    }

    public void HoldRocks()
    {
        LastHold = Time.time;
    }

    void Update()
    {
        if(Time.time - LastHold > HoldInterval)
            CurrentAngle += RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, CurrentAngle);
    }

    void OnShoot()
    {
        Debug.Log("Shoot");
    }
}
