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

        currentNode = nodes[0];
        currentNodeNumber = 0;
        player.SetPlayerTarget(currentNode);
    }

    private void Update()
    {
        distanceFromTarget = Vector3.Distance(currentNode.position, player.transform.position);
        if(distanceFromTarget <= 0.1f)
        {
            currentNodeNumber++;
            currentNode = nodes[currentNodeNumber];
            player.SetPlayerTarget(currentNode);
        }
    }

    // EDITOR
    public void ReorderNodesList()
    {
        nodes.RemoveAll(n => n == null);
        for(int i = 0; i < nodes.Count; i++) nodes[i].name = "Node" + Util.ReturnNodeSuffix(i);
    }
}
