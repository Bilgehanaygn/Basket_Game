using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private bool clickedToBall = false;

    private Vector2 _firstTouch;
    private RaycastHit2D _hit;

    Camera _cam;
    private Vector2 _symmetricPoint;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
         _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        fingerDrag();
    }



    //Note: if the finger distance is too little, cancel the trajectory and do not shoot
    private void fingerDrag(){
        //if clicked on the ball and dragged
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            _firstTouch = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(_firstTouch);
            _hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(_hit.collider && _hit.collider.GetComponent<Ball>() != null){

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
            Ball.Instance.ShootTheBall(_hit.transform.position, _symmetricPoint, _hit.rigidbody, false);

        }

        //render a line according to the position of the finger
        if (clickedToBall && Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)){
            _symmetricPoint = findSymmetric(_cam.ScreenToWorldPoint(Input.GetTouch(0).position), Ball.Instance.transform.position);
            renderTrajectory(Ball.Instance.transform.position, 50);
        }
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
        float force = (_symmetricPoint - ballCenter).magnitude*Ball.ballForce;
        Vector2 direction = (_symmetricPoint-ballCenter).normalized;
        Vector2 velocity = direction * force;
        _lineRenderer.positionCount = maxIterations;
        for(int i=0;i<maxIterations;i++){
            float newX = ballCenter.x + (velocity.x*i*Time.fixedDeltaTime);
            float newY = ballCenter.y + (velocity.y*i*Time.fixedDeltaTime) - ((9.8f)*Mathf.Pow(i*Time.fixedDeltaTime,2))/2;
            _lineRenderer.SetPosition(i, new Vector3(newX, newY, 0));
        }
        
    }





}
