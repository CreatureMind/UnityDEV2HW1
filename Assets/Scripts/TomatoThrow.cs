using UnityEngine;

public class TomatoThrow : MonoBehaviour
{
[Header("Settings")]
    [Tooltip("The MeshCollider of the dancer")]
    [SerializeField] private MeshCollider dancerCollider;

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

        Vector3 targetPoint = GetRandomPointOnMesh();
        Vector3 spawnPos = mainCam.transform.position + (mainCam.transform.forward * 1f); 
        GameObject newTomato = Instantiate(tomatoPrefab, spawnPos, Quaternion.identity);
        Vector3 direction = (targetPoint - spawnPos).normalized;
        Rigidbody rb = newTomato.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
        }
    }

    private Vector3 GetRandomPointOnMesh()
    {
        Mesh mesh = dancerCollider.sharedMesh;
        
        int randomVertexIndex = Random.Range(0, mesh.vertices.Length);
        
        Vector3 localPoint = mesh.vertices[randomVertexIndex];

        Vector3 worldPoint = dancerCollider.transform.TransformPoint(localPoint);

        return worldPoint;
    }
}
