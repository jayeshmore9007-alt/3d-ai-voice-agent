using UnityEngine;
using System;

public class MasterController : MonoBehaviour
{
    [SerializeField] private AndroidAgentClient aiBrain;
    [SerializeField] private AvatarController avatarController;
    [SerializeField] private VoiceAgentManager voiceManager;

    private AndroidJavaObject accessibilityBridge;

    private void Start()
    {
        InitAccessibilityBridge();
        if (avatarController != null)
        {
            avatarController.SpeechFinished += OnSpeechFinished;
        }
    }

    private void InitAccessibilityBridge()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass serviceClass = new AndroidJavaClass("com.example.aicontrol.MyAccessibilityService"))
            {
                accessibilityBridge = serviceClass.CallStatic<AndroidJavaObject>("getInstance");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[MasterController] Accessibility Bridge Error: {e.Message}");
        }
#endif
    }

    public void OnVoiceInputReceived(string userTranscript)
    {
        if (avatarController != null) avatarController.TriggerGesture("Think");
        if (aiBrain != null) aiBrain.SendTranscript(userTranscript);
    }

    public void OnAIResponseReceived(ActionData actionData, AudioClip speechClip)
    {
        if (avatarController != null && speechClip != null) avatarController.PlaySpeech(speechClip);
        if (actionData != null && actionData.type == "function_call") ExecuteAndroidCommand(actionData);
    }

    private void ExecuteAndroidCommand(ActionData action)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (accessibilityBridge != null)
        {
            string jsonPayload = $"{{\"action\":\"{action.action_name}\", \"target\":\"{action.message}\"}}";
            accessibilityBridge.Call<string>("handleJsonCommand", jsonPayload);
        }
#endif
    }

    private void OnSpeechFinished()
    {
        if (avatarController != null) avatarController.SetIdle();
    }
}
