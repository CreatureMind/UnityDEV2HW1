using UnityEngine;

public class TomatoThrow : MonoBehaviour
{
[Header("Settings")]
    [Tooltip("The MeshCollider of the dancer")]
    [SerializeField] private BoxCollider dancerCollider;

    [Tooltip("The tomato prefab (Must have Rigidbody)")]
    [SerializeField] private GameObject tomatoPrefab;

    [Tooltip("How fast the tomato flies")]
    [SerializeField] private float throwForce = 15f;

    [Tooltip("Time in seconds before tomato is destroyed")]
    [SerializeField] private float destroyTime = 5f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    public void ThrowTomatoAtDancer()
    {
        if (dancerCollider == null || tomatoPrefab == null)
        {
            Debug.LogError("Where is Dancer Collider and Tomato Prefab biatch! add them in the inspector");
            return;
        }

        Vector3 targetPoint = GetRandomPointInBox(dancerCollider);
        Vector3 spawnPos = mainCam.transform.position + (mainCam.transform.forward * 1f); 
        GameObject newTomato = Instantiate(tomatoPrefab, spawnPos, Quaternion.identity);
        Vector3 direction = (targetPoint - spawnPos).normalized;
        Rigidbody rb = newTomato.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }
    }

    private Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.center;
        Vector3 extents = box.size * 0.5f;

        Vector3 randomLocalPoint = new Vector3(
            Random.Range(center.x - extents.x, center.x + extents.x),
            Random.Range(center.y - extents.y, center.y + extents.y),
            Random.Range(center.z - extents.z, center.z + extents.z)
        );

        return box.transform.TransformPoint(randomLocalPoint);
    }
}
