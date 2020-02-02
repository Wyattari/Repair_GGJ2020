using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRotator : MonoBehaviour
{
    public float RotationSpeed = 10;
    public float HoldInterval = 1;
    public float WarningInterval = 1f;
    public float HoldSpeed = 1;
    public float FreezePulseRate = 0.5f;

    float[] CurrentAngle;
    float LastHold = 0;

    [SerializeField] GameObject[] childrenRotate;
    [SerializeField] GameObject[] childrenRotateOnly;
    Color[] childrenRotateColor;
    Color[] childrenRotateOnlyColor;
    public bool enableChildRotation = true;
    public Vector3[] childOffset;
    [SerializeField] AnimationCurve startCurve;
    public Color FreezeColor = Color.blue;
    public Color WarningColor = Color.red;
    Vector3[] childrenOrigins;
    bool beginning;
    float introTime = 0;

    Color GetMateriaColorTint(GameObject gobj)
    {
        var renderer = gobj.GetComponent<Renderer>();
        if (renderer == null)
            return Color.white;

        var material = renderer.material;
        if (material == null)
            return Color.white;

        var ret = material.GetColor("_ColorTint");
        return ret != null ? ret : Color.white;
    }

    void Start()
    {

        childrenOrigins = new Vector3[childrenRotate.Length];
        CurrentAngle = new float[childrenRotate.Length];
        childrenRotateColor = childrenRotate.Length > 0 ? new Color[childrenRotate.Length] : null;
        childrenRotateOnlyColor = childrenRotateOnly.Length > 0 ? new Color[childrenRotateOnly.Length] : null;

        LastHold = Time.time - HoldInterval;
        for (int i = 0; i < childrenRotate.Length; i++)
        {
            childrenOrigins[i] = childrenRotate[i].transform.localPosition;
            CurrentAngle[i] = Random.Range(0, 180);
            childrenRotateColor[i] = GetMateriaColorTint(childrenRotate[i]);
        }

        for (int i = 0; i < childrenRotateOnly.Length; i++)
        {
            childrenRotateOnlyColor[i] = GetMateriaColorTint(childrenRotateOnly[i]);
        }

        StartCoroutine(BeginRotation());


    }

    public void HoldRocks()
    {
        LastHold = Time.time;
    }

    public void FreezeChild(GameObject child, Color orig_color, bool frozen)
    {
        var renderer = child.GetComponent<Renderer>();
        if (renderer == null)
            return;
        var material = renderer.material;
        if (material == null)
            return;
        float time_since_hold = Time.time - LastHold;
        if (!frozen)
        {
            material.SetColor("_ColorTint", orig_color);
            return;
        }
        var time_left = HoldInterval - time_since_hold;
        var color = time_left < WarningInterval ? WarningColor : FreezeColor;
        color = Color.Lerp(color, orig_color, 0.5f + 0.5f*Mathf.Sin(2f * Mathf.PI * time_since_hold / FreezePulseRate));

        material.SetColor("_ColorTint", color);

        foreach(Transform subchild in child.transform)
        {
            FreezeChild(subchild.gameObject, orig_color, frozen);
        }
    }

    void Update()
    {
        float speed = RotationSpeed;
        bool frozen = false;
        if (Time.time - LastHold < HoldInterval)
        {
            frozen = true;
            speed = HoldSpeed;
        }
        if (beginning)
        {
            frozen = false;
            introTime += Time.deltaTime;
            speed = Mathf.Lerp(HoldSpeed, RotationSpeed, startCurve.Evaluate(introTime));
        }
        CurrentAngle[0] += speed * Time.deltaTime;
        if (enableChildRotation)
        {
            for (int i = 0; i < childrenRotate.Length; i++)
            {
                var child = childrenRotate[i];
                FreezeChild(child, childrenRotateColor[i], frozen);
                child.transform.localRotation = Quaternion.Euler(0, 0, CurrentAngle[i]);
            }
            if (childrenRotateOnly != null) { 
                for (int i = 0; i < childrenRotateOnly.Length; i++)
                {
                    var child = childrenRotate[i];
                    FreezeChild(child, childrenRotateOnlyColor[i], frozen);
                    child.transform.localRotation = Quaternion.Euler(0, 0, CurrentAngle[i]);
                }
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
