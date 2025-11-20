using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

public class HandGestureController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private GameObject callButtons;
    [SerializeField] private GameObject crewPanel;
    [SerializeField] private GameObject crewPanelButtons;
    
    [Header("Gesture Settings")]
    [SerializeField] private float extendedThreshold = 0.05f;
    [SerializeField] private float gestureHoldTime = 0.3f;
    
    private XRHandSubsystem handSubsystem;
    private static readonly List<XRHandSubsystem> s_Subsystems = new List<XRHandSubsystem>();
    
    private bool callMenuActive = false;
    private bool crewMenuActive = false;
    private float oneFingerTimer = 0f;
    private float twoFingerTimer = 0f;
    private float fistTimer = 0f;
    
    void Start()
    {
        if (callButtons) callButtons.SetActive(false);
        if (crewPanel) crewPanel.SetActive(false);
        checkCrewButtons(false);
    }

    void checkCrewButtons(bool turnOn) 
    {
        if (crewPanelButtons) {
          if (turnOn) {
            foreach (Transform child in crewPanelButtons.transform)
            {
              // Deactivate the child GameObject
              foreach (Transform grandchild in child)
              {
                grandchild.gameObject.SetActive(true);
              }
            }
          } else {
            foreach (Transform child in crewPanelButtons.transform)
            {
              // Deactivate the child GameObject
              foreach (Transform grandchild in child)
              {
                grandchild.gameObject.SetActive(false);
              }
            }
          }
        }
    }
    
    void Update()
    {
        if (handSubsystem == null || !handSubsystem.running)
        {
            SubsystemManager.GetSubsystems(s_Subsystems);
            if (s_Subsystems.Count > 0)
            {
                handSubsystem = s_Subsystems[0];
            }
            else
            {
                return;
            }
        }
        
        XRHand rightHand = handSubsystem.rightHand;
        
        if (rightHand.isTracked)
        {
            bool indexExtended = IsFingerExtended(rightHand, XRHandFingerID.Index);
            bool middleExtended = IsFingerExtended(rightHand, XRHandFingerID.Middle);
            bool ringExtended = IsFingerExtended(rightHand, XRHandFingerID.Ring);
            bool pinkyExtended = IsFingerExtended(rightHand, XRHandFingerID.Little);
            
            bool isOneFinger = indexExtended && !middleExtended && !ringExtended && !pinkyExtended;
            bool isTwoFingers = indexExtended && middleExtended && !ringExtended && !pinkyExtended;
            bool isFist = !indexExtended && !middleExtended && !ringExtended && !pinkyExtended;
            
            // hold up one finger to show the caller menu, two for crewmate info
            if (isOneFinger && !crewMenuActive)
            {
                oneFingerTimer += Time.deltaTime;
                if (oneFingerTimer >= gestureHoldTime && !callMenuActive)
                {
                    Debug.Log("Opening call menu");
                    callMenuActive = true;
                    if (callButtons) callButtons.SetActive(true);
                }
            } else if (isTwoFingers && !callMenuActive) {
                twoFingerTimer += Time.deltaTime;
                if (twoFingerTimer >= gestureHoldTime && !crewMenuActive)
                {
                    Debug.Log("Opening crew menu");
                    crewMenuActive = true;
                    if (crewPanel) crewPanel.SetActive(true);
                    checkCrewButtons(true);
                }
            } else
            {
                oneFingerTimer = 0f;
                twoFingerTimer = 0f;
            }
            
            // hold fist to close the caller menu
            if (isFist)
            {
                fistTimer += Time.deltaTime;
                if (fistTimer >= gestureHoldTime && (callMenuActive || crewMenuActive))
                {
                    Debug.Log("Closing all menus");
                    callMenuActive = false;
                    crewMenuActive = false;
                    if (callButtons) callButtons.SetActive(false);
                    if (crewPanel) crewPanel.SetActive(false);
                    checkCrewButtons(false);
                    fistTimer = 0f;
                }
            }
            else
            {
                fistTimer = 0f;
            }
        }
    }
    
    public void CloseMenu()
    {
        callMenuActive = false;
        crewMenuActive = false;
        if (callButtons) callButtons.SetActive(false);
        if (crewPanel) crewPanel.SetActive(false);
        checkCrewButtons(false);
    }
    
    bool IsFingerExtended(XRHand hand, XRHandFingerID finger)
    {
        var tip = hand.GetJoint(finger.GetFrontJointID());
        var knuckle = hand.GetJoint(finger.GetBackJointID());
        
        if (!tip.TryGetPose(out var tipPose)) return false;
        if (!knuckle.TryGetPose(out var knucklePose)) return false;
        
        float distance = Vector3.Distance(tipPose.position, knucklePose.position);
        return distance > extendedThreshold;
    }
}