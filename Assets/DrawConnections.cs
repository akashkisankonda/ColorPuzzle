using UnityEngine;

public class DrawConnections : MonoBehaviour
{
    public Transform[] parentObjects;

    void Start()
    {
        DrawAllConnections();
    }

    void DrawAllConnections()
    {
        foreach (Transform parent in parentObjects)
        {
            DrawLinesToChildren(parent);

            // Connect parent to the next parent object if exists
            int parentIndex = System.Array.IndexOf(parentObjects, parent);
            if (parentIndex < parentObjects.Length - 1)
            {
                Transform nextParent = parentObjects[parentIndex + 1];
                DrawLine(parent, nextParent);
            }
        }
    }

    void DrawLinesToChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            DrawLine(parent, child);

            // Recursively connect sub-children
            if (child.childCount > 0)
            {
                DrawLinesToChildren(child);
            }
        }
    }

    void DrawLine(Transform start, Transform end)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.position = start.position;
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Configure the LineRenderer
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);

        // Optional: Set the line color or material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;
    }

    // Draw lines in the Scene view for visualization
    void OnDrawGizmos()
    {
        if (parentObjects == null || parentObjects.Length == 0)
            return;

        Gizmos.color = Color.red;
        foreach (Transform parent in parentObjects)
        {
            DrawGizmoLinesToChildren(parent);

            // Connect parent to the next parent object if exists
            int parentIndex = System.Array.IndexOf(parentObjects, parent);
            if (parentIndex < parentObjects.Length - 1)
            {
                Transform nextParent = parentObjects[parentIndex + 1];
                Gizmos.DrawLine(parent.position, nextParent.position);
            }
        }
    }

    void DrawGizmoLinesToChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Gizmos.DrawLine(parent.position, child.position);

            // Recursively connect sub-children
            if (child.childCount > 0)
            {
                DrawGizmoLinesToChildren(child);
            }
        }
    }
}
