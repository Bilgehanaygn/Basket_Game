using UnityEngine;

public class HitAudioPlayer : MonoBehaviour
{

    private AudioSource _audioSource;
    private void Awake() => _audioSource = GetComponent<AudioSource>();

    private void OnEnable() => Ball.OnAnyCollision += PlayHitAudio;
    private void OnDisable() => Ball.OnAnyCollision -= PlayHitAudio;
    
    private void PlayHitAudio() => _audioSource.Play();
}
