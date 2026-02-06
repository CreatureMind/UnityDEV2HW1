using UnityEngine;
using System.Collections;

public class TomatoCollision : MonoBehaviour
{
    [Header("Effects")]
    [Tooltip("The Particle System for the tomato splash")]
    [SerializeField] private ParticleSystem splashParticles;

    [Header("Components")]
    [SerializeField] private MeshRenderer tomatoMesh;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider tomatoCollider;

    private bool hasCollided = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        hasCollided = true;
        StartCoroutine(SplashRoutine());
    }

    private IEnumerator SplashRoutine()
    {
        rb.isKinematic = true;
        if (tomatoMesh != null) tomatoMesh.enabled = false;
        if (tomatoCollider != null) tomatoCollider.enabled = false;
        if (splashParticles != null)
        {
            splashParticles.Play();
            yield return new WaitForSeconds(splashParticles.main.duration);
        }
        else
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}