using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager1 : MonoBehaviour
{
    public static SFXManager1 instance;
    [SerializeField] private AudioClip[] clips;

    private AudioSource self;
    

    private void Awake()
    {
        instance = this;
        self = GetComponent<AudioSource>();
    }

    public void playClip(AudioClip clip)
    {
        self.clip = clip;
        if (self.isPlaying) self.Stop();
        self.Play();
    }

    public void playItemPickup()
    {
        self.clip = clips[0];
        self.Play();
    }

    public void playDialogueSFX()
    {
        self.clip = clips[1];
        self.Play();
    }

    public void stopClip()
    {
        self.Stop();
    }
}
