using UnityEngine;
using UnityEngine.UI;

public class TimerPanelUI : MonoBehaviour
{
    [Header("Data")]
    public UnderwaterTimer timer;

    [Header("UI")]
    public Text timeLabel; 
    public CanvasGroup canvasGroup; 
    public bool startVisible = false;

    bool visible;

    void Awake() => SetVisible(startVisible, true);

    void Update()
    {
        if (!visible || timer == null || timeLabel == null) return;
        timeLabel.text = timer.FormatMMSS();
    }

    public void Toggle() => SetVisible(!visible, false);

    void SetVisible(bool v, bool instant)
    {
        visible = v;
        if (canvasGroup == null) { gameObject.SetActive(v); return; }

        if (instant)
        {
            canvasGroup.alpha = v ? 1f : 0f;
            canvasGroup.interactable = v;
            canvasGroup.blocksRaycasts = v;
            gameObject.SetActive(v || canvasGroup.alpha > 0f);
        }
        else
        {
            StopAllCoroutines();
            gameObject.SetActive(true);
            StartCoroutine(Fade(v ? 1f : 0f, 0.12f));
        }
    }

    System.Collections.IEnumerator Fade(float target, float dur)
    {
        float a0 = canvasGroup.alpha, t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = 1f - Mathf.Pow(1f - Mathf.Clamp01(t/dur), 3f);
            canvasGroup.alpha = Mathf.Lerp(a0, target, k);
            yield return null;
        }
        canvasGroup.alpha = target;
        bool on = target > 0.99f;
        canvasGroup.interactable = on;
        canvasGroup.blocksRaycasts = on;
        if (!on) gameObject.SetActive(false);
    }
}
