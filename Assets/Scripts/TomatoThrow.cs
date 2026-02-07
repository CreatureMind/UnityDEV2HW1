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

    [ContextMenu("Throw Tomato At Dancer")]
    public void ThrowTomatoAtDancer()
    {
        if (dancerCollider == null || tomatoPrefab == null)
        {
            Debug.LogError("Where is Dancer Collider and Tomato Prefab biatch! add them in the inspector");
            return;
        }

        var targetPoint = GetRandomPointInBox(dancerCollider);
        var spawnPos = mainCam.transform.position + (mainCam.transform.forward * 1f); 
        var newTomato = Instantiate(tomatoPrefab, spawnPos, Quaternion.identity);
        var direction = (targetPoint - spawnPos).normalized;
        var rb = newTomato.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }
    }

    private Vector3 GetRandomPointInBox(BoxCollider box)
    {
        var center = box.center;
        var extents = box.size * 0.5f;

        Vector3 randomLocalPoint = new Vector3(
            Random.Range(center.x - extents.x, center.x + extents.x),
            Random.Range(center.y - extents.y, center.y + extents.y),
            Random.Range(center.z - extents.z, center.z + extents.z)
        );

        return box.transform.TransformPoint(randomLocalPoint);
    }
}
