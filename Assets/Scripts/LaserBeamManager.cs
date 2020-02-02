using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamManager : MonoBehaviour
{
    public BeamEffect[] beams;

    public void ShootBeams(Vector3 startPosition, Vector3 endPosition)
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].ShootBeam(startPosition, endPosition);
        }
    }

    public void StopBeams(Vector3 startPosition, Vector3 endPosition)
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].StopBeam();
        }
    }

    public void SetPositions(Vector3 startPosition, Vector3 endPosition)
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].startPosition = startPosition;
            beams[i].endPosition = endPosition;
        }
    }
}
