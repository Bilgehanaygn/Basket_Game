using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, ICollidable
{

    private AudioSource _audioSource;
    // Start is called before the first frame update
    private void Awake() => _audioSource = GetComponent<AudioSource>();

    public void OnCollisionWithBall(){
        this._audioSource.Play();
    }
}
