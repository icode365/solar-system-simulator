// ═══ Animation Configuration ═══

using System.Threading.Tasks;
using UnityEngine;

public struct UIScreenAnimationConfig
{
    public float FadeInDuration { get; set; }
    public float FadeOutDuration { get; set; }
    public AnimationCurve FadeInCurve { get; set; }
    public AnimationCurve FadeOutCurve { get; set; }

    public static UIScreenAnimationConfig Default => new()
    {
        FadeInDuration = 0.2f,
        FadeOutDuration = 0.25f,
        FadeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1),
        FadeOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0)
    };
}

// ═══ Extension Methods (adds behavior to IUIScreen without inheritance) ═══
public static class UIScreenAnimationExtensions
{
    public static async Task ShowWithFadeInAsync(
        this MainMenuUIHandler screen,
        UIScreenAnimationConfig? config = null)
    {
        config ??= UIScreenAnimationConfig.Default;
        await FadeAsync(screen.CanvasGroup, 0, 1, config.Value.FadeInDuration, config.Value.FadeInCurve);
    }

    public static async Task HideWithFadeOutAsync(
        this MainMenuUIHandler screen,
        UIScreenAnimationConfig? config = null)
    {
        config ??= UIScreenAnimationConfig.Default;
        await FadeAsync(screen.CanvasGroup, 1, 0, config.Value.FadeOutDuration, config.Value.FadeOutCurve);
    }

    private static async Task FadeAsync(
        CanvasGroup canvasGroup,
        float startAlpha,
        float endAlpha,
        float duration,
        AnimationCurve curve)
    {
        if (canvasGroup == null) return;

        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Min(elapsed / duration, 1f);
            float easedT = Mathf.SmoothStep(0, 1, t);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, easedT);
            await Task.Yield();
        }

        canvasGroup.alpha = endAlpha;
    }
}