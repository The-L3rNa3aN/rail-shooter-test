using System.Collections.Generic;
using UnityEngine;

public class demo4GM : MonoBehaviour
{
    public demoplayer prefab_player;
    public List<Transform> nodes;
    public Transform currentNode;
    [Tooltip("Change it from '0' if you want the player to start at a specific node.")] public int currentNodeNumber;
    public float qDistNodes;
    public static demo4GM Main { get; private set; }
    [HideInInspector] public demoplayer player;
    [HideInInspector] public bool justStarted = true;

    private void Awake()
    {
        if (Main != null && Main != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        player = Instantiate(prefab_player);

        currentNode = nodes[currentNodeNumber];

        //TEMPORARY. For the player starting at Node#000 on the start of the scene.
        player.transform.position = currentNode.position;
        List<NodeListItem> l = currentNode.GetComponent<Node>().nodeEventList;
        if (l[0].eventType == Util.EventType.stop)
        {
            player.pVelocity = 0f;
            player.willStop = true;
        }
        justStarted = false;
        //player.SetPlayerTarget(currentNode);

        //qDistNodes = Vector3.Distance(player.transform.position, currentNode.position) / 4;
    }

    public void NextNode()
    {
        currentNodeNumber++;
        if (currentNodeNumber != nodes.Count)        //When there is no node after the last one.
        {
            currentNode = nodes[currentNodeNumber];
            player.SetPlayerTarget(currentNode);

            qDistNodes = Vector3.Distance(nodes[currentNodeNumber - 1] ? nodes[currentNodeNumber - 1].position : player.transform.position, currentNode.position) / 4;

            switch (currentNode.GetComponent<Node>().nodeEventList[0].eventType)
            {
                case Util.EventType.stop:
                    player.willStop = true;
                    break;

                case Util.EventType.jump:
                    player.willJump = true;
                    break;
            }
        }
    }

    // EDITOR
    public void ReorderNodesList()
    {
        nodes.RemoveAll(n => n == null);
        for (int i = 0; i < nodes.Count; i++) nodes[i].name = "Node" + Util.ReturnNodeSuffix(i);
    }
}
