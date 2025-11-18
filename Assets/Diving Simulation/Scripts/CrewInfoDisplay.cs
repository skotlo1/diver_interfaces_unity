using UnityEngine;
using UnityEngine.UI;

public class CrewInfoDisplay : MonoBehaviour
{
    public CallTowerManager ctm;
    public Text infoText;
    public string crewmateName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      infoText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
      CrewInfo[] crewInformation = ctm.GetCrewmatesInformation();
      int foundIdx = -1;
      
      for (int i = 0; i < crewInformation.Length; i++) {
        if (crewInformation[i].name == crewmateName) {
          foundIdx = i;
          break;
        }
      }

      if (foundIdx != -1) {
        CrewInfo selectedCrewmate = crewInformation[foundIdx];
        string currPosition = selectedCrewmate.worldLocation.ToString();
        string frequency = selectedCrewmate.frequency.ToString();

        infoText.text = $"Position: {currPosition}\nTo call, dial: {frequency}";
      } 

    }
}
