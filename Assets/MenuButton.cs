using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    public GameObject NewSongButton;
    public Canvas MusicList;
    public Canvas InfoCanvas;

	public void LoadSongList()
    {
        MusicList.gameObject.SetActive(true);
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Songs");
        foreach (DirectoryInfo subdir in dir.GetDirectories())
        {
            GameObject newButton = Instantiate(NewSongButton,MusicList.transform);
            newButton.GetComponent<SongButton>().LoadSongData(subdir.Name);
            newButton.GetComponent<SongButton>().InfoCanvas = InfoCanvas;
        }
        transform.parent.gameObject.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
