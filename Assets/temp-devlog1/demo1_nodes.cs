using UnityEngine;

[ExecuteInEditMode]
public class demo1_nodes : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform[] children;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        children = GetComponentsInChildren<Transform>();
        children = System.Array.FindAll(children, t => t != transform);

        lineRenderer.positionCount = children.Length;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        for(int i = 0; i < children.Length; i++)
        {
            lineRenderer.SetPosition(i, children[i].position);
        }
    }
}
