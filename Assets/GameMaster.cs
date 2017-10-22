using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    private int score;
    public static float bpm;
    public float BPM;
    public GameObject PanelMaster;

    // Use this for initialization
    void Start () {
        bpm = BPM;
        InvokeRepeating("Beat", 0f, 60 / bpm);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Beat()
    {
        PanelMaster.SendMessage("Beat", SendMessageOptions.DontRequireReceiver);
    }

    public static float BeatsToSeconds(int beats)
    {
        return (60 / bpm) * beats;
    }
}
