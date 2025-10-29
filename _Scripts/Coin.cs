using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float bounceHeight = 0.25f;
    public float bounceSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the coin
        startPosition = transform.position;
    }

    void Update()
    {
        // --- Rotation ---
        // Rotate the coin around its up axis (Y-axis)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);


        // --- Bouncing ---
        // Calculate the new Y position using a sine wave for a smooth bounce
        float newY = startPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

        // Apply the new position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
