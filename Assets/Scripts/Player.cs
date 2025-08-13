using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public Transform target;
    public float pVelocity = 5f;
    private CharacterController controller;
    private Vector3 targetVector;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        controller.Move(Time.deltaTime * pVelocity * targetVector);
    }

    public void SetPlayerTarget(Transform _target) => targetVector = (_target.position - transform.position).normalized;

       public void NodeAction(NodeListItem action)
       {
           switch(action.eventType)
           {
               case Util.EventType.look:
                   StartCoroutine(Action_Look(action.param_v, action.duration));
                   break;

               case Util.EventType.walk:
                   break;
               
               case Util.EventType.stop:
               case Util.EventType.hold:
                   break;

               case Util.EventType.open:
                   break;
           }
       }

       public IEnumerator Action_Look(Vector3 v, float f)
       {
           float elapsedTime = 0f;

           while (elapsedTime < f)
           {
               float t = elapsedTime / f;
               //cameraContainer.transform.rotation = 
               // Lerp the local Euler angle to 'v'.
               elapsedTime += Time.deltaTime;
               yield return null;
            }

           //transform.rotation = transform.localEulerAngles
       }
}
