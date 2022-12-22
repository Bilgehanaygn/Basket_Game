using UnityEngine;

public class CollideAudioPlayer : MonoBehaviour, ICollidable {
    
    private AudioSource _audioSource;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    public void OnCollisionWithBall() {
        _audioSource.Play();
    }

    
}