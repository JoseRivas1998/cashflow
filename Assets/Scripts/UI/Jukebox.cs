using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jukebox : MonoBehaviour
{

    [System.Serializable]
    public struct Song
    {
        public string title;
        public string artist;
        public AudioClip audio;
    }

    public Song[] music;
    public AudioSource audioSource;
    public float maxVolume = 0.8f;

    public Text title;
    public RawImage playPauseImage;
    public Texture playButton;
    public Texture pauseButton;

    private bool playing;
    private Song currentSong;
    private CardStack<Song> musicStack;

    void Awake()
    {
        musicStack = new CardStack<Song>(new List<Song>(music));
        playing = true;
        currentSong = musicStack.Pop();
        audioSource.clip = currentSong.audio;
        title.text = currentSong.title + " - " + currentSong.artist;
        audioSource.Play();
        UpdateVolume(1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(playing && !audioSource.isPlaying)
        {
            NextSong();
        }
    }

    public void UpdateVolume(float vol)
    {
        audioSource.volume = Mathf.Lerp(0, maxVolume, vol);
    }

    public void PlayPause()
    {
        if (playing)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }

    public void NextSong()
    {
        audioSource.Stop();
        currentSong = musicStack.Pop();
        audioSource.clip = currentSong.audio;
        audioSource.Play();
        title.text = currentSong.title + " - " + currentSong.artist;
        playing = true;
        playPauseImage.texture = pauseButton;
    }

    private void Play()
    {
        playing = true;
        audioSource.UnPause();
        playPauseImage.texture = pauseButton;
    }

    private void Pause()
    {
        playing = false;
        playPauseImage.texture = playButton;
        audioSource.Pause();
    }

}
