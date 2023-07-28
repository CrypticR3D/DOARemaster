using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public LineRenderer lineRendererPrefab;

    public Camera mainCamera; // Assign the main camera reference in the Unity Editor

    private GameObject currentNode;
    private LineRenderer currentLine;

    UIManager uIManager;

    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
    }
    void Update()
    {
        if (!uIManager.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Cast a ray from the mouse position
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Node"))
                    {
                        // Player clicked on a node
                        currentNode = hit.collider.gameObject;
                        if (lineRendererPrefab != null)
                        {
                            currentLine = Instantiate(lineRendererPrefab, hit.collider.transform.position, Quaternion.identity);
                            currentLine.SetPosition(0, currentNode.transform.position);
                        }
                        else
                        {
                            Debug.LogError("Line Renderer Prefab is not assigned!");
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (currentLine != null)
                {
                    // Cast a ray from the mouse position
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.CompareTag("Node") && hit.collider.gameObject != currentNode)
                        {
                            // Player released the mouse button on another node
                            GameObject targetNode = hit.collider.gameObject;
                            Vector3 targetPosition = targetNode.transform.position;
                            targetPosition.x = currentNode.transform.position.x; // Keep the X coordinate the same
                            currentLine.SetPosition(1, targetPosition);
                            currentLine = null;
                        }
                    }
                    else
                    {
                        // Player released the mouse button in an empty space, destroy the line renderer
                        Destroy(currentLine.gameObject);
                        currentLine = null;
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (currentLine != null)
                {
                    // Update line renderer position while dragging, keeping the X coordinate the same
                    Vector3 linePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(mainCamera.transform.position.x - currentNode.transform.position.x)));
                    linePosition.x = currentNode.transform.position.x; // Keep the X coordinate the same
                    currentLine.SetPosition(1, linePosition);
                }
            }
        }
    }

    public void SpawnNode(Vector3 position)
    {
        Instantiate(nodePrefab, position, Quaternion.identity);
    }
}
