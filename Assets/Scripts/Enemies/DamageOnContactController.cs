using System.Linq;
using UnityEngine;

namespace Enemies
{
    public class DamageOnContactController : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 10f;
        [SerializeField] private string[] targetTags;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDealDamage(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TryDealDamage(other.gameObject);
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