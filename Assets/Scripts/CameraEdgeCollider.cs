using UnityEngine;

public class CameraEdgeCollider : MonoBehaviour
{
    private Camera cam;
    private EdgeCollider2D camBox;
    private float sizeX, sizeY, ratio;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        camBox = GetComponent<EdgeCollider2D>();

        Vector2 bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        Vector2 topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector2 bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

        Vector2[] edgePoints = {bottomLeft,topLeft,topRight,bottomRight, bottomLeft};
        camBox.points = edgePoints;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
