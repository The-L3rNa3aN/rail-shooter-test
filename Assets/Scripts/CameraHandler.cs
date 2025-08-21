using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public bool isSine = true;

    [Header("Sine Wave Sway")]
    public float x_amplitude = 0.2f;
    public float y_amplitude = 0.1f;

    public float x_frequency = 2f;
    public float y_frequency = 1f;

    [Header("Perlin Noise Sway")]
    public float speed = 1f;
    public float intensity = 0.1f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) isSine = !isSine;

        if(isSine)
            SineWaveSway();
        else
            PerlinNoiseSway();
    }

    private void SineWaveSway()
    {
        float _x = x_amplitude * Mathf.Sin(x_frequency * Time.time);
        float _y = y_amplitude * Mathf.Sin(y_frequency * Time.time);

        transform.localEulerAngles = new Vector3(_x, _y, transform.localEulerAngles.z);
    }

    private void PerlinNoiseSway()
    {
        float _x = Mathf.PerlinNoise(Time.time * speed, 0f) * 2f - 1f;
        float _y = Mathf.PerlinNoise(0f, Time.time * speed) * 2f - 1f;

        transform.localEulerAngles = new Vector3(_x, _y, transform.localEulerAngles.z);
    }
}
