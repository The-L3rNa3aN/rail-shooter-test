using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TO DO: find a way to add nodes to 'nodes' when creating new ones in the editor mode and not having to use the nodeContainer at all.
/// </summary>
public class GameManager : MonoBehaviour
{
    public Player player;
    public List<Transform> nodes;
    public Transform currentNode;
    public int currentNodeNumber;
    public float distanceFromTarget;
    public static GameManager Main { get; private set; }
    [SerializeField] private Transform nodeContainer;

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

        for (int i = 0; i < nodeContainer.childCount; i++)
        {
            nodes.Add(nodeContainer.GetChild(i));
        }
        currentNode = nodes[0];
        currentNodeNumber = 0;
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
}