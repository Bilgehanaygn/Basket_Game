using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayChangerObstacle : MonoBehaviour, ICollidable
{
    private BoxCollider2D boxCollider;

    [SerializeField]
    private Vector2 _newVelocity;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void SetNewVelocity(Vector2 newVelocity){
        this._newVelocity = newVelocity;
    }


    private void OnCollisionEnter2D(Collision2D other) {
        //collided object must be ball, change the way of the ball by updating velocity of the rigidbody
        other.rigidbody.velocity = _newVelocity;
    }

    
}
