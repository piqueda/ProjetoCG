using System;
using System.Collections;
using UnityEngine;

public class PlaySnarl : MonoBehaviour
{
  [Header("Configurações de Fade")]
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float maxVolume = 1f;

    private AudioSource audioSource;
    private Coroutine currentRoutine;
    private bool isFading = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
    }

    // 🔊 Função principal
    public void PlayAudio()
    {
       
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PlayAudioRoutine());
    }

    private IEnumerator PlayAudioRoutine()
    {
        isFading = true;

        // Garante volume inicial zero
        audioSource.Stop();
        audioSource.volume = 0f;
        audioSource.Play();

        // ✅ Fade In
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, maxVolume, t / fadeInDuration);
            yield return null;
        }
        audioSource.volume = maxVolume;

        isFading = false;

        // Espera o som terminar
        yield return new WaitWhile(() => audioSource.isPlaying);

        // ✅ Fade Out
        isFading = true;
        t = 0f;
        float startVol = audioSource.volume;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0f, t / fadeOutDuration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();

        isFading = false;
        currentRoutine = null;
    }

    // 👀 Aqui está a função que você pediu:
    public bool IsPlaying()
    {
        // Retorna true se o som estiver tocando ou em processo de fade
        return audioSource.isPlaying || isFading;
    }
}
