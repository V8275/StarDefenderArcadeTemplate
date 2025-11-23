using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 0.3f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;

    private void Start()
    {
        player.GetComponent<HealthAbility>().OnDamage += ShakeCamera;
    }

    private void ShakeCamera()
    {
        transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            var health = player.GetComponent<HealthAbility>();
            if (health != null)
                health.OnDamage -= ShakeCamera;
        }
    }
}