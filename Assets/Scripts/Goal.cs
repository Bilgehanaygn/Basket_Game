using UnityEngine;

public class Goal : MonoBehaviour
{
    private BoxCollider2D goalCollider;

    private void Start(){
        goalCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("its a goal");
    }
}
