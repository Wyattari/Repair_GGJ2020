using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamEffect : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    BSpline bSpline;
    public LineRenderer lineRenderer;
    public float duration;
    public float waitTimeEndBeam;
    public AnimationCurve animationCurve;

    public Vector3 min;
    public Vector3 max;

    public Vector3 jitterMin;
    public Vector3 jitterMax;
    public float beamSpinSpeed = 0.5f;


    Vector3 InitOffset1;
    Vector3 InitOffset2;
    Vector3 Offset1;
    Vector3 Offset2;
    float OffsetAngle1;
    float OffsetAngle2;
    bool isShooting = false;

    Vector3[] points;
    List<Vector3> bPoints;

    void Start()
    {
        bSpline = new BSpline();
    }

    public void ShootBeam(Vector3 _StartPosition, Vector3 _EndPosition)
    {
        startPosition = _StartPosition;
        endPosition = _EndPosition;


        Offset1 = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
        Offset2 = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
        InitOffset1= Offset1;
        InitOffset2= Offset2;
        
        if(isActiveAndEnabled)
        {
            StartCoroutine(ShootOverTime());
        }
    }

    public void StopBeam()
    {
        isShooting = false;
    }

    void getPoints()
    {

        Vector3[] inputPoints = new Vector3[] { startPosition, Vector3.Lerp(startPosition, endPosition, 0.33f) - Offset1, Vector3.Lerp(startPosition, endPosition, 0.67f) - Offset2, endPosition };

        bPoints = new List<Vector3>();
        bPoints.AddRange(bSpline.GetPoints(inputPoints));
    }

    void changeOffsets()
    {
        OffsetAngle1+=beamSpinSpeed;
        OffsetAngle2+=beamSpinSpeed;
        Offset1 = RotateAroundPoint(InitOffset1, Vector3.Lerp(startPosition, endPosition, 0.33f), Quaternion.Euler(0, 0, OffsetAngle1))+new Vector3(UnityEngine.Random.Range(jitterMin.x, jitterMax.x), UnityEngine.Random.Range(jitterMin.y, jitterMax.y), UnityEngine.Random.Range(jitterMin.z, jitterMax.z));;
        Offset2 = RotateAroundPoint(InitOffset2, Vector3.Lerp(startPosition, endPosition, 0.67f), Quaternion.Euler(0, 0, OffsetAngle2))+new Vector3(UnityEngine.Random.Range(jitterMin.x, jitterMax.x), UnityEngine.Random.Range(jitterMin.y, jitterMax.y), UnityEngine.Random.Range(jitterMin.z, jitterMax.z));;
    }

    Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
    {
        return angle * (point - pivot) + pivot;
    }

    public IEnumerator ShootOverTime()
    {
        float journey = 0;

        int startIndex = 0;
        int endIndex = 0;

        float endBeamTime = 0;
        float endJourney;

        while (endBeamTime < duration)
        {
            getPoints();
            changeOffsets();
            journey += Time.deltaTime;
            float startJourney = animationCurve.Evaluate(journey / duration);

            if (isShooting)
            {
                endBeamTime += Time.deltaTime;
                endJourney = animationCurve.Evaluate(endBeamTime / duration);
                endIndex = Mathf.FloorToInt(endJourney * (bPoints.Count - 1));
            }

            startIndex = Mathf.FloorToInt(startJourney * (bPoints.Count - 1));
            List<Vector3> JourneyPoints = bPoints.GetRange(endIndex, startIndex - endIndex);

            lineRenderer.positionCount = JourneyPoints.Count;
            lineRenderer.SetPositions(JourneyPoints.ToArray());

            // expandSize = Mathf.Lerp(currentPercentage, endSize, startJourney);
            // PopupMenuRect.sizeDelta = new Vector2(PopupMenuRect.sizeDelta.x, expandSize);
            yield return null;
        }
        yield return null;
    }
}
