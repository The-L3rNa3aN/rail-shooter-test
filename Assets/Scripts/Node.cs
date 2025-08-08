using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public enum EventType { test_1, test_2 }

[System.Serializable]
public class TestListItem
{
    public EventType eventType;
    public Vector3 param_v;
    public float param_f;
    //public float duration;
}

public class Node : MonoBehaviour
{
    //public TestListItem testListItem;
    public List<TestListItem> testListItem;
}
