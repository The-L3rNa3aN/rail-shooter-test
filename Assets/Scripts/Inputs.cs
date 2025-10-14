using System;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static Action key_esc;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            key_esc?.Invoke();
    }
}
