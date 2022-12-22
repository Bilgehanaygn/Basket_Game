using UnityEngine;

public class CameraEdgeCollider : MonoBehaviour
{
    private Camera _cam;
    private EdgeCollider2D _camBox;
    private float sizeX, sizeY, ratio;
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<Camera>();
        _camBox = GetComponent<EdgeCollider2D>();

        Vector2 bottomLeft = (Vector2)_cam.ScreenToWorldPoint(new Vector3(0, 0, _cam.nearClipPlane));
        Vector2 topLeft = (Vector2)_cam.ScreenToWorldPoint(new Vector3(0, _cam.pixelHeight, _cam.nearClipPlane));
        Vector2 topRight = (Vector2)_cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, _cam.pixelHeight, _cam.nearClipPlane));
        Vector2 bottomRight = (Vector2)_cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, 0, _cam.nearClipPlane));

        Vector2[] edgePoints = {bottomLeft,topLeft,topRight,bottomRight, bottomLeft};
        _camBox.points = edgePoints;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
