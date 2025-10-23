using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player prefab_player;
    public List<Transform> nodes;
    public Transform currentNode;
    [Tooltip("Change it from '0' if you want the player to start at a specific node.")] public int currentNodeNumber;
    public float qDistNodes;
    public UIHandler uih;
    public static GameManager Main { get; private set; }
    [HideInInspector] public Player player;
    [HideInInspector] public bool justStarted = true;

    private void Awake()
    {
        if (Main != null && Main != this)
            Destroy(gameObject);
        else
            Main = this;
    }

    private void Start()
    {
        //NEW
        currentNode = nodes[currentNodeNumber];

        player = Instantiate(prefab_player, currentNode.position, Quaternion.identity);

        Camera.main.transform.parent = player.cameraContainer;
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.rotation = Camera.main.transform.parent.rotation;

        //NEW
        List<NodeListItem> l = currentNode.GetComponent<Node>().nodeEventList;
        if (l[0].eventType == Util.EventType.stop)
        {
            player.pVelocity = 0f;
            player.willStop = true;
        }
        justStarted = false;
        //SetNodeAndPlayerPosition();

        //Pause menu.
        Inputs.key_esc += () => GamePause(Time.timeScale != 0f);

        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void NextNode()
    {
        currentNodeNumber++;
        if(currentNodeNumber != nodes.Count)        //When there is no node after the last one.
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

    public void SetNodeAndPlayerPosition()
    {
        currentNode = nodes[currentNodeNumber];

        player.transform.position = currentNode.position;
        List<NodeListItem> l = currentNode.GetComponent<Node>().nodeEventList;
        if (l[0].eventType == Util.EventType.stop)
        {
            player.pVelocity = 0f;
            player.willStop = true;
        }
        justStarted = false;
    }

    public void RestartLevel()
    {
        //currentNodeNumber = 1;
        //currentNode = nodes[currentNodeNumber];

        //player.transform.position = nodes[1].position;
        //Debug.Log($"player position: {player.transform.position}, node 0 position: {nodes[0].position}");
        //List<NodeListItem> l = currentNode.GetComponent<Node>().nodeEventList;
        //if (l[0].eventType == Util.EventType.stop)
        //{
        //    player.pVelocity = 0f;
        //    player.willStop = true;
        //}
        //justStarted = false;

        //Fuck all of it. This is the easiest one.
        Time.timeScale = 1f;
        currentNodeNumber = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //uih.sceneLoader.LoadSceneWithDelay(SceneManager.GetActiveScene().buildIndex);
    }

    public void GamePause(bool p)
    {
        Time.timeScale = p ? 0f : 1f;
        uih.pauseScreenParent.SetActive(p);
    }

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Time.timeScale = 1f;
    //}

    private void OnDestroy()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
        Inputs.key_esc -= () => GamePause(Time.timeScale != 0f);
    }

    // EDITOR
    public void ReorderNodesList()
    {
        nodes.RemoveAll(n => n == null);
        for(int i = 0; i < nodes.Count; i++) nodes[i].name = "Node" + Util.ReturnNodeSuffix(i);
    }
}
