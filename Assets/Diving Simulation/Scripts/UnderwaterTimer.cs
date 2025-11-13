using UnityEngine;

public class UnderwaterTimer : MonoBehaviour
{
    public InformationManager info;=
    public float SecondsUnderwater { get; private set; }

    void Update()
    {
        if (info != null && info.GetDepth() < 0f)
            SecondsUnderwater += Time.deltaTime;
    }

    public string FormatMMSS()
    {
        int m = Mathf.FloorToInt(SecondsUnderwater / 60f);
        int s = Mathf.FloorToInt(SecondsUnderwater % 60f);
        return $"{m:00}:{s:00}";
    }

    public void ResetTimer() => SecondsUnderwater = 0f;
}
