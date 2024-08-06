using UnityEngine;
using System;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class HealthHandler : MonoBehaviour
    {
        public int maxHealth = 100;

        public event Action<int> OnHealthUpdatedCallback;
        public event Action OnDieCallback;

        public bool Dead { get; private set; }

        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (Dead)
                    return;

                OnHealthUpdatedCallback?.Invoke(value);
                currentHealth = Mathf.Clamp(value, 0, maxHealth);

                if (value <= 0)
                {
                    Dead = true;
                    OnDieCallback?.Invoke();
                }
            }
        }

        private int currentHealth;

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage) => CurrentHealth -= damage;

        public void Revive()
        {
            Dead = false;
            CurrentHealth = maxHealth;
        }
    }
}
