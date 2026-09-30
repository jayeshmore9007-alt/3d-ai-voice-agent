using UnityEngine;

public class VoiceAgentManager : MonoBehaviour
{
    [SerializeField] private MasterController masterController;
    private AudioClip recordedClip;
    private bool isRecording = false;

    public void StartListening()
    {
        if (Microphone.devices.Length > 0)
        {
            recordedClip = Microphone.Start(null, false, 10, 44100);
            isRecording = true;
        }
    }

    public void StopListening()
    {
        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            if (masterController != null)
            {
                masterController.OnVoiceInputReceived("User Audio Recorded");
            }
        }
    }
}
