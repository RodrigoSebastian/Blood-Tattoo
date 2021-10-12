using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChange : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip backMusic;
    public AudioClip labMusic;


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.Stop();
            audioSource.clip = labMusic;
            audioSource.Play();
        }
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(StartFade(audioSource, 5, 0));
            audioSource.Stop();
            audioSource.clip = backMusic;
            audioSource.Play();
        }
    }

    public static IEnumerator StartFade(AudioSource audio, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audio.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audio.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }


}
