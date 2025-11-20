using UnityEngine;
using TMPro;

public class PhoneKeypad : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI displayText;
    
    [Header("Dial System")]
    [SerializeField] private SimpleDial simpleDial;

    private string currentNumber = "";

    void Start()
    {
        if (displayText) displayText.text = "";
        Debug.Log("PhoneKeypad script is running!");
    }

    public void PressDigit(string digit)
    {
        currentNumber += digit;
        UpdateDisplay();
    }

    public void DeleteLastDigit()
    {
        if (currentNumber.Length > 0)
        {
            currentNumber = currentNumber.Substring(0, currentNumber.Length - 1);
            UpdateDisplay();
        }
    }

    public void ClearNumber()
    {
        currentNumber = "";
        UpdateDisplay();
    }

    public void CallNumber()
    {
        
        if (string.IsNullOrEmpty(currentNumber))
        {
            Debug.Log("No number entered!");
            return;
        }

        Debug.Log("Calling: " + currentNumber);
        
        string crewName = "";
        
        if (currentNumber == "495")
        {
            crewName = "Pedro";
        }
        else if (currentNumber == "894")
        {
            crewName = "Suzie";
        }
        else if (currentNumber == "347")
        {
            crewName = "Romain";
        }
        else if (currentNumber == "1001")
        {
            simpleDial.QuickDial(1001);
        }
        
        if (!string.IsNullOrEmpty(crewName))
        {
            if (simpleDial != null)
            {
                simpleDial.DialCrewmateByName(crewName);
            }
            else
            {
                Debug.LogError("SimpleDial not assigned!");
            }
        }
        else
        {
            Debug.Log("Unknown number: " + currentNumber);
        }
    }

    private void UpdateDisplay()
    {
        if (displayText)
            displayText.text = currentNumber;
    }
}