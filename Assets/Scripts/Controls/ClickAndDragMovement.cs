using UnityEngine;

public class ClickAndDragMovement : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 10;
    [SerializeField] private float scrollSpeed = 2;

    [Space, SerializeField] private float zoomSpeed;
    [SerializeField] private Vector2 zoomMinMax = new(-3, -10);
    [SerializeField] private Vector2 movementClamp;
    private Vector3 dragOrigin;

    private void LateUpdate()
    {
        Move();
        ClampPosition();
        Zoom();
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, -movementClamp.x, movementClamp.x);
        clampedPosition.y = Mathf.Clamp(transform.position.y, -movementClamp.y, movementClamp.y);
        transform.position = clampedPosition;
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newOrthoSize = GetComponent<Camera>().orthographicSize - scroll * zoomSpeed;
        newOrthoSize = Mathf.Clamp(newOrthoSize, zoomMinMax.x, zoomMinMax.y);
        GetComponent<Camera>().orthographicSize = newOrthoSize;
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        { 
            var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            var step = dragSpeed * Time.deltaTime;
            var move = new Vector3(pos.x * step, pos.y * step, 0);

            transform.Translate(-move, Space.World);
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            var move = new Vector3(x, y, 0) * scrollSpeed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 2.0f : 1.0f);
            transform.Translate(move, Space.World);
        }
        
    }
}