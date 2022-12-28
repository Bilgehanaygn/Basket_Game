using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    private BoxCollider2D _goalCollider;
    private ParticleSystem _goalConfetti;
    private GameManager _gameManager;

    private void Start(){
        _goalCollider = GetComponent<BoxCollider2D>();
        _goalConfetti = GetComponent<ParticleSystem>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        _goalCollider.enabled = false;
        _goalConfetti.Play();
        StartCoroutine(onGoal());
        
    }

    private IEnumerator onGoal(){
        Debug.Log("on goal is called");
        yield return new WaitForSecondsRealtime(_goalConfetti.main.startLifetime.constantMax);
        _gameManager.LevelComplete();
        _goalCollider.enabled = true;
    }
}
