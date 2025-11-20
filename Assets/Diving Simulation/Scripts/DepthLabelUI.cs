using UnityEngine;
#if TMP_PRESENT || UNITY_TEXTMESHPRO
using TMPro;
#endif
using UnityEngine.UI;

public class DepthLabelUI : MonoBehaviour
{
    [Header("data")]
    public InformationManager info;

    [Header("ui (assign ONE)")]
    public Text uiText;
#if TMP_PRESENT || UNITY_TEXTMESHPRO
    public TMP_Text tmpText;
#endif

    [Header("format")]
    public string prefix = "Depth: ";
    public string suffix = " m";

    [Header("behavior")]
    [Tooltip("Your sim uses NEGATIVE values when underwater. Leave true for your project.")]
    public bool simUsesNegativeUnderwater = true;

    [Tooltip("Anything within ±this of the surface is shown as exactly 0.")]
    public float surfaceEpsilon = 0.05f; // meterz

    static T FindFirst<T>() where T : Object
    {
#if UNITY_6000_0_OR_NEWER
        return Object.FindFirstObjectByType<T>(FindObjectsInactive.Exclude);
#else
        return Object.FindObjectOfType<T>();
#endif
    }

    void Reset()
    {
        if (!uiText) uiText = GetComponent<Text>();
#if TMP_PRESENT || UNITY_TEXTMESHPRO
        if (!uiText && !tmpText) tmpText = GetComponent<TMP_Text>();
#endif
        if (!info) info = FindFirst<InformationManager>();
    }

    void Awake()
    {
        if (!info) info = FindFirst<InformationManager>();
    }

    void Update()
    {
        if (!info) return;

        float raw = info.GetDepth();

        float displayDepth;

        if (simUsesNegativeUnderwater)// negatif only when below surface (i think we only want 0 when abov water)
        {
            displayDepth = raw;
        }
        else
        {
            displayDepth = -raw;
        }

        if (Mathf.Abs(displayDepth) < surfaceEpsilon)
            displayDepth = 0f;

        string s = prefix + displayDepth.ToString("0.0") + suffix;

#if TMP_PRESENT || UNITY_TEXTMESHPRO
        if (tmpText) { tmpText.text = s; return; }
#endif
        if (uiText) uiText.text = s;
    }
}
