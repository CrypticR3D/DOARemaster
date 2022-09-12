using UnityEngine;

public class RotateHorizontally : MonoBehaviour
{
    // This will make it move 90 degrees per second
    [SerializeField] public float speed = 1;
    float yRotation = 0;

    private void Update()
    {
        // Deltatime is the amount of time it takes for a frame to pass
        // This means it is an accurate way of counting upwards.
        yRotation += Time.deltaTime * speed;

        // This will set the rotation to a new rotation based on an increasing Y axis.
        // Which will make it spin horizontally
        this.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}