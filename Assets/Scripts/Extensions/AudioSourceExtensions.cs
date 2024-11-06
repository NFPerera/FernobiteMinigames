using UnityEngine;

namespace _Main.Scripts.DevelopmentUtilities.Extensions
{
    public static class AudioSourceExtensions
    {
        public static void PlayThenToLoop(this AudioSource p_source, AudioClip p_introClip, AudioClip p_loopClip)
        {
            p_source.clip = p_loopClip;
            p_source.loop = true;
            p_source.PlayOneShot(p_introClip);
            p_source.PlayScheduled(AudioSettings.dspTime + p_introClip.length);
        }
    }
}