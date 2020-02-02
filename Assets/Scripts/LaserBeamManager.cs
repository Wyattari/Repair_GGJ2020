using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamManager : MonoBehaviour
{
    public BeamEffect[] beams;
    public Transform PlayerStartPosition;

    void Start()
    {
        SetStartPosition();
    }

    public void SetStartPosition()
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].startPosition = PlayerStartPosition;
        }
    }

    public void ShootBeams(Vector3 endPosition)
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].ShootBeam(endPosition);
        }
    }

    public void StopBeams()
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].StopBeam();
        }
    }

    public void SetPositions(Vector3 endPosition)
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].endPosition = endPosition;
        }
    }
}
