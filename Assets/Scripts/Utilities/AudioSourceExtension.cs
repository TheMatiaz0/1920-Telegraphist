using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtension
{
    public static void PlayOneShotRandomClip(this AudioSource source, AudioClip[] clips)
    {
        var rndClip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(rndClip);
    }
}
