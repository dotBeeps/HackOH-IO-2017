using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Panel : MonoBehaviour {

    private GameObject spotlight;
    private GameObject spotlightInverse;
    public Material BaseMaterial;
    public Material LitMaterial;
    private Renderer pRenderer;
    public float dim;
    public float bright;
    public Color OriginalColor;
    private Color spotlightColor;
    private Material spotlightMat;
    private Material spotlightIMat;

	// Use this for initialization
	void Start () {
        spotlightColor = OriginalColor;
        pRenderer = GetComponent<Renderer>();
        spotlight = transform.Find("DanceLight").gameObject;
        spotlightInverse = transform.Find("DanceLightI").gameObject;

        spotlightMat = spotlight.GetComponent<Renderer>().material;
        spotlightIMat = spotlightInverse.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

    void Beat()
    {
        if (FloatCloseToEqual(GetSpotlightVib(), dim, .01f))
        {
            spotlightColor = SetVValue(OriginalColor,bright);
            spotlightMat.SetColor("_EmissionColor", spotlightColor);
            spotlightIMat.SetColor("_EmissionColor", spotlightColor);
        } else if (FloatCloseToEqual(GetSpotlightVib(), bright, .01f))
        {
            spotlightColor = SetVValue(OriginalColor, 0);
            spotlightMat.SetColor("_EmissionColor", spotlightColor);
            spotlightIMat.SetColor("_EmissionColor", spotlightColor);
        }
        
    }

    public void On()
    {
        spotlightColor = SetVValue(OriginalColor, dim);
        spotlightMat.SetColor("_EmissionColor", spotlightColor);
        spotlightIMat.SetColor("_EmissionColor", spotlightColor);
    }

    float GetSpotlightVib()
    {
        float h, s, v;
        Color.RGBToHSV(spotlightMat.GetColor("_EmissionColor"), out h, out s, out v);
        return v;
    }

    Color SetVValue(Color color, float vVal)
    {
        float h;
        float s;
        float v;
        Color.RGBToHSV(color, out h, out s, out v);
        return Color.HSVToRGB(h, s, vVal);
    }

    bool FloatCloseToEqual(float f1, float f2, float epsilon)
    {
        return Mathf.Abs(f1 - f2) < epsilon;
    }

}
