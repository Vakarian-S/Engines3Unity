using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class ExplodeOnContactController : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 10f;
        [SerializeField] private string[] targetTags;
        [SerializeField] private GameObject explosionObject;

        public System.Action OnDeath;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDealDamage(collision.gameObject);
            Explode();
        }

        private void Explode()
        {
            Instantiate(explosionObject, transform.position, transform.rotation);
            HandleDeath();
        }

        private void HandleDeath()
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }

        private void TryDealDamage(GameObject otherObject)
        {
            if (!IsValidTarget(otherObject))
            {
                return;
            }

            IDamageable damageableTarget = otherObject.GetComponent<IDamageable>();
            damageableTarget?.TakeDamage((int)damageAmount);
        }

        private bool IsValidTarget(GameObject otherObject)
        {
            if (targetTags == null || targetTags.Length == 0)
            {
                return true;
            }

            return targetTags.Any(otherObject.CompareTag);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}