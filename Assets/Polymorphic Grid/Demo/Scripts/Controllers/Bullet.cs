using UnityEngine;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class Bullet : MonoBehaviour
    {
        public int damage = 1;

        private void OnCollisionEnter(Collision collision)
        {
            HealthHandler health = collision.gameObject.GetComponent<HealthHandler>();
            if (health != null)
                health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}