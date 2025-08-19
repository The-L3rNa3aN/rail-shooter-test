using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public List<Transform> nodes;
    public Transform currentNode;
    public int currentNodeNumber;
    public float distanceFromTarget;
    public static GameManager Main { get; private set; }

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
        Camera.main.transform.parent = player.cameraContainer;
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.rotation = Camera.main.transform.parent.rotation;

        currentNode = nodes[0];
        currentNodeNumber = 0;
        player.SetPlayerTarget(currentNode);
    }

    public void NextNode()
    {
        currentNodeNumber++;
        if(currentNodeNumber != nodes.Count)        //When there is no node after the last one.
        {
            currentNode = nodes[currentNodeNumber];
            player.SetPlayerTarget(currentNode);

            if (currentNode.GetComponent<Node>().nodeEventList[0].eventType == Util.EventType.stop) player.willStop = true;
        }
    }

    // EDITOR
    public void ReorderNodesList()
    {
        nodes.RemoveAll(n => n == null);
        for(int i = 0; i < nodes.Count; i++) nodes[i].name = "Node" + Util.ReturnNodeSuffix(i);
    }
}
