using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRotator : MonoBehaviour
{
    public float RotationSpeed = 10;
    public float HoldInterval = 1;
    public float HoldSpeed = 1;

    float CurrentAngle = 0;
    float LastHold = 0;

    [SerializeField] GameObject[] childrenRotate;
    public bool enableChildRotation = true;
    public Vector3[] childOffset;
    [SerializeField] AnimationCurve startCurve;
    Vector3[] childrenOrigins;


    void Start()
    {
        childrenOrigins = new Vector3[childrenRotate.Length];
        LastHold = Time.time - HoldInterval;

        for (int i = 0; i < childrenRotate.Length; i++) {
            childrenOrigins[i] = childrenRotate[i].transform.localPosition;
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
        if(Time.time - LastHold < HoldInterval)
        {
            speed = HoldSpeed;
        }
        CurrentAngle += speed * Time.deltaTime;
        if (enableChildRotation) {
            foreach(GameObject child in childrenRotate) {
                child.transform.localRotation = Quaternion.Euler(0,0,CurrentAngle);
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, CurrentAngle);
    }

    public IEnumerator BeginRotation() {
        float t = 0;

        Debug.Log(startCurve.keys[1].time);
        while (t <= startCurve.keys[1].time) {
            t += Time.deltaTime;
            for (int i = 0; i < childrenRotate.Length; i++) {
                childrenRotate[i].transform.localPosition = Vector3.Lerp(childrenOrigins[i], childrenOrigins[i] + childOffset[i], startCurve.Evaluate(t));
            }
            yield return new WaitForEndOfFrame();
        }
    }


    public IEnumerator Reassemble() {
        yield return null;
    }

}
