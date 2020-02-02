using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{

    public Color color;

    public ParticleSystem LargeSparks;
    public ParticleSystem SmallSparks;
    public MeshRenderer Texture;
    public MeshRenderer PulsingRim;
    public LaserBeamManager laserBeamManager;
    

    // Start is called before the first frame update
    void Start()
    {
        var LMain = LargeSparks.main;
        var SMain = SmallSparks.main;
        LMain.startColor = GetHueColor(LargeSparks.main.startColor.color); 
        SMain.startColor = GetHueColor(SmallSparks.main.startColor.color);

        Texture.material.SetColor("_TintColor", GetHueColor(Texture.material.GetColor("_TintColor")));
        PulsingRim.material.SetColor("_ColorTint",     GetHueColor(PulsingRim.material.GetColor("_ColorTint")));
        PulsingRim.material.SetColor("_RimColorOuter", GetHueColor(PulsingRim.material.GetColor("_RimColorOuter")));
        PulsingRim.material.SetColor("_RimColorInner", GetHueColor(PulsingRim.material.GetColor("_RimColorInner")));


        for(int i = 0 ; i <laserBeamManager.beams.Length; i++)
        {
            Material beamMat = laserBeamManager.beams[i].lineRenderer.material;
            beamMat.SetColor("_TintColor", GetHueColor(beamMat.GetColor("_TintColor")));
        }

    }

    Color GetHueColor(Color _color)
    {
        float h1, s1, b1;
        Color.RGBToHSV(color, out h1, out s1, out b1);

        float h2, s2, b2;
        Color.RGBToHSV(_color, out h2, out s2, out b2);

        Color rgbColor = Color.HSVToRGB(h1, s2, b2); 
        return new Color(rgbColor.r, rgbColor.g, rgbColor.b, _color.a);
    }

    
}
