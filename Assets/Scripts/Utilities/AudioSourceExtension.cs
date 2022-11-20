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

    public static bool IsReversePitch(this AudioSource source)
    {
        return source.pitch < 0f;
    }

    public static float GetClipRemainingTime(this AudioSource source)
    {
        // Calculate the remainingTime of the given AudioSource,
        // if we keep playing with the same pitch.
        float remainingTime = (source.clip.length - source.time) / source.pitch;
        return source.IsReversePitch() ?
            (source.clip.length + remainingTime) :
            remainingTime;
    }
}
