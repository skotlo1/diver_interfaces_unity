using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using UnityEngine.SubsystemsImplementation;

public class EmergencyPalmXRHands : MonoBehaviour
{
    public SimpleDial dial;
    public CallTowerManager callTower;
    [Tooltip("≈ 10 cm; increase if it's hard to trigger and vice versa lol")]
    public float triggerDistanceMeters = 0.10f;
    public float cooldown = 1.5f;
    public bool showOverlay = true;

    XRHandSubsystem hands;
    float nextAllowed, lastDist = -1f;
    string lastMsg = "";

    void OnEnable()
    {
        if (!dial) dial = FindFirstObjectByType<SimpleDial>(FindObjectsInactive.Exclude);
        if (!callTower) callTower = FindFirstObjectByType<CallTowerManager>(FindObjectsInactive.Exclude);

        hands = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();
        if (hands == null)
        {
            SubsystemManager.GetSubsystems(tmp);
            if (tmp.Count > 0) hands = tmp[0];
            tmp.Clear();
        }
        if (hands == null) Debug.LogWarning("[EmergencyClap] ClapSystem not found.");
    }
    static readonly System.Collections.Generic.List<XRHandSubsystem> tmp = new();

    void Update()
    {
        if (hands == null || !dial || !callTower) return;

        var lh = hands.leftHand;
        var rh = hands.rightHand;
        if (!lh.isTracked || !rh.isTracked) { lastDist = -1f; return; }

        if (!TryWrist(lh, out var lw) || !TryWrist(rh, out var rw)) { lastDist = -1f; return; }

        lastDist = Vector3.Distance(lw.position, rw.position);

        if (lastDist <= triggerDistanceMeters && Time.time >= nextAllowed)
        {
            nextAllowed = Time.time + cooldown;
            int f = callTower.GetEmergencyFrequency();
            dial.QuickDial(f);
            lastMsg = $"[Palm XRHands] Dialed {f}   dist={lastDist:0.000} m";
            Debug.Log(lastMsg);
        }
    }

    bool TryWrist(XRHand h, out Pose p)
    {
        p = default;
        var j = h.GetJoint(XRHandJointID.Wrist);
        return j.TryGetPose(out p);
    }

    void OnGUI()
    {
        if (!showOverlay) return;
        var r = new Rect(10, 100, 420, 70);
        GUI.color = new Color(0,0,0,0.6f); GUI.Box(r, GUIContent.none); GUI.color = Color.white;
        GUILayout.BeginArea(r);
        GUILayout.Label("Emergency Palm Clap (XR Hands: Wrist–Wrist)");
        GUILayout.Label($"dist: {(lastDist<0? "n/a" : lastDist.ToString("0.000"))} m   thr: {triggerDistanceMeters:0.000}");
        if (!string.IsNullOrEmpty(lastMsg)) GUILayout.Label(lastMsg);
        GUILayout.EndArea();
    }
}
