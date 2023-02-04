using UnityEngine;

public class ClickAndDragMovement : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;


    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        var step = dragSpeed * Time.deltaTime;
        var move = new Vector3(pos.x * step, pos.y * step, 0);

        transform.Translate(-move, Space.World);
    }
}
