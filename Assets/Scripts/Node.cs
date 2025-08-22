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
        if (player) StartCoroutine(player.ParseNodeActions(nodeEventList));

        if (!player.willStop && !player.willJump) GameManager.Main.NextNode();
    }
}
