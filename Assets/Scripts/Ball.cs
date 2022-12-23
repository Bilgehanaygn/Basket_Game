
using UnityEngine;

public class Ball : MonoBehaviour {
    
    public static Ball Instance{get; private set;}
    private bool _isGhost;
    private AudioSource _audioSource;

    public static float ballForce = 15f;

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
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //dependency inversion here
        ICollidable otherCollidable = other.gameObject.GetComponent<ICollidable>();
        
        if(otherCollidable!=null){
            //play bounce voice
            this._audioSource.Play();
            return;
        }



    }

    private void OnTriggerEnter2D(Collider2D other) {
        IKiller otherKiller = other.gameObject.GetComponent<IKiller>();
        if(otherKiller!=null){
            FindObjectOfType<GameManager>().GetComponent<GameManager>().GameOver();
            //game over
            Debug.Log("Game Over");
        }
    }

        public void ShootTheBall(Vector2 ballCenter, Vector2 symmetricPoint, Rigidbody2D ballBody, bool isGhost){
            _isGhost = isGhost;
            //calculate force depending on the length of the line
            float force = (symmetricPoint - ballCenter).magnitude*ballForce;
            Vector2 direction = (symmetricPoint-ballCenter).normalized;
            ballBody.AddForce(direction*force, ForceMode2D.Impulse);
    }
}