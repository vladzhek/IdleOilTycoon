using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform bottomLeftPoint;
    [SerializeField] private Transform bottomRightPoint;
    [SerializeField] private Transform topRightPoint;
    [SerializeField] private Transform topLeftPoint;
    [SerializeField] private float _cameraSpeed = 0.25f;
    [SerializeField] private float _dragSpeed = 0.15f;

    private float _zoomSpeed = 0.05f;
    private float _maxZoom = 5f;
    private float _minZoom = 10f;

    private Camera _camera;
    private Vector3 _lastMousePosition;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR

        if (EventSystem.current.IsPointerOverGameObject())
            return;
#else
                if (Input.touches.Length > 0 && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return;
#endif

        float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        if (zoomDelta != 0)
        {
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - zoomDelta, _maxZoom, _minZoom);
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
                float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
                float touchDeltaMag = (touch1.position - touch2.position).magnitude;
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + deltaMagnitudeDiff * _zoomSpeed,
                    _maxZoom, _minZoom);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            _camera.transform.position -= new Vector3(delta.x, delta.y, 0) * (_dragSpeed * Time.deltaTime);
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X") * _cameraSpeed * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * _cameraSpeed * Time.deltaTime;
            _camera.transform.Translate(new Vector3(x, y, 0));
        }

        SetCameraBounds();
    }
    
    private void SetCameraBounds()
    {
        float slopeBot = (bottomRightPoint.position.y - bottomLeftPoint.position.y) / (bottomRightPoint.position.x - bottomLeftPoint.position.x);
        float yInterceptBot = bottomLeftPoint.position.y - slopeBot * bottomLeftPoint.position.x;
        float cameraYBot = slopeBot * _camera.transform.position.x + yInterceptBot;
        
        float slopeTop = (topRightPoint.position.y - topLeftPoint.position.y) / (topRightPoint.position.x - topLeftPoint.position.x);
        float yInterceptTop = topLeftPoint.position.y - slopeTop * topLeftPoint.position.x;
        float cameraYTop = slopeTop * _camera.transform.position.x + yInterceptTop;
        
        float slopeLeft = (topLeftPoint.position.y - bottomLeftPoint.position.y) / (topLeftPoint.position.x - bottomLeftPoint.position.x);
        float yInterceptLeft = bottomLeftPoint.position.y - slopeLeft * bottomLeftPoint.position.x;
        float cameraYLeft = slopeLeft * _camera.transform.position.x + yInterceptLeft;
    
        float slopeRight = (topRightPoint.position.y - bottomRightPoint.position.y) / (topRightPoint.position.x - bottomRightPoint.position.x);
        float yInterceptRight = bottomRightPoint.position.y - slopeRight * bottomRightPoint.position.x;
        float cameraYRight = slopeRight * _camera.transform.position.x + yInterceptRight;
        
        if (_camera.transform.position.y < cameraYBot)
        {
            Vector3 clampedCameraPos = _camera.transform.position;
            clampedCameraPos.y = cameraYBot;
            _camera.transform.position = clampedCameraPos;
        }
        if (_camera.transform.position.y > cameraYTop)
        {
            Vector3 clampedCameraPos = _camera.transform.position;
            clampedCameraPos.y = cameraYTop;
            _camera.transform.position = clampedCameraPos;
        }
        if (_camera.transform.position.y > cameraYRight)
        {
            Vector3 clampedCameraPos = _camera.transform.position;
            clampedCameraPos.y = cameraYRight;
            _camera.transform.position = clampedCameraPos;
        }
        if (_camera.transform.position.y < cameraYLeft)
        {
            Vector3 clampedCameraPos = _camera.transform.position;
            clampedCameraPos.y = cameraYLeft;
            _camera.transform.position = clampedCameraPos;
        }

        var leftX = topLeftPoint.transform.position.x < bottomLeftPoint.transform.position.x ?
            topLeftPoint.transform.position.x : bottomLeftPoint.transform.position.x;
        var rightX = topRightPoint.transform.position.x > bottomRightPoint.transform.position.x ?
            topRightPoint.transform.position.x : bottomRightPoint.transform.position.x;

        if (_camera.transform.position.x < leftX)
        {
            _camera.transform.position = topLeftPoint.transform.position;
        }
        if (_camera.transform.position.x > rightX)
        {
            _camera.transform.position = bottomRightPoint.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (bottomLeftPoint != null && bottomRightPoint != null && topRightPoint != null && topLeftPoint != null)
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(bottomLeftPoint.position, bottomRightPoint.position);
            Gizmos.DrawLine(bottomRightPoint.position, topRightPoint.position);
            Gizmos.DrawLine(topRightPoint.position, topLeftPoint.position);
            Gizmos.DrawLine(topLeftPoint.position, bottomLeftPoint.position);
            
            Gizmos.DrawSphere(bottomLeftPoint.position, 0.5f);
            Gizmos.DrawSphere(bottomRightPoint.position, 0.5f);
            Gizmos.DrawSphere(topRightPoint.position, 0.5f);
            Gizmos.DrawSphere(topLeftPoint.position, 0.5f);
        }
    }
    
}