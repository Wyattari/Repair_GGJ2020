using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{

    Color color;
    public Color Color {
		get { return color;  }
		set {
            color = value;
            SetColor(color);
		}
    }

    public ParticleSystem LargeSparks;
    public ParticleSystem SmallSparks;
    public MeshRenderer Texture;
    public MeshRenderer PulsingRim;

    void SetColor(Color color) {
        var LMain = LargeSparks.main;
        var SMain = SmallSparks.main;
        LMain.startColor = GetHueColor(color, LargeSparks.main.startColor.color);
        SMain.startColor = GetHueColor(color, SmallSparks.main.startColor.color);

        Texture.material.SetColor("_TintColor", GetHueColor(color, Texture.material.GetColor("_TintColor")));
        PulsingRim.material.SetColor("_ColorTint", GetHueColor(color, PulsingRim.material.GetColor("_ColorTint")));
        PulsingRim.material.SetColor("_RimColorOuter", GetHueColor(color, PulsingRim.material.GetColor("_RimColorOuter")));
        PulsingRim.material.SetColor("_RimColorInner", GetHueColor(color, PulsingRim.material.GetColor("_RimColorInner")));
    }

    Color GetHueColor(Color hueColor, Color mainColor)
    {
        float h1, s1, b1;
        Color.RGBToHSV(hueColor, out h1, out s1, out b1);

        float h2, s2, b2;
        Color.RGBToHSV(mainColor, out h2, out s2, out b2);

        Color rgbColor = Color.HSVToRGB(h1, s2, b2); 
        return new Color(rgbColor.r, rgbColor.g, rgbColor.b, mainColor.a);
    }    
}
