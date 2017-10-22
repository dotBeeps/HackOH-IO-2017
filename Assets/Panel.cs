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
    private PanelControl panelController;
    public DisplayPanel PanelRep;
    private bool turnOn = false;
    private int waitBeats = 0;

	// Use this for initialization
	void Start () {
        spotlightColor = OriginalColor;
        pRenderer = GetComponent<Renderer>();
        spotlight = transform.Find("DanceLight").gameObject;
        spotlightInverse = transform.Find("DanceLightI").gameObject;
        panelController = transform.parent.GetComponent<PanelControl>();
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
            if (waitBeats >= panelController.WaitBeats)
            {
                spotlightColor = SetVValue(OriginalColor, bright);
                spotlightMat.SetColor("_EmissionColor", spotlightColor);
                spotlightIMat.SetColor("_EmissionColor", spotlightColor);
                waitBeats = 0;
                PanelRep.TurnOff();
            } else
            {
                waitBeats++;
            }
        } else if (FloatCloseToEqual(GetSpotlightVib(), bright, .01f))
        {
            spotlightColor = SetVValue(OriginalColor, 0);
            spotlightMat.SetColor("_EmissionColor", spotlightColor);
            spotlightIMat.SetColor("_EmissionColor", spotlightColor);
            if (CheckForPlayer())
                panelController.Hit();
            else
                panelController.Miss();
            
        }
        
    }

    bool CheckForPlayer()
    {
        Collider[] overlap = Physics.OverlapCapsule(transform.position, transform.position + Vector3.up * 20f, 0.5f, -1, QueryTriggerInteraction.Collide);
        foreach (Collider col in overlap.Where(col => col.tag == "ControllerSphere"))
        {
            SteamVR_Controller.Input((int)col.transform.GetComponentInParent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(1000);
        }
        if (overlap.Any(col => col.tag == "MainCamera"))
        {
            foreach(GameObject gObj in GameObject.FindGameObjectsWithTag("ControllerSphere"))
            {
                SteamVR_Controller.Input((int)gObj.transform.GetComponentInParent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(1000);
            }
        }

        return overlap.Any(col => col.tag == "MainCamera" || col.tag == "ControllerSphere");
    }

    public void On()
    {
        PanelRep.TurnOn();
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
