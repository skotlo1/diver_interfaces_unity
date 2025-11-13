using UnityEngine;
using UnityEngine.UI;
#if TMP_PRESENT || UNITY_TEXTMESHPRO
using TMPro;
#endif

public class TimerMenuLabel : MonoBehaviour
{
    [Header("data")]
    public UnderwaterTimer timer;

    [Header("ui (assign ONE)")]
    public Text uiText; 
    #if TMP_PRESENT || UNITY_TEXTMESHPRO
    public TMP_Text tmpText; //thik might use textmesh pro
    #endif

    [Header("format")]
    [TextArea] public string prefix = "Time Spent Underwater: ";

    void Reset()
    {
        uiText = GetComponent<Text>();
        #if TMP_PRESENT || UNITY_TEXTMESHPRO
        if (uiText == null) tmpText = GetComponent<TMP_Text>();
        #endif
        if (timer == null) timer = FindObjectOfType<UnderwaterTimer>();
    }

    void Awake()
    {
        if (timer == null) timer = FindObjectOfType<UnderwaterTimer>();
    }

    void Update()
    {
        if (timer == null) return;
        string s = prefix + timer.FormatMMSS();

        #if TMP_PRESENT || UNITY_TEXTMESHPRO
        if (tmpText != null) { tmpText.text = s; return; }
        #endif
        if (uiText != null) uiText.text = s;
    }
}
