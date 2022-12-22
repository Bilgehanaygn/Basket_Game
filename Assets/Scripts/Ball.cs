
using UnityEngine;

public class Ball : MonoBehaviour {
    
    public static Ball Instance{get; private set;}
    private bool _isGhost;

    private void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
            Debug.Log("destroyed");
        } 
        else 
        { 
            Instance = this; 
        }
    }

    private void Start(){
        // _ballBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //dependency inversion here
        if(_isGhost) return;
        ICollidable otherCollidable = other.gameObject.GetComponent<ICollidable>();
        if(otherCollidable!=null){
            otherCollidable.OnCollisionWithBall();
        }
        
    }

        public void ShootTheBall(Vector2 ballCenter, Vector2 symmetricPoint, Rigidbody2D ballBody, bool isGhost){
            _isGhost = isGhost;
            //calculate force depending on the length of the line
            float force = (symmetricPoint - ballCenter).magnitude*8;
            Vector2 direction = (symmetricPoint-ballCenter).normalized;
            ballBody.AddForce(direction*force, ForceMode2D.Impulse);
    }
}