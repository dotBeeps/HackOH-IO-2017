using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class ReadBeatmap : MonoBehaviour {

    private bool init = false;
    private int i = 0;
    public string mapPath;

    public struct Metadata
    {
        //example values idk what's actually going to be in the file
        public string title;
        public string highScore;
        public string maxCombo;
        public string beatmapVersion;
        public Dictionary<float, int> songData;
        public float bpm;
    }


    public Dictionary<float, int> songData = new Dictionary<float, int>(); 

    Metadata metaData = new Metadata();

    // Use this for initialization
    void Start () {

    }

    public static Metadata ReadMap(string path)
    {
        Dictionary<float, int> data = new Dictionary<float, int>();
        Metadata metadata = new Metadata();
        string[] fileData = File.ReadAllLines(path);
        for(int i = 4; i < fileData.Length-1; i += 2)
        {
            data.Add(float.Parse(fileData[i]), int.Parse(fileData[i + 1]));
        }
        metadata.beatmapVersion = fileData[0];
        metadata.title = fileData[1];
        metadata.highScore = fileData[2];
        metadata.maxCombo = fileData[3];
        metadata.songData = data;
        Regex r = new Regex(@"[0-9]+\.[0-9]+");
        Debug.Log(fileData.Last());
        Debug.Log(r.Match(fileData.Last()).Value);
        metadata.bpm = float.Parse(r.Match(fileData.Last()).Value);
        return metadata;
    }


   
}
