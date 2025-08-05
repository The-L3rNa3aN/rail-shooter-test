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
        string suffix;
        Transform nodeContainer;

        switch (count / 10)                                                                             //Assigning a suffix after "Node" in the xxx format, ranging between 0 and 999.
        {
            case < 1:
                suffix = "00" + count;
                break;

            case < 10:
                suffix = "0" + count;
                break;

            default:
                return;
        }

        if (GameObject.Find("NodeContainer"))                                                           //Finding the NodeContainer gameobject or creating one if it doesn't exist.
            nodeContainer = GameObject.Find("NodeContainer").transform;
        else
        {
            GameObject nc = new() { name = "NodeContainer" };
            nc.transform.position = Vector3.zero;
            nodeContainer = nc.transform;
        }

        instance.name = "Node" + suffix;
        if(nodeContainer.childCount != 0)                                                               //Assigning the position of the new node based on the previous node's position or at Vector3.zero.
        {
            Transform lastChild = nodeContainer.GetChild(nodeContainer.childCount - 1);
            instance.transform.position = new Vector3(lastChild.position.x + 2.5f, 0f, lastChild.position.z + 2.5f);
        }
        else
            instance.transform.position = Vector3.zero;

        ///TO DO
        ///Have the new node positioned a unit away from the previous one based on the direction between the previous one and the one before it.

        instance.transform.SetParent(nodeContainer);                                                    //Assigning the node's parent to NodeContainer and adding it in the GameManager's "nodes" list.

        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.nodes.Add(instance.transform);

        ///TO DO
        ///Removing any nodes from the scene should accordingly rearrange the GameManager's "nodes" list by removing the empty element and renaming the nodes after that correctly.

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Selection.activeObject = instance;
    }
}