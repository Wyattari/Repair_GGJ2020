using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamManager : BaseBehaviour
{
    public BeamEffect[] beams;
    int playerId;
    public int PlayerId {
		get { return playerId; }
		set {
            playerId = value;
            SetColor(state.Players[playerId].Color);
        }
    }

    public void ShootBeams(Vector3 startPosition, Vector3 endPosition)
    {
        Debug.Log("WE SHOOTING");
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

    public void SetStartPositions(Vector3 startPosition)
    {
        for(int i = 0 ; i < beams.Length; i++)
        {
            beams[i].startPosition = startPosition;
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

	void SetColor(Color color) {
        for (int i = 0; i < beams.Length; i++) {
            Material beamMat = beams[i].lineRenderer.material;
            beamMat.SetColor("_TintColor", GetHueColor(color, beamMat.GetColor("_TintColor")));
        }
    }

    Color GetHueColor(Color hueColor, Color mainColor) {
        float h1, s1, b1;
        Color.RGBToHSV(hueColor, out h1, out s1, out b1);

        float h2, s2, b2;
        Color.RGBToHSV(mainColor, out h2, out s2, out b2);

        Color rgbColor = Color.HSVToRGB(h1, s2, b2);
        return new Color(rgbColor.r, rgbColor.g, rgbColor.b, mainColor.a);
    }
}
