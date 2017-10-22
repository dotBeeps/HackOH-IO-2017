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
    public float bpm;
    public AudioSource audioSource;
    ReadBeatmap.Metadata songMetadata;
    public string BeatmapPath;
    public string SongPath;

    public float BeatsToSeconds(int beats)
    {
        return (60 / bpm) * beats;
    }

    public void LoadSong()
    {
        songMetadata = ReadBeatmap.ReadMap(BeatmapPath);
        AudioClip audio = Resources.Load(SongPath) as AudioClip;
        audio.LoadAudioData();
        audioSource.clip = audio;
        bpm = songMetadata.bpm;
    }

    public void PlaySong()
    {
        InvokeRepeating("Beat", 0f, 60 / bpm);
        foreach (KeyValuePair<float, int> beat in songMetadata.songData)
        {
            StartCoroutine(DelayedTurnOnPanel(beat.Value, beat.Key));
        }
        
        audioSource.Play();
    }

    IEnumerator DelayedTurnOnPanel(int i, float wait)
    {
        yield return new WaitForSeconds(wait);
        panels[i].On();
    }

    public void TurnOnPanel(int i)
    {
        panels[i].On();
    }

    void Beat()
    {

        panels.ForEach(panel => panel.SendMessage("Beat", SendMessageOptions.DontRequireReceiver));

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("l"))
        {
            LoadSong();
            PlaySong();
        }
	}
}
