using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Inspector;

namespace DragonCrashers
{

    public class UnitHealthBehaviour : MonoBehaviour
    {

        [Header("Health Info")]
        [SerializeField]
        [ReadOnly] private int currentHealth;
        public bool isHero; //tracking if unit is hero or not

        [Header("Events")]
        public UnityEvent<int> healthDifferenceEvent;
        public UnityEvent healthIsZeroEvent;

        //Delegate for external systems to detect (IE: Unit's UI)
        public delegate void HealthChangedEventHandler(int newHealthAmount);
        public event HealthChangedEventHandler HealthChangedEvent;

        public float lastCombatTime; //tracking time

        private void Start()
        {
            if (isHero) { 
                
            }
        }
        public void SetupCurrentHealth(int totalHealth)
        {
            currentHealth = totalHealth;
        }

        public void ChangeHealth(int healthDifference, UnitController source = null)
        {
            currentHealth += healthDifference;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                HealthIsZeroEvent();
            }

            healthDifferenceEvent.Invoke(healthDifference);
            DelegateEventHealthChanged();

            // Heal the attacker if this unit is an enemy and the source is a hero
            if (!isHero && source != null && source.healthBehaviour.isHero)
            {
                int healAmount = -healthDifference; // Convert damage to healing
                source.healthBehaviour.ChangeHealth(healAmount, null);
            }
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        void HealthIsZeroEvent()
        {
            healthIsZeroEvent.Invoke();
        }

        void DelegateEventHealthChanged()
        {
            if(HealthChangedEvent != null)
            {
                HealthChangedEvent(currentHealth);
            }
        }

    }
}