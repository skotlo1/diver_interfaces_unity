using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleDial : MonoBehaviour
{

    public CallTowerManager ctm;
    private AudioSource aS;
    private bool isBusy = false;
    private bool isIncomingCall = false;

    public AudioClip ringtone;
    private AudioClip incomingCall;
    private int incomingCallerFrequency = 0;

    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void QuickDial(int freq)
    {
        //Must hangup before making another call
        if (!isBusy)
        {
            isBusy = true;
            aS.clip = ctm.CallCrewmate(freq);
            aS.Play();
        }
    }

    public bool IncomingCall(AudioClip speaker, int freq)
    {
        Debug.Log(" incoming:" + freq);
        if (!isBusy && !isIncomingCall)
        {
            incomingCall = speaker;
            incomingCallerFrequency = freq;
            isIncomingCall = true;
            isBusy = true;

            aS.clip = ringtone;
            aS.loop = true;
            aS.Play();
        }

        return !isBusy;

    }

    public void AnswerCall()
    {
        if (isIncomingCall)
        {
            aS.Stop();
            aS.loop = false;
            aS.clip = incomingCall;
            aS.Play();
            isIncomingCall = false;
        }
    }

    public void HangUp()
    {
        if (isBusy)
        {
            aS.Stop();
            isBusy = false;
            isIncomingCall = false;
        }
    }

    public void DialCrewmateByName(string crewmateName)
{
    CrewInfo[] crewInfo = ctm.GetCrewmatesInformation();
    
    foreach (CrewInfo crew in crewInfo)
    {
        if (crew.name == crewmateName)
        {
            QuickDial(crew.frequency);
            Debug.Log($"Dialing {crewmateName} at frequency {crew.frequency}");
            return;
        }
    }
    
    Debug.LogWarning($"Could not find crew member: {crewmateName}");
}
}