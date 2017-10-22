using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPanel : MonoBehaviour {

    public Material BaseMaterial;
    public Material ColorMaterial;
    public Color ActiveColor;
    private Material material;

	// Use this for initialization
	void Start () {
        material = GetComponent<Renderer>().material;
	}
	
	public void TurnOn()
    {
        GetComponent<Renderer>().material = ColorMaterial;
        GetComponent<Renderer>().material.color = ActiveColor;
    }

    public void TurnOff()
    {
        GetComponent<Renderer>().material = BaseMaterial;
    }
}
