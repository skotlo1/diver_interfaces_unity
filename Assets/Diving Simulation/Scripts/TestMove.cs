using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TestMove : MonoBehaviour
{
    [SerializeField] Transform xrOrigin;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotateSpeed = 90f;
    [SerializeField] float strokeDistance = 2f;
    [SerializeField] Transform xrCamera;         // fallback forward source
    private float currentCameraPitch = 0f;
    [SerializeField] float maxLookAngle = 80f;

    // Update is called once per frame
    void Update()
    {
      HandleKeyboardMovement();
    }

    void HandleKeyboardMovement() {
      if (xrOrigin == null) return;
      
      Keyboard kb = Keyboard.current;
      if (kb == null) return;
        
      Vector3 movement = Vector3.zero;
      
      if (kb.wKey.isPressed) movement += xrOrigin.forward;
      if (kb.sKey.isPressed) movement -= xrOrigin.forward;
      if (kb.aKey.isPressed) movement -= xrOrigin.right;
      if (kb.dKey.isPressed) movement += xrOrigin.right;
      
      if (movement.sqrMagnitude > 0)
      {
          movement.y = 0;
          movement.Normalize();
          xrOrigin.position += movement * moveSpeed * Time.deltaTime;
      }

      if (kb.qKey.isPressed){
        xrOrigin.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
      }
      if (kb.eKey.isPressed){
        xrOrigin.Rotate(0, rotateSpeed * Time.deltaTime, 0);
      }

      if (kb.spaceKey.wasPressedThisFrame){
        DoStrokeImpulse();
      }

      if (kb.rKey.isPressed) { //up
        currentCameraPitch -= rotateSpeed * Time.deltaTime;
        currentCameraPitch = Mathf.Clamp(currentCameraPitch, -maxLookAngle, maxLookAngle);
        xrCamera.localRotation = Quaternion.Euler(currentCameraPitch, 0, 0);
      }
      if (kb.fKey.isPressed) { //down
        currentCameraPitch += rotateSpeed * Time.deltaTime;
        currentCameraPitch = Mathf.Clamp(currentCameraPitch, -maxLookAngle, maxLookAngle);
        xrCamera.localRotation = Quaternion.Euler(currentCameraPitch, 0, 0);
      }

      if (kb.upArrowKey.isPressed){
        xrOrigin.position += xrOrigin.forward * moveSpeed * Time.deltaTime;
      }
      if (kb.downArrowKey.isPressed){
        xrOrigin.position -= xrOrigin.forward * moveSpeed * Time.deltaTime;
      }
      if (kb.leftArrowKey.isPressed){
        xrOrigin.position -= xrOrigin.right * moveSpeed * Time.deltaTime;
      }
      if (kb.rightArrowKey.isPressed){
        xrOrigin.position += xrOrigin.right * moveSpeed * Time.deltaTime;
      }

    }

    void DoStrokeImpulse(){
      Vector3 fwd = xrOrigin.forward;
      fwd.y = 0;
      fwd.Normalize();
      xrOrigin.position += fwd * strokeDistance;
    }
}
