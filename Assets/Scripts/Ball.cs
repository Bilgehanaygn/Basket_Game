
using UnityEngine;

public class Ball : MonoBehaviour {
    
    public static Ball Instance{get; private set;}

    public static event System.Action OnAnyCollision;

    public static float ballForce = 15f;
    
    private GameManager _gameManager;



    private void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
            Debug.Log("destroyed");
        } 
        else 
        {
            _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
            Instance = this; 
        }
    }

    // private void Start(){
        // _ballBody = GetComponent<Rigidbody2D>();
       
    // }

    private void OnCollisionEnter2D(Collision2D other) {
        //dependency inversion here
        #region observer
            Debug.Log("On collision enter is called");
            OnAnyCollision?.Invoke();
        #endregion

    }

    private void OnTriggerEnter2D(Collider2D other) {
        IKiller otherKiller = other.gameObject.GetComponent<IKiller>();
        if(otherKiller!=null){
            _gameManager.GameOver();
            //game over
            Debug.Log("Game Over");
            return;
        }
    }

    public void ShootTheBall(Vector2 ballCenter, Vector2 _symmetricPoint, Rigidbody2D ballBody, bool isGhost){
        //calculate force depending on the length of the line
        Debug.Log("called");
        float force = (_symmetricPoint - ballCenter).magnitude*ballForce;
        Vector2 direction = (_symmetricPoint-ballCenter).normalized;
        ballBody.AddForce(direction*force, ForceMode2D.Impulse);
    }


}