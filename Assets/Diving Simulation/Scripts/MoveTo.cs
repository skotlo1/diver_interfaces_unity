using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using System.Collections;

public class MoveTo : MonoBehaviour 
{

    public CallTowerManager ctm;
    [SerializeField] XROrigin xrOrigin;          // XR Origin to move
    [SerializeField] Transform aimPose;          // optional forward source (e.g., Left/Aim Pose)
    [SerializeField] Transform xrCamera;         // fallback forward source
    [SerializeField] bool useCharacterController = true;
    
    CharacterController cc;
    XRHandSubsystem handSubsystem;

    void Awake()
    {
        if (!xrOrigin) xrOrigin = GetComponent<XROrigin>();
        if (!xrCamera && xrOrigin) xrCamera = xrOrigin.Camera?.transform;

        cc = xrOrigin ? xrOrigin.GetComponent<CharacterController>() : null;

        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader != null) handSubsystem = loader.GetLoadedSubsystem<XRHandSubsystem>();
    }

    // Start is called before the first frame update
    public void MoveToCrewmember(string crewmateName)
    {
      CrewInfo[] crewInformation = ctm.GetCrewmatesInformation();
      int foundIdx = -1;
      
      for (int i = 0; i < crewInformation.Length; i++) {
        if (crewInformation[i].name == crewmateName) {
          foundIdx = i;
          break;
        }
      }

      if (foundIdx == -1) {
        Debug.Log("crewmate not in list");
        return;
      } 

      CrewInfo selectedCrewmate = crewInformation[foundIdx];
      Vector3 crewmatePosition = selectedCrewmate.worldLocation;
      Debug.Log("moving to" + crewmatePosition.ToString());       

      if (xrOrigin == null){ 
        Debug.Log("No XROrigin found");
        return;
      }

      if (useCharacterController && cc != null && cc.enabled){
        cc.enabled = false;
        xrOrigin.transform.position = crewmatePosition;
        cc.enabled = true;
      } else {
        xrOrigin.transform.position = crewmatePosition;
      }
      Debug.Log("xrOrigin is:" + xrOrigin.transform.position);
    }

}
