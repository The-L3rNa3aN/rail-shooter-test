using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NodeListItem
{
    public Util.EventType eventType;
    public Vector3 param_v;
    public float param_f;
    public bool param_b;
    public float duration;
}

public class Node : MonoBehaviour
{
    public List<NodeListItem> nodeEventList;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(player)
        {
            foreach(NodeListItem item in nodeEventList)
                player.NodeAction(item);
        }

        GameManager.Main.NextNode();
    }
}
