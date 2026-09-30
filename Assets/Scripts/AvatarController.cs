using UnityEngine;
using System;

public class AvatarController : MonoBehaviour
{
    public event Action SpeechFinished;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator;

    public void TriggerGesture(string gestureName)
    {
        if (animator != null)
        {
            animator.SetTrigger(gestureName);
        }
    }

    public void PlaySpeech(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Invoke(nameof(OnSpeechComplete), clip.length);
        }
    }

    public void SetIdle()
    {
        if (animator != null)
        {
            animator.Play("Idle");
        }
    }

    private void OnSpeechComplete()
    {
        SpeechFinished?.Invoke();
    }
}
