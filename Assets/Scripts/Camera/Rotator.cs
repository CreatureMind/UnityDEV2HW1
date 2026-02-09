using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    void LateUpdate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
