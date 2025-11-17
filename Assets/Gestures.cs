using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

public class HandGestureController : MonoBehaviour
{
    [Header("Gesture Settings")]
    [SerializeField] private float holdDuration = 0.5f;
    [SerializeField] private Handedness gestureHand = Handedness.Right;
    
    [Header("Button References")]
    //Access to dialing buttons
    [SerializeField] private GameObject callButtons;
    //Access to crew member buttons
    [SerializeField] private GameObject crewButtons;
    
    private XRHandSubsystem m_Subsystem;
    private float currentHoldTime = 0f;
    private bool isMenuVisible = false;
    
    static readonly List<XRHandSubsystem> s_SubsystemsReuse = new List<XRHandSubsystem>();
    
    void Start()
    {
        if (callButtons) callButtons.SetActive(false);
        if (crewButtons) crewButtons.SetActive(false);
    }
    
    void Update()
    {
        if (m_Subsystem == null || !m_Subsystem.running)
        {
            SubsystemManager.GetSubsystems(s_SubsystemsReuse);
            for (var i = 0; i < s_SubsystemsReuse.Count; ++i)
            {
                if (s_SubsystemsReuse[i].running)
                {
                    m_Subsystem = s_SubsystemsReuse[i];
                    break;
                }
            }
        }
        
        if (m_Subsystem == null) return;
        
        XRHand hand = gestureHand == Handedness.Left ? m_Subsystem.leftHand : m_Subsystem.rightHand;
        
        if (!hand.isTracked)
        {
            HideMenu();
            return;
        }
        
        int fingerCount = CountExtendedFingers(hand);
        
        if (fingerCount == 1)
        {
            ShowCallMenu();
            currentHoldTime = 0f;
        }
        else if (fingerCount == 2)
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime >= holdDuration)
            {
                ShowCrewInfoMenu();
            }
        }
        else
        {
            HideMenu();
            currentHoldTime = 0f;
        }
    }
    
    int CountExtendedFingers(XRHand hand)
    {
        int count = 0;
        
        for (int fingerIndex = (int)XRHandFingerID.Thumb; 
             fingerIndex <= (int)XRHandFingerID.Little; 
             fingerIndex++)
        {
            if (IsFingerExtended(hand, (XRHandFingerID)fingerIndex))
                count++;
        }
        
        return count;
    }
    
    bool IsFingerExtended(XRHand hand, XRHandFingerID fingerId)
    {
        var tipJoint = hand.GetJoint(fingerId.GetFrontJointID());
        var baseJoint = hand.GetJoint(fingerId.GetBackJointID());
        
        if (!tipJoint.TryGetPose(out var tipPose) || !baseJoint.TryGetPose(out var basePose))
            return false;
        
        float distance = Vector3.Distance(tipPose.position, basePose.position);
        float threshold = fingerId == XRHandFingerID.Thumb ? 0.04f : 0.06f;
        
        return distance > threshold;
    }
    
    void ShowCallMenu()
    {
        
        if (callButtons) callButtons.SetActive(true);
        if (crewButtons) crewButtons.SetActive(false);
        
        isMenuVisible = true;
    }
    
    void ShowCrewInfoMenu()
    {
        
        if (callButtons) callButtons.SetActive(false);
        if (crewButtons) crewButtons.SetActive(true);
        
        isMenuVisible = true;
    }
    
    void HideMenu()
    {
        if (callButtons) callButtons.SetActive(false);
        if (crewButtons) crewButtons.SetActive(false);
        isMenuVisible = false;
    }
}