using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadBeatmap : MonoBehaviour {

    private bool init = false;
    private int i = 0;
    public string mapPath;

    public struct Metadata
    {
        //example values idk what's actually going to be in the file
        public string title;
        public string artist;
        public string songPath;

        public float beatmapOffset;

        public float bpm;
    }

    public List<int> songData;
    public List<float> songTimes;

    Metadata metaData = new Metadata();

    // Use this for initialization
    void Start () {
        string[] fileData = File.ReadAllLines(mapPath);
        //initialize metadata & do whatever with it
        for(int i = 0; i < fileData.Length; i += 2)
        {
            songTimes[i / 2] = float.Parse(fileData[i]);
            songData[i / 2] =  int.Parse(fileData[i + 1]);
        }
    }

    // Update is called once per frame
    void Update () {
        if (init && i < songData.Count)
        {
            StartCoroutine(PlayBeat(songData[i]));
            i++;
        }
	}
    
    IEnumerator PlayBeat(int square)
    {
        //do game stuff in the game square
        yield return new WaitForSeconds(0 /*time delay*/);
    }
}
