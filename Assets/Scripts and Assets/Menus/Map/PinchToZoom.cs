using UnityEngine;

public class PinchToZoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoom = 0.5f;
    public float maxZoom = 3.0f;

    private Vector2 initialTouchPosition1;
    private Vector2 initialTouchPosition2;
    private float initialDistance;

    void Update()
    {
        // Check if there are two touches on the screen
        if (Input.touchCount == 2)
        {
            // Get the positions of the two touches
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Check if the touches just started
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialTouchPosition1 = touch1.position;
                initialTouchPosition2 = touch2.position;
                initialDistance = Vector2.Distance(initialTouchPosition1, initialTouchPosition2);
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                // Calculate the current distance between the touches
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);

                // Calculate the zoom amount based on the change in distance
                float zoomAmount = (currentDistance - initialDistance) * zoomSpeed * Time.deltaTime;

                // Apply the zoom to the GameObject's scale (zoom in or out)
                transform.localScale = new Vector3(
                    Mathf.Clamp(transform.localScale.x + zoomAmount, minZoom, maxZoom),
                    Mathf.Clamp(transform.localScale.y + zoomAmount, minZoom, maxZoom),
                    1f);

                // Update initial values for the next frame
                initialTouchPosition1 = touch1.position;
                initialTouchPosition2 = touch2.position;
                initialDistance = currentDistance;
            }
        }
    }
}