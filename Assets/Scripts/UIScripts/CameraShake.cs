using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake s_instance;

    [SerializeField] private Camera m_camera;
    [SerializeField] private float m_shakeAmount = 0.1f;
    [SerializeField] private float m_decreaseFactor = 1.0f;
    private int camShaking;
    private Vector3 m_cameraStartPos;

    private void Awake()
    {
        //Set the instance to this if it's null
        if (s_instance == null)
            s_instance = this;

        m_cameraStartPos = m_camera.transform.localPosition;
    }

    public static void ShakeCam(float camShaking)
    {
        s_instance.StartCoroutine(s_instance.Shake(camShaking));
    }

    private IEnumerator Shake(float camShaking)
    {
        float timer = camShaking;
        
        //continues shaking camera until the timer has reached 0
        while (timer > 0)
        {
            //choose a random position within a given radius (ignoring the z axis)
            Vector3 randomOffset = Random.insideUnitSphere * m_shakeAmount;
            randomOffset.z = 0;

            //offset the camera's position by the random offset
            m_camera.transform.localPosition = s_instance.m_cameraStartPos + randomOffset;

            //decrease timer every iteration
            timer -= Time.deltaTime * m_decreaseFactor;
            yield return null;
        }

        m_camera.transform.localPosition = m_cameraStartPos;
    }
}
