using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance; // Singleton instance
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise; // Reference to the noise profile

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Shake(float intensity)
    {
        virtualCameraNoise.m_AmplitudeGain = intensity;
        StartCoroutine(StopShake(1f, intensity)); 
    }

    IEnumerator StopShake(float duration, float initialIntensity)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            // Linearly decrease the intensity
            virtualCameraNoise.m_AmplitudeGain = Mathf.Lerp(initialIntensity, 0f, progress);

            yield return null;
        }

        virtualCameraNoise.m_AmplitudeGain = 0f;
    }
}
