using UnityEngine;

public class ShootBall : MonoBehaviour
{
    Vector3 touchPosWorld;

    Vector2 firstTouch;

    RaycastHit2D hit;
    bool clickedToBall = false;
    LineRenderer lineRenderer;

    GameObject[] balls;

    Camera cam;


    Vector2 symmetricPoint;
    
    // Start is called before the first frame update

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        balls = GameObject.FindGameObjectsWithTag("Ball");
        cam = Camera.main;
    }
    void Start()
    {
        
    }

    private void RenderLine(Vector2 firstPoint,Vector2 secondPoint){
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firstPoint);
        lineRenderer.SetPosition(1, secondPoint);
    }

    void Update()
    {
        //if clicked on the ball and dragged
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            firstTouch = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(firstTouch);
            hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit.collider && hit.collider.CompareTag("Ball")){

                clickedToBall = true;
            }
            else{
                clickedToBall = false;
            }
        }

        //if the finger is lifted remove the lint
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
            lineRenderer.positionCount = 0;
            ShootTheBall(hit.transform.position, Input.GetTouch(0).position, hit.rigidbody);
        }

        //render a line according to the position of the finger
        if (clickedToBall && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
            symmetricPoint = findSymmetric(cam.ScreenToWorldPoint(Input.GetTouch(0).position), hit.transform.position);
            RenderLine(hit.transform.position, symmetricPoint);
        }
        
    }

    //calculates the symmetric of a point referencing to a point..
    private Vector2 findSymmetric(Vector2 thePoint, Vector2 referencePoint){
        float newX = referencePoint.x + (referencePoint.x - thePoint.x);
        float newY = referencePoint.y + (referencePoint.y - thePoint.y);
        return new Vector2(newX, newY);
    }

    private void ShootTheBall(Vector2 ballCenter, Vector2 fingerPoint, Rigidbody2D ballBody){
        //calculate force depending on the length of the line
        float force = Mathf.Sqrt(Vector2.Distance(ballCenter, fingerPoint))/5;
        ballBody.AddForce((symmetricPoint-ballCenter)*force, ForceMode2D.Impulse);
    }

    // private Vector2 ReferenceToBallDirection(Vector2 ballCenter){

    // }


}
