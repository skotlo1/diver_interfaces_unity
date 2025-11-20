using UnityEngine;

public class CubeStatsFollowHead : MonoBehaviour
{
    public Transform centerEye;
    public Vector3  localPos  = new Vector3(0.12f, -0.06f, 0.45f);
    public Vector3  localEuler= Vector3.zero;
    public bool     pin = true;

    Transform originalParent;
    Vector3 origLocalPos, origLocalEuler;

    void Awake()
    {
        originalParent = transform.parent;
        origLocalPos   = transform.localPosition;
        origLocalEuler = transform.localEulerAngles;
        if (!centerEye && Camera.main) centerEye = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (pin && centerEye)
        {
            if (transform.parent != centerEye)
                transform.SetParent(centerEye, false);
            transform.localPosition    = localPos;
            transform.localEulerAngles = localEuler;
        }
        else if (transform.parent != originalParent)
        {
            transform.SetParent(originalParent, false);
            transform.localPosition    = origLocalPos;
            transform.localEulerAngles = origLocalEuler;
        }
    }
}
