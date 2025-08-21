using UnityEngine;

public class NodeLineDrawer : MonoBehaviour
{
    [SerializeField] private int segments = 20;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for(int i = 0; i < transform.childCount - 1; i++)
        {
            /// WORKING CODE WITHOUT DRAWING ARCS
            //if (transform.GetChild(i) != null && transform.GetChild(i + 1) != null)
            //    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);

            if (transform.GetChild(i) != null && transform.GetChild(i + 1) != null)
            {
                Transform a = transform.GetChild(i);
                Transform b = transform.GetChild(i + 1);

                if (a.GetComponent<Node>().nodeEventList[0].eventType == Util.EventType.jump)
                {
                    Vector3 coffset = new(0f, a.GetComponent<Node>().nodeEventList[0].param_f, 0f);
                    Vector3 c = (a.position + b.position) / 2 + coffset;
                    Vector3 olda = a.position;

                    for(int j = 0; j <= segments; j++)
                    {
                        float t = j / (float)segments;
                        Vector3 p = Util.QuadraticBezierPoint(t, a.position, c, b.position);

                        Gizmos.DrawLine(olda, p);
                        olda = p;
                    }
                }
                else
                    Gizmos.DrawLine(a.position, b.position);
            }
        }
    }
}
