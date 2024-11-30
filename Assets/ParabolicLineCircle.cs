using UnityEngine;

using UnityEngine;

public class ParabolicLineCircle : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisual; // Reference to XRInteractorLineVisual
    public GameObject circleObject;           // Circle 3D object to instantiate
    private LineRenderer lineRenderer;        // LineRenderer component

    private GameObject ballInstance;          // Ball instance to hold the created object

    void Start()
    {
        // Check if lineVisual and circleObject are assigned
        if (lineVisual != null && circleObject != null)
        {
            circleObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            // Retrieve the LineRenderer component from the XRInteractorLineVisual
            lineRenderer = lineVisual.GetComponent<LineRenderer>();

            if (lineRenderer == null)
            {
                Log("LineRenderer component not found on the XRInteractorLineVisual.");
            }
        }
        else
        {
            Log("lineVisual or circleObject is not assigned.");
        }
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            int pointCount = lineRenderer.positionCount;  // Get the number of positions in the line

            // Get the positions of the line from the LineRenderer
            Vector3[] linePoints = new Vector3[pointCount];
            lineRenderer.GetPositions(linePoints);

            // Find the middle point of the line (halfway through the array of points)
            Vector3 middlePoint = linePoints[pointCount / 2];

            // Instantiate or move the circle object to the middle point of the parabolic line
            if (ballInstance == null)
            {
                ballInstance = Instantiate(circleObject, middlePoint, Quaternion.identity);
            }
            else
            {
                ballInstance.transform.position = middlePoint; // Update position if the ball already exists
            }
        }
    }

    private void Log(string message) { Debug.Log($"拋物線: {message}"); }
    private void LogWarning(string message) { Debug.LogWarning($"拋物線: {message}"); }
}
