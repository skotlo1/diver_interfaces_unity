using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using System.Collections;
using UnityEngine.UI;

public class MoveTo : MonoBehaviour 
{

    public CallTowerManager ctm;
    public Text buttonText;
    public string crewmateName;
    private bool isDirecting = false;

    public RectTransform arrowPositioning;
    public Text locationDisplay;

    [SerializeField] XROrigin xrOrigin;          // XR Origin to move
    [SerializeField] Transform aimPose;          // optional forward source (e.g., Left/Aim Pose)
    [SerializeField] Transform xrCamera;         // fallback forward source
    [SerializeField] Transform mainCamera; 
    [SerializeField] bool useCharacterController = true;

    public float arrowDistance = 2f;

    public float arrowHeight = -0.2f;

    
    CharacterController cc;
    XRHandSubsystem handSubsystem;

    Vector3? crewmatePosition = null;

    void Awake()
    {
        if (!xrOrigin) xrOrigin = GetComponent<XROrigin>();
        if (!xrCamera && xrOrigin) xrCamera = xrOrigin.Camera?.transform;

        cc = xrOrigin ? xrOrigin.GetComponent<CharacterController>() : null;

        buttonText.text = $"Navigate";
        isDirecting = false;
        arrowPositioning.gameObject.SetActive(false);
        locationDisplay.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public void OnGoToClick()
    {
      isDirecting = !isDirecting;

      if (isDirecting) {
        buttonText.text = $"Stop Directing To {crewmateName}";
        crewmatePosition = FindCrewmatePosition();
        arrowPositioning.gameObject.SetActive(true);
        locationDisplay.gameObject.SetActive(true);
      } else {
        buttonText.text = $"Navigate";
        arrowPositioning.gameObject.SetActive(false);
        locationDisplay.gameObject.SetActive(false);
      }

    }

    void Update() {

      if (isDirecting && xrCamera != null && crewmatePosition != null) {
        UpdateArrowDirection();
      }
    }

    void UpdateArrowDirection() {

      // get orientation direction (which way is the user rotated compared to crewmate?)
      if (crewmatePosition == null) return;

      Debug.Log("updating arrow direction to point towards" + crewmatePosition.Value.ToString());

      Vector3 directionToPoint = crewmatePosition.Value - mainCamera.position;
      Vector3 localDirection = mainCamera.InverseTransformDirection(directionToPoint);

      Quaternion targetRotation = Quaternion.LookRotation(localDirection.normalized, Vector3.up);
      Quaternion offset = Quaternion.Euler(0, -90f, 0);
      arrowPositioning.localRotation = targetRotation * offset;

      UpdateDistanceText(localDirection);

    }

    private Vector3? FindCrewmatePosition() {
      // find crewmember's location
      CrewInfo[] crewInformation = ctm.GetCrewmatesInformation();
      int foundIdx = -1;
      
      for (int i = 0; i < crewInformation.Length; i++) {
        if (crewInformation[i].name == crewmateName) {
          foundIdx = i;
          break;
        }
      }

      if (foundIdx == -1) {
        return null;
      } 

      CrewInfo selectedCrewmate = crewInformation[foundIdx];
      Vector3 crewmatePosition = selectedCrewmate.worldLocation;

      Debug.Log("crewmate position is" + crewmatePosition);

      return crewmatePosition;
    }

    void UpdateDistanceText(Vector3 localDirection){
      Debug.Log("updating text");
      string leftRight = localDirection.x > 0 ? 
        $"{localDirection.x:F1} units to your RIGHT" : 
        $"{Mathf.Abs(localDirection.x):F1} units to your LEFT";
    
      string upDown = localDirection.y > 0 ? 
        $"{localDirection.y:F1} units ABOVE you" : 
        $"{Mathf.Abs(localDirection.y):F1} units BELOW you";
    
      string frontBack = localDirection.z > 0 ? 
        $"{localDirection.z:F1} units IN FRONT of you" : 
        $"{Mathf.Abs(localDirection.z):F1} units BEHIND you";

      locationDisplay.text = $"{crewmateName} is:\n{leftRight}\n{upDown}\n{frontBack}";
    }

} 
