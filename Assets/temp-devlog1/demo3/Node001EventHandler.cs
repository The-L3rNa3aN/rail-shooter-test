using UnityEngine;

public class Node001EventHandler : MonoBehaviour
{
    public Animator targetAnimator;
    public string nextAnimationTrigger = "demo3_player";

    public void TriggerAnimator()
    {
        targetAnimator.enabled = true;
        targetAnimator.Play(nextAnimationTrigger);
    }
}
