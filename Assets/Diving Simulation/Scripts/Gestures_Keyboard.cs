using UnityEngine;
using UnityEngine.InputSystem;

public class HandGestureKeyboardTest : MonoBehaviour
{
    //Access to dialing buttons
    [SerializeField] private GameObject callButtons;
    
    //Access to crew member buttons
    [SerializeField] private GameObject crewPanel;
    [SerializeField] private GameObject crewPanelButtons;
    
    void Start()
    {
        //Buttons originally do not appear
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
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        
        // key 1 = 1 finger up
        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            ShowCallMenu();
        }
        
        // key 2 = 2 fingers up
        if (keyboard.digit2Key.wasPressedThisFrame)
        {
            ShowCrewInfo();
        }
        
        // key 0 means hide the buttons
        if (keyboard.digit0Key.wasPressedThisFrame)
        {
            HideMenu();
        }
    }
    
    void ShowCallMenu()
    {
        if (callButtons) callButtons.SetActive(true);
        if (crewPanel) crewPanel.SetActive(false);
        checkCrewButtons(false);
    }
    
    void ShowCrewInfo()
    {
        if (callButtons) callButtons.SetActive(false);
        if (crewPanel) crewPanel.SetActive(true);
        checkCrewButtons(true);
    }
    
    void HideMenu()
    {
        if (callButtons) callButtons.SetActive(false);
        if (crewPanel) crewPanel.SetActive(false);
        checkCrewButtons(false);
    }
}