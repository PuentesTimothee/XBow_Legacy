using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip rightStep;
    [SerializeField]
    private AudioClip leftStep;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void StepRight()
    {
        audioSource.PlayOneShot(rightStep);
    }

    private void StepLeft()
    {
        audioSource.PlayOneShot(leftStep);
    }
}
