using UnityEngine;
using System.Collections;

public class BoidController : MonoBehaviour
{
    public float minVelocity = 5;
    public float maxVelocity = 20;
    public float randomness = 1;
    public static int flockSize = 10;
    public GameObject prefab;
    public GameObject chasee;

    public Vector3 flockCenter;
    public Vector3 flockVelocity;

    private GameObject[] boids;

    void Start()
    {
        boids = new GameObject[flockSize];
        for (var i = 0; i < flockSize; i++)
        {
            Vector3 position = new Vector3(
                Random.value * GetComponent<Collider>().bounds.size.x,
                Random.value * GetComponent<Collider>().bounds.size.y,
                Random.value * GetComponent<Collider>().bounds.size.z
            ) - GetComponent<Collider>().bounds.extents;

            GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            boid.transform.parent = transform;
            boid.transform.localPosition = position;
            boid.GetComponent<BoidFlocking>().SetController(gameObject);
            boids[i] = boid;
            //print(boids[i]);
        }
    }


    void Update()
    {
        Vector3 theCenter = Vector3.zero;
        Vector3 theVelocity = Vector3.zero;
        if(boids.Length != flockSize)
        {
            Destroy(boids);
            Start();
        }
        foreach (GameObject boid in boids)
        {
            theCenter = theCenter + boid.transform.localPosition;
            theVelocity = theVelocity + boid.GetComponent<Rigidbody>().velocity;
        }

        flockCenter = theCenter / (flockSize);
        flockCenter = this.transform.localPosition;
        flockVelocity = theVelocity / (flockSize);
        flockVelocity = MoveKnight.rigid.velocity;
    }

    private void Destroy(GameObject[] boids)
    {
        var size = boids.Length;
        for (int i = 0; i < size; i++)
        {
            Destroy(boids[i]);
        }
    }
}