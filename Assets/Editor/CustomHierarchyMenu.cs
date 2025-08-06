using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CustomHierarchyMenu
{
    private const string nodePath = "Assets/Prefabs/Node.prefab";

    [MenuItem("GameObject/rail-shooter-test/Create player node", false, 0)]
    private static void CreateNode()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(nodePath);                        //Finding the prefab in Assets and instantiating it.
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        int count = GameObject.FindGameObjectsWithTag("Node").Length - 1;
        Transform nodeContainer;

        if (GameObject.Find("NodeContainer"))                                                           //Finding the NodeContainer gameobject or creating one if it doesn't exist.
            nodeContainer = GameObject.Find("NodeContainer").transform;
        else
        {
            GameObject nc = new() { name = "NodeContainer" };
            nc.transform.position = Vector3.zero;
            nodeContainer = nc.transform;
        }

        instance.name = "Node" + Util.ReturnNodeSuffix(count);
        if(nodeContainer.childCount != 0)                                                               //Assigning the position of the new node based on the previous node's position or at Vector3.zero.
        {
            Transform lastChild = nodeContainer.GetChild(nodeContainer.childCount - 1);
            Vector3 dir = (instance.transform.position - lastChild.transform.position).normalized;
            instance.transform.position = new Vector3(lastChild.position.x + dir.x + 1f, 0f, lastChild.position.z + dir.z + 1f);
        }
        else
            instance.transform.position = Vector3.zero;

        instance.transform.SetParent(nodeContainer);                                                    //Assigning the node's parent to NodeContainer and adding it in the GameManager's "nodes" list.

        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.nodes.Add(instance.transform);

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Selection.activeObject = instance;
    }
}
