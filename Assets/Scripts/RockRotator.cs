using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRotator : MonoBehaviour
{
    public float RotationSpeed = 10;
    public float HoldInterval = 1;
    public float HoldSpeed = 1;

    float[] CurrentAngle;
    float LastHold = 0;

    [SerializeField] GameObject[] childrenRotate;
    [SerializeField] GameObject[] childrenRotateOnly;
    public bool enableChildRotation = true;
    public Vector3[] childOffset;
    [SerializeField] AnimationCurve startCurve;
    Vector3[] childrenOrigins;
    bool beginning;
    float introTime = 0;


    void Start()
    {

        childrenOrigins = new Vector3[childrenRotate.Length];
        CurrentAngle = new float[childrenRotate.Length];
        LastHold = Time.time - HoldInterval;
        for (int i = 0; i < childrenRotate.Length; i++)
        {
            childrenOrigins[i] = childrenRotate[i].transform.localPosition;
            CurrentAngle[i] = Random.Range(0, 180);
        }

        StartCoroutine(BeginRotation());


    }

    public void HoldRocks()
    {
        LastHold = Time.time;
    }

    void Update()
    {
        float speed = RotationSpeed;
        if (Time.time - LastHold < HoldInterval)
        {
            speed = HoldSpeed;
        }
        if (beginning)
        {
            introTime += Time.deltaTime;
            speed = Mathf.Lerp(HoldSpeed, RotationSpeed, startCurve.Evaluate(introTime));
        }
        CurrentAngle[0] += speed * Time.deltaTime;
        if (enableChildRotation)
        {
            for (int i = 0; i < childrenRotate.Length; i++)
            {
                childrenRotate[i].transform.localRotation = Quaternion.Euler(0, 0, CurrentAngle[i]);
            }
            for (int i = 0; i < childrenRotateOnly.Length; i++)
            {
                childrenRotateOnly[i].transform.localRotation = Quaternion.Euler(0, 0, CurrentAngle[i]);
            }
        }
    }

    public IEnumerator BeginRotation()
    {
        float t = 0;

        Debug.Log(startCurve.keys[1].time);
        while (t <= startCurve.keys[1].time)
        {
            beginning = true;
            t += Time.deltaTime;
            for (int i = 0; i < childrenRotate.Length; i++)
            {
                childrenRotate[i].transform.localPosition = Vector3.Lerp(childrenOrigins[i], childrenOrigins[i] + childOffset[i], startCurve.Evaluate(t));
            }
            yield return new WaitForEndOfFrame();
        }
        beginning = false;
    }


    public IEnumerator Reassemble()
    {
        yield return null;
    }

}
