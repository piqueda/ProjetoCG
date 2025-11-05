using UnityEngine;
using System.Collections;

public class MainAudioManager : MonoBehaviour
{
    [Header("Fontes de Áudio")]
    public AudioSource backgroundMusic;
    public PlaySnarl specialSound;

    [Header("Configurações")]
    public float fadeDuration = 1f; // tempo para suavizar a transição
    public float reducedVolume = 0.2f; // volume da música enquanto o som toca

    private float originalVolume;

    public void PlaySpecialSound()
    {
        
       
        if (specialSound == null || backgroundMusic == null) return;

        StartCoroutine(HandleSpecialSound());
    }

    private IEnumerator HandleSpecialSound()
    {
        // guarda o volume original
        originalVolume = backgroundMusic.volume;

        // reduz o volume com fade
        yield return StartCoroutine(FadeVolume(backgroundMusic, reducedVolume, fadeDuration));

        // toca o som especial
        specialSound.PlayAudio();
       
        {
            
        }
        // espera ele terminar
        yield return new WaitWhile(() => specialSound.IsPlaying());

        // volta o volume original com fade
        yield return StartCoroutine(FadeVolume(backgroundMusic, originalVolume, fadeDuration));
    }

    private IEnumerator FadeVolume(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
}
