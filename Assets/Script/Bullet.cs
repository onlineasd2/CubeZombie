using UnityEngine;

public class Bullet : MonoBehaviour {
    
    void OnTriggerEnter (Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Enemy")
        {
            other.GetComponent<Enemy>().HP -= 1;
            Debug.Log("Hit");
            Destroy(gameObject);
        }
    }
}
