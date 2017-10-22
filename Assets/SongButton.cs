using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongButton : MonoBehaviour {

    private ReadBeatmap.Metadata metadata;
    private Text buttonText;
    public Canvas InfoCanvas;


    public void LoadSongData(string song)
    {
        Debug.Log("Attempting to load info about " + song);
        metadata = ReadBeatmap.ReadMap("Assets/Resources/Songs/" + song + "/" + "beatmap.txt");
        buttonText = GetComponentInChildren<Text>();
        buttonText.text = metadata.title;
    }

    public void StageSong()
    {
        InfoCanvas.gameObject.SetActive(true);
        InfoCanvas.transform.Find("Info").GetComponent<Text>().text = string.Format("Song: {0}\nHigh Score: {1}\nBest Combo: {2}", metadata.title, metadata.highScore, metadata.maxCombo);
        CurrentSongData.metadata = metadata;
        transform.parent.gameObject.SetActive(false);

    }
}
