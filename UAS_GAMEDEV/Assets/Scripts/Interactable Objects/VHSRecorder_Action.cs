using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSRecorder_Action : PuzzleAction
{
    [SerializeField] private AudioSource src;
    public override void action()
    {
        if (!src.isPlaying) src.Play();
        Debug.Log(src.isPlaying);
    }
}
