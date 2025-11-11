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
    public void MoveToCrewmember()
    {
      CrewInfo[] crewInformation = ctm.GetCrewmatesInformation();
      CrewInfo currCrewmate = crewInformation[0];
      string currName = currCrewmate.name;
      Vector3 crewmatePosition = currCrewmate.worldLocation;
      Debug.Log("moving to" + crewmatePosition.ToString());

      if (xrOrigin == null){ 
        Debug.Log("xrOrigin is null");
        return;
      }

      if (useCharacterController && cc != null && cc.enabled){
        cc.enabled = false;
        xrOrigin.transform.position = crewmatePosition;
        cc.enabled = true;
      } else {
        xrOrigin.transform.position = crewmatePosition;
      }
    }

}
