using UnityEngine;


public class ShootBall : MonoBehaviour
{
    Vector2 _firstTouch;
    RaycastHit2D _hit;
    bool clickedToBall = false;
    Camera _cam;
    AudioSource _audioSource;
    Ball _ball;
    private Vector2 symmetricPoint;
    
    private LineRenderer _lineRenderer;
    
    // Start is called before the first frame update
    public static ShootBall Instance {get; private set;}

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _cam = Camera.main;
        _ball = FindObjectOfType<Ball>();
        _lineRenderer = GetComponent<LineRenderer>();
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
    void Start()
    {
        
    }

    void Update()
    {
        fingerDrag();
        
    }

    //calculates the symmetric of a point referencing to a point..
    private Vector2 findSymmetric(Vector2 thePoint, Vector2 referencePoint){
        float newX = referencePoint.x + (referencePoint.x - thePoint.x);
        float newY = referencePoint.y + (referencePoint.y - thePoint.y);
        return new Vector2(newX, newY);
    }

    private void renderTrajectory(Vector2 ballCenter, int maxIterations){
        //in vertical V0t - 1/2gt^2
        //in horizontal V0t
        //horizontal constituent -> cos(degree from the x axis)*V0
        //vertical constituent -> sin(degree from the x axis)*V0
        float force = (symmetricPoint - ballCenter).magnitude*8;
        Vector2 direction = (symmetricPoint-ballCenter).normalized;
        Vector2 velocity = direction * force;
        _lineRenderer.positionCount = maxIterations;
        for(int i=0;i<maxIterations;i++){
            float newX = ballCenter.x + (velocity.x*i*Time.fixedDeltaTime);
            float newY = ballCenter.y + (velocity.y*i*Time.fixedDeltaTime) - ((9.8f)*Mathf.Pow(i*Time.fixedDeltaTime,2))/2;
            _lineRenderer.SetPosition(i, new Vector3(newX, newY, 0));
        }
        
    }

    private void fingerDrag(){
        //if clicked on the ball and dragged
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            _firstTouch = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(_firstTouch);
            _hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(_hit.collider && _hit.collider.CompareTag("Ball")){

                clickedToBall = true;
            }
            else{
                clickedToBall = false;
            }
        }

        //if the finger is lifted remove the trajectory
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
            if(!clickedToBall){
                return;
            }
            _lineRenderer.positionCount = 0;
            _ball.ShootTheBall(_hit.transform.position, symmetricPoint, _hit.rigidbody, false);

            _audioSource.Play();
        }

        //render a line according to the position of the finger
        if (clickedToBall && Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)){
            symmetricPoint = findSymmetric(_cam.ScreenToWorldPoint(Input.GetTouch(0).position), _hit.transform.position);
            renderTrajectory(_ball.transform.position, 50);
        }
    }

}
