using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class BloodEffect : MonoBehaviour
{
    [Header("Configurações da Hit Screen")]
    [SerializeField] private CanvasGroup hitScreenCanvasGroup;
    [SerializeField] private float fadeInDuration = 0.1f;
    [SerializeField] private float displayDuration = 0.2f;
    [SerializeField] private float fadeOutDuration = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeHitScreen();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeHitScreen()
    {
        if (hitScreenCanvasGroup != null)
        {
            hitScreenCanvasGroup.alpha = 0f;
            hitScreenCanvasGroup.interactable = false;
            hitScreenCanvasGroup.blocksRaycasts = false;
        }
    }

     // Método público para ativar o efeito
    public void ShowHitEffect()
    {
        if (hitScreenCanvasGroup != null && gameObject.activeInHierarchy)
        {
            
            StartCoroutine(HitEffectRoutine());
        }
    }

    private IEnumerator HitEffectRoutine()
    {
        // esperar o audio estar em 4s
        yield return new WaitForSeconds(4f);


        // Fade In rápido
        yield return StartCoroutine(FadeCanvasGroup(0f, 1f, fadeInDuration));
        
        // Aguarda tempo de exibição
        yield return new WaitForSeconds(displayDuration);
        
        // Fade Out suave
        yield return StartCoroutine(FadeCanvasGroup(1f, 0f, fadeOutDuration));
    }

    private IEnumerator FadeCanvasGroup(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            hitScreenCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        hitScreenCanvasGroup.alpha = targetAlpha;
    }

    // Método para configurar via código se necessário
    public void SetHitScreenConfig(float fadeIn, float display, float fadeOut)
    {
        fadeInDuration = fadeIn;
        displayDuration = display;
        fadeOutDuration = fadeOut;
    }
    
}
