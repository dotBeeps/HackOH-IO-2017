using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSongData : MonoBehaviour {

    public static ReadBeatmap.Metadata metadata;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
