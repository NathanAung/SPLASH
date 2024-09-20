using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip levelTrack;
    public AudioClip bossTrack;
    public AudioClip victoryTrack;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeTrack(0);
    }

    public void ChangeTrack(int trackNo){
        switch(trackNo){
            case 0:
                audioSource.clip = levelTrack;
                audioSource.volume = 0.2f;
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = bossTrack;
                audioSource.volume = 0.2f;
                audioSource.Play();
                break;
            case 2:
                audioSource.Stop();
                audioSource.clip = victoryTrack;
                audioSource.volume = 0.5f;
                audioSource.PlayDelayed(3);
                break;
        }
    }
}
