using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayChangerObstacle : MonoBehaviour, ICollidable
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    private void OnCollisionEnter2D(Collision2D other) {
        //collided object must be ball, change the way of the ball by updating velocity of the rigidbody
        other.rigidbody.velocity = new Vector2(0, -5f);
    }

    
}
