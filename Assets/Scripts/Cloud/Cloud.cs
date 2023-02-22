using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Camera cam;
    private Plane[] planes;
    private float moveSpeed = 37.5f;

    private void Start()
    {
        // Buluta verdiğim hızda ilermesi için kuvvet uyguluyoru
        GetComponent<Rigidbody2D>().AddForce(new Vector2(moveSpeed, 0.0f));
        // Sahnedeki aktif kamerayı alıyorum
        cam = Camera.main;
        // Cameranın görüş açısında olan noktalrı alıyorum
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
    }

    private void Update()
    {
        // Blutun collideri kameranın görüş açısının dışındaysa ve bulutun x pozisyonu 0 dan büyükse bulutu siliyoz 
        if (!GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds) && transform.position.x > 0)
        {
            Destroy(gameObject);
        }
    }
}