using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damage = 5;
    [SerializeField] float speed = 5f;
    [SerializeField] float despawn_time = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, despawn_time);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += -transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") ||
            other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().GetDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
