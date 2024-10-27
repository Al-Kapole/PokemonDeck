using UnityEngine;
using System.Threading.Tasks;

public class FadeTransition
{
    public enum Fade { IN, OUT}

    /// <summary>
    /// Transit from one CanvasGroup to an other by fading out the first one and fading in the second one.
    /// </summary>
    /// <param name="from">CanvasGroup to leave from and fade out.</param>
    /// <param name="to">CanvasGroup to go to and fade in.</param>
    /// <param name="time">Second that the transition should take.</param>
    /// <param name="onCompletion">Callback that is called once the transition is complete.</param>
    public static async void Transition(CanvasGroup from, CanvasGroup to, float time = 1f, System.Action onCompletion = null)
    {
        from.interactable = false;
        to.interactable = false;
        float halfTime = time * 0.5f;
        float t = 1;
        while(t > 0)
        {
            t -= Time.deltaTime / halfTime;
            from.alpha = t;
            await Task.Yield();
        }
        from.gameObject.SetActive(false);
        t = 0;
        to.gameObject.SetActive(true);
        while(t < 1)
        {
            t += Time.deltaTime / halfTime;
            to.alpha = t;
            await Task.Yield();
        }
        onCompletion?.Invoke();
        to.interactable = true;
    }

    /// <summary>
    /// Fade a CanvasGroup. In or Out.
    /// </summary>
    /// <param name="canvas">The CanvasGroup to be faded.</param>
    /// <param name="fade">Type of fade. In or Out</param>
    /// <param name="time">Second that the fade should take.</param>
    /// <param name="onCompletion">Callback that is called once the fade is complete.</param>
    /// <returns></returns>
    public static async Task FadeCanvas(CanvasGroup canvas, Fade fade, float time = 1f, System.Action onCompletion = null)
    {
        float t;
        if (fade == Fade.IN)
        {
            canvas.gameObject.SetActive(true);
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / time;
                canvas.alpha = t;
                await Task.Yield();
            }
        }
        else
        {
            t = 1;
            while (t > 0)
            {
                t -= Time.deltaTime / time;
                canvas.alpha = t;
                await Task.Yield();
            }
            canvas.gameObject.SetActive(false);
        }
        onCompletion?.Invoke();
    }
    /// <summary>
    /// Fade in for UI Image.
    /// </summary>
    /// <param name="image">UI Image</param>
    /// <param name="time">Second that the fade should take.</param>
    /// <param name="onCompletion">Callback that is called once the fade is complete.</param>
    public static async void FadeIn(UnityEngine.UI.Image image, float time = 1f, System.Action onCompletion = null)
    {
        Color newColor = image.color;
        float t = 0;
        image.gameObject.SetActive(true);
        while (t < 1)
        {
            t += Time.deltaTime / time;
            newColor.a = t;
            image.color = newColor;
            await Task.Yield();
        }
        onCompletion?.Invoke();
    }
}
