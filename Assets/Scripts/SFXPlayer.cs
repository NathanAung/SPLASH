using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] SFXList;

    private void Awake(){
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int SFXNo){
        audioSource.clip = SFXList[SFXNo];
        audioSource.Play();
    }
}
