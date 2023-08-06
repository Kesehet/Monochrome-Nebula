using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introClip;
    public AudioClip mainClip;

    private bool introFinished = false;

    private void Start()
    {
        // Start playing the intro clip
        if (introClip != null)
        {
            audioSource.clip = introClip;
            audioSource.Play();
            StartCoroutine(PlayMainAudioAfterIntro());
        }
        else
        {
            // If there's no intro clip, play the main clip immediately
            PlayMainAudio();
        }
    }

    private IEnumerator PlayMainAudioAfterIntro()
    {
        // Wait until the intro clip finishes playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // Intro clip has finished, play the main clip
        PlayMainAudio();
    }

    private void PlayMainAudio()
    {
        // Loop the main audio clip
        if (mainClip != null)
        {
            audioSource.clip = mainClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Main audio clip is not assigned!");
        }
    }
}
