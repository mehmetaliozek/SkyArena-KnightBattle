using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Camera cam;

    private Plane[] planes;
    private float moveSpeed = 37.5f;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(moveSpeed, 0.0f));
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
    }

    private void Update()
    {
        if (!GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds) && transform.position.x > 0)
        {
            Destroy(gameObject);
        }
    }
}