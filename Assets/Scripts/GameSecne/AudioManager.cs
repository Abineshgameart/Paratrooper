using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip Shooting;
    public AudioClip ShipDestroy;
    public AudioClip JetDestroy;
    public AudioClip ParatrooperDestroy;
    public AudioClip ShooterDestroy;
    public AudioClip BombDestroy;


    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}
