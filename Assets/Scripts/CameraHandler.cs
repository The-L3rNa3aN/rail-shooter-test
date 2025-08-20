using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public float amplitude = 6.25f;
    public float frequency = 0.004f;

    private void Update()
    {
        float ampX = amplitude / 2;
        float freX = frequency / 2;

        float _x = transform.parent.eulerAngles.x + ampX + Mathf.Sin(freX);
        float _y = transform.parent.eulerAngles.y + amplitude + Mathf.Sin(frequency);

        transform.Rotate(_x, _y, transform.parent.eulerAngles.z);
    }
}
