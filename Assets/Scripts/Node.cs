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
        if(GameManager.Main.currentNode == transform)
        {
            Player player = other.GetComponent<Player>();
            if (player) StartCoroutine(player.ParseNodeActions(nodeEventList));
            if (!GameManager.Main.justStarted && !player.willStop && !player.willJump) GameManager.Main.NextNode();

            if(GameManager.Main.currentNode == GameManager.Main.nodes[^1])
                StartCoroutine(GameManager.Main.uih.FadeInEndScreen(0.25f));
        }
    }
}
