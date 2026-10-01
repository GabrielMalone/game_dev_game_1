using UnityEngine;

public class LineColliderScript : MonoBehaviour

{

    public GameObject sparkPrefab;

    private EdgeCollider2D edgeCollider;

    void Start()

    {

        edgeCollider = GetComponent<EdgeCollider2D>();

    }

    void OnTriggerEnter2D(Collider2D other)

    {

        if (other.CompareTag("Player"))

        {

            Vector2 sparkPosition =

                edgeCollider.ClosestPoint(other.transform.position);

            GameObject spark = Instantiate(
                sparkPrefab,
                sparkPosition,
                Quaternion.identity
            );

            Destroy(spark, 1f);

        }

    }

}