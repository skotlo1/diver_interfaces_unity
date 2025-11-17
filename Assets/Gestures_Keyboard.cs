using UnityEngine;
using UnityEngine.InputSystem;

public class HandGestureKeyboardTest : MonoBehaviour
{
    //Access to dialing buttons
    [SerializeField] private GameObject callButtons;
    //Access to crew member buttons
    [SerializeField] private GameObject crewButtons;
    
    void Start()
    {
        //Buttons originally do not appear
        if (callButtons) callButtons.SetActive(false);
        if (crewButtons) crewButtons.SetActive(false);
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
        if (crewButtons) crewButtons.SetActive(false);
    }
    
    void ShowCrewInfo()
    {
        if (callButtons) callButtons.SetActive(false);
        if (crewButtons) crewButtons.SetActive(true);
    }
    
    void HideMenu()
    {
        if (callButtons) callButtons.SetActive(false);
        if (crewButtons) crewButtons.SetActive(false);
    }
}