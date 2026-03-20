using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
    public static SFXManager s_instance;
    private void Awake()
    {
        //Set the instance to thisif it's null
        if (s_instance == null)
            s_instance = this;
    }

    [SerializeField] private AudioSource m_bgAudioSource;
    [SerializeField] private AudioSource m_effectAudioSource;

    [SerializeField] private AudioClip m_calmBackgroundTrack01;
    [SerializeField] private AudioClip m_calmBackgroundTrack02;
    [SerializeField] private AudioClip m_battleBackgroundTrack01;

    

    public static void PlayBackground01()
    {
        s_instance.StartCoroutine(s_instance.AudioFade(SFXManager.s_instance.m_bgAudioSource, 2f, s_instance.m_calmBackgroundTrack01));
    }

    public static void PlayBackground02()
    {
        s_instance.StartCoroutine(s_instance.AudioFade(SFXManager.s_instance.m_bgAudioSource, 2f, s_instance.m_calmBackgroundTrack02));
    }

    public static void PlayBackgroundBattle()
    {
        s_instance.StartCoroutine(s_instance.AudioFade(SFXManager.s_instance.m_bgAudioSource, 2f, s_instance.m_battleBackgroundTrack01));
    }

    public static void PlayAudio(AudioClip audioClip)
    {
        SFXManager.s_instance.m_effectAudioSource.PlayOneShot(audioClip);
    }

    public IEnumerator AudioFade(AudioSource audioSource, float FadeTime, AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        audioSource.volume = 0;

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
