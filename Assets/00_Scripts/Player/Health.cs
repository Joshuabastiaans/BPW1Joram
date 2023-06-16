using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("UI")]
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Transform heartsContainer;
    [SerializeField] private float heartSpacing = 20f;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            //restart level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        // Clear existing hearts
        foreach (Transform child in heartsContainer)
        {
            Destroy(child.gameObject);
        }

        // Calculate the total width of all hearts
        float totalWidth = (heartPrefab.rectTransform.rect.width + heartSpacing) * currentHealth - heartSpacing;

        // Calculate the starting position for the hearts
        float startX = -totalWidth;

        // Create hearts based on the current health with spacing
        for (int i = 0; i < currentHealth; i++)
        {
            Image heart = Instantiate(heartPrefab, heartsContainer);
            heart.rectTransform.anchoredPosition = new Vector2(startX + (heartPrefab.rectTransform.rect.width + heartSpacing) * i, 0f);
        }
    }
}
