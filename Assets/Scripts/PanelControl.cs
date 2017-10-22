using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelControl : MonoBehaviour {

    public GameObject PlayAreaObject;
    private SteamVR_PlayArea playArea;
    private float filledPanel = 70;
    public int beatsUntilFilled;
    public List<Panel> panels = new List<Panel>();
    private int test;
    private int beatNum = 0;
    public float bpm;
    public Text ScoreText;
    public AudioSource audioSource;
    ReadBeatmap.Metadata songMetadata;
    public string BeatmapPath;
    public string SongPath;
    private int score;
    private int combo;
    public int WaitBeats;
    private int waited;
    private bool startedPlaying;
    private int maxCombo;
    public Text ComboText;
    public List<string> InspirationalText = new List<string>();
    public List<Color> MotiviationalColors = new List<Color>();

    public float BeatsToSeconds(int beats)
    {
        return (60 / bpm) * beats;
    }

    public void Start()
    {
        songMetadata = CurrentSongData.metadata;
        Debug.Log("Loading audio from: " + "Songs/" + songMetadata.title + "/" + songMetadata.title);
        AudioClip audio = Resources.Load("Songs/" + songMetadata.title + "/" + songMetadata.title)as AudioClip;
        audio.LoadAudioData();
        audioSource.clip = audio;
        bpm = songMetadata.bpm;
        PlaySong();
    }


    public void PlaySong()
    {
        InvokeRepeating("Beat", 0f, 60 / bpm);
        foreach (KeyValuePair<float, int> beat in songMetadata.songData)
        {
            StartCoroutine(DelayedTurnOnPanel(beat.Value, beat.Key));
        }
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

    IEnumerator DelayedReturnToMenu(float wait)
    {
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(0);
    }

    void Beat()
    {
        if (waited < WaitBeats)
        {
            waited++;
        } else if (!startedPlaying)
        {
            audioSource.Play();
            startedPlaying = true;
        } else if (startedPlaying && !audioSource.isPlaying)
        {
            // Finished song
            string[] lines = File.ReadAllLines("Assets/Resources/Songs/" + CurrentSongData.metadata.title + "/beatmap.txt");
            if (score > int.Parse(lines[2])) lines[2] = score.ToString();
            if (maxCombo > int.Parse(lines[3])) lines[3] = maxCombo.ToString();
            File.WriteAllLines("Assets/Resources/Songs/" + CurrentSongData.metadata.title + "/beatmap.txt", lines);
            StartCoroutine(DelayedReturnToMenu(2f));
        }
        panels.ForEach(panel => panel.SendMessage("Beat", SendMessageOptions.DontRequireReceiver));
    }

    public void Hit()
    {
        combo++;
        ComboText.text = InspirationalText[Random.Range(0, InspirationalText.Count-1)] + " - " + combo + " Combo!!!";
        ComboText.color = MotiviationalColors[Random.Range(0, InspirationalText.Count-1)];
        score += Mathf.FloorToInt(combo * 1.25f);
        if (combo > maxCombo) maxCombo = combo;
    }

    public void Miss()
    {
        ComboText.text = ":(";
        ComboText.color = Color.red;
        combo = 0;
    }

    // Update is called once per frame
    void Update () {
        ScoreText.text = string.Format("Score: {0}\nCombo: {1}", score, combo);
	}
}
