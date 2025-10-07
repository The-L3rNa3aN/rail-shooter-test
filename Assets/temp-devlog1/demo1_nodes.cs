using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class demo1_nodes : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform[] children;
    public int arcSegments = 20;

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
        //for(int i = 0; i < children.Length; i++)
        //{
        //    lineRenderer.SetPosition(i, children[i].position);
        //}
        int totalPositions = 0;

        for (int i = 0; i < children.Length; i++)
        {
            bool hasJump = children[i].GetComponent<Node>().nodeEventList[0].eventType == Util.EventType.jump;
            totalPositions += hasJump ? arcSegments : 2;
        }

        lineRenderer.positionCount = totalPositions;
        int positionIndex = 0;

        for(int i = 0; i < children.Length - 1; i++)
        {
            Vector3 startPos = children[i].position;
            Vector3 endPos = children[i + 1].position;
            bool hasJump = children[i].GetComponent<Node>().nodeEventList[0].eventType == Util.EventType.jump;

            if(hasJump)
            {
                for(int j = 0; j < arcSegments; j++)
                {
                    float t = j / (float)(arcSegments - 1);
                    float h = children[i].GetComponent<Node>().nodeEventList[0].param_f;
                    Vector3 arcPosition = CalculateArcPosition(startPos, endPos, t, h/2);
                    lineRenderer.SetPosition(positionIndex, arcPosition);
                    positionIndex++;
                }
            }
            else
            {
                lineRenderer.SetPosition(positionIndex, startPos);
                positionIndex++;
                lineRenderer.SetPosition(positionIndex, endPos);
                positionIndex++;
            }
        }
    }

    private Vector3 CalculateArcPosition(Vector3 start, Vector3 end, float t, float height)
    {
        Vector3 position = Vector3.Lerp(start, end, t);
        float parabola = 4 * height * t * (1 - t);
        position.y += parabola;
        return position;
    }
}
