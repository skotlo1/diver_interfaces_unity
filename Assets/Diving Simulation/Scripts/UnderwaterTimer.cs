using UnityEngine;

public class UnderwaterTimer : MonoBehaviour
{
    public InformationManager info;
    [SerializeField] bool underwaterWhenDepthIsPositive = false;

    [SerializeField] float secondsUnderwater = 0f;
    public float SecondsUnderwater => secondsUnderwater;

    void Update()
    {
        if (info == null) info = FindObjectOfType<InformationManager>();
        if (info == null) return;

        float d = info.GetDepth();
        bool isUnderwater = underwaterWhenDepthIsPositive ? (d > 0f) : (d < 0f);
        if (isUnderwater) secondsUnderwater += Time.deltaTime;
    }

    public string FormatMMSS()
    {
        int m = (int)(secondsUnderwater / 60f);
        int s = (int)(secondsUnderwater % 60f);
        return m.ToString("00") + ":" + s.ToString("00");
    }
}