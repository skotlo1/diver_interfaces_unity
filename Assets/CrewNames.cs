using UnityEngine;
using UnityEngine.UI;

public class CrewName : MonoBehaviour
{
    public CallTowerManager ctm;
    private Text textBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBox = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CrewInfo[] crewInformation = ctm.GetCrewmatesInformation();
        string crewText = "";

        foreach(CrewInfo currCrewmate in crewInformation)
        {
            string currName = currCrewmate.name;
            string currPosition = currCrewmate.worldLocation.ToString();
            crewText = crewText + currName + "" + currPosition + "";

        }

        Debug.Log(crewText);
        textBox.text = crewText;

        
    }
}
