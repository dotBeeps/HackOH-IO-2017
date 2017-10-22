using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour {

    public GameObject PlayAreaObject;
    private SteamVR_PlayArea playArea;
    private float filledPanel = 70;
    public int beatsUntilFilled;
    public List<Panel> panels = new List<Panel>();
    private int test;
    private int beatNum = 0;

	// Use this for initialization
	void Start () {

	}


    public void Beat()
    {
        beatNum++;
        panels.ForEach(panel => panel.SendMessage("Beat",SendMessageOptions.DontRequireReceiver));
        if (beatNum % 2 == 0)
        {
            panels[test].On();
            test++;
            if (test > 5) test = 0;
        }
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown("l"))
        {
            panels[0].On();
        }
	}
}
