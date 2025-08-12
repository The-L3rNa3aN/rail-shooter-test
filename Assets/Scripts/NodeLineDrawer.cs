using UnityEngine;

public class NodeLineDrawer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for(int i = 0; i < transform.childCount - 1; i++)
        {
            if (transform.GetChild(i) != null && transform.GetChild(i + 1) != null)
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }
}
