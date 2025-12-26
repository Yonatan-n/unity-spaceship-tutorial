using UnityEngine;
using UnityEngine.UI;

public class Obstacles : MonoBehaviour
{
    [SerializeField] float minSize = 5f;
    [SerializeField] float maxSize = 10f;
    [SerializeField] float minSpeed = 1.0f;
    [SerializeField] float maxSpeed = 1.0f;
    [SerializeField] float maxSpinSpeed = 100f;

    Rigidbody2D rb;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] float exploTTL = 8f;
    [SerializeField] Color exploColor = new Color32(145, 145, 145, 255);
    [SerializeField] float speedMul = 0.15f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        float randomSize = Random.Range(minSize, maxSize);
        // float randomSpeed = Random.Range(minSpeed, maxSpeed);
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);
        rb.AddTorque(randomTorque);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //  speed up on hit
        if (rb != null)
        {
            // Vector2 currentSpeed = rb.linearVelocity;
            // Vector2 newSpeed = currentSpeed * speedMul;
            rb.linearVelocity *= 1 + speedMul;
        }

        GameObject explosionInstanace = Instantiate(explosionEffect, transform.position, transform.rotation);
        Renderer rendererExplo = explosionInstanace.GetComponent<Renderer>();
        if (rendererExplo != null)
        {
            rendererExplo.material.color = exploColor;
        }
        Destroy(explosionInstanace, exploTTL);
    }
}
