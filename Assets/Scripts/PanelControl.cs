using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour {

    public float NumberPanels;
    public GameObject PlayAreaObject;
    private SteamVR_PlayArea playArea;


	// Use this for initialization
	void Start () {
        StartCoroutine(SetPanelBounds());
	}

    IEnumerator SetPanelBounds()
    {
        Valve.VR.HmdQuad_t rect = new Valve.VR.HmdQuad_t();

        while (!SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref rect))
            yield return new WaitForSeconds(0.1f);

        Vector3 newScale = new Vector3(Mathf.Abs(rect.vCorners0.v0 - rect.vCorners2.v0), this.transform.localScale.y, Mathf.Abs(rect.vCorners0.v2 - rect.vCorners2.v2));

        this.transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
