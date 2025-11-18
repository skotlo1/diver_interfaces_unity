using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class EmergencyGesturesMiddleFinger : MonoBehaviour
{
    [Header("call")]
    public SimpleDial dial;
    public CallTowerManager callTower;

    [Header("use left hand here")]
    public bool useLeftHand = true;

    [Header("Finger Tips")]
    public Transform leftThumbTip, leftMiddleTip;
    public Transform rightThumbTip, rightMiddleTip;

    [Header("Tuning")]
    public float pinchThreshold = 0.022f;
    public float cooldown = 1.5f;

    [Header("Debugging why wont this workkk")]
    public bool showOverlay = true;
    public bool allowKeyboardTest = true; 

    float nextAllowed;
    bool wasPinching;
    float lastDist = -1f;
    string lastMsg = "";

    void Reset() {
        if (!dial) dial = FindObjectOfType<SimpleDial>(true);
        if (!callTower) callTower = FindObjectOfType<CallTowerManager>(true);
    }

    void Update()
    {
        #if ENABLE_INPUT_SYSTEM
        if (allowKeyboardTest && Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
            Trigger("Keyboard[J]");
        #endif

        if (!dial || !callTower) return;

        Transform thumb = useLeftHand ? leftThumbTip  : rightThumbTip;
        Transform mid   = useLeftHand ? leftMiddleTip : rightMiddleTip;
        if (!thumb || !mid) return;

        lastDist = Vector3.Distance(thumb.position, mid.position);
        bool pinching = lastDist <= pinchThreshold;

        if (pinching && !wasPinching && Time.time >= nextAllowed)
        {
            Trigger("Thumb–Middle Pinch");
            nextAllowed = Time.time + cooldown;
        }
        wasPinching = pinching;
    }

    public void Trigger(string src)
    {
        int f = callTower.GetEmergencyFrequency();
        dial.QuickDial(f);
        lastMsg = $"[{src}] Dialed {f} ({(useLeftHand ? "L" : "R")} hand) @ {Time.time:F1}s";
        Debug.Log($"[EmergencyGesturesMiddleFinger] {lastMsg}");
    }

    void OnGUI()
    {
        if (!showOverlay) return;
        var r = new Rect(10, 10, 360, 72);
        GUI.color = new Color(0,0,0,0.6f); GUI.Box(r, GUIContent.none); GUI.color = Color.white;
        GUILayout.BeginArea(r);
        GUILayout.Label($"Emergency (Thumb–Middle, {(useLeftHand ? "LEFT" : "RIGHT")})");
        GUILayout.Label($"dist: {(lastDist < 0 ? "n/a" : lastDist.ToString("0.000"))}  thr: {pinchThreshold:0.000}");
        if (!string.IsNullOrEmpty(lastMsg)) GUILayout.Label(lastMsg);
        GUILayout.EndArea();
    }
}
