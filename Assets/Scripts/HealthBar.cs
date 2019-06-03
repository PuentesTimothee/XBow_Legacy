using System.Collections;
using System.Collections.Generic;
using SceneManagers;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float MaxHealth;

    private float _health;
    private bool _dead = true;
    public ParticleSystem DeathParticle;
    
    private void Update()
    {
        if (_health <= 0 && !_dead)
        {
            _dead = true;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        Time.timeScale = 0.0f;
        DeathParticle.Play();
        yield return new WaitForSeconds(1);
        Time.timeScale = 1.0f;
        SceneManager.ActualScene.LoadScene("MenuDeath");
    }
    
    public void SetupForGame()
    {
        _health = MaxHealth;
        _dead = false;
    }

    public void Disable()
    {
        _health = 0;
        _dead = true;
    }
    
    public float GetHealth()
    {
        return _health;
    }
    
    public float HealthPercentage { get { return (_health / MaxHealth); } }

    public void TakeDamage(float damage)
    {
        if (damage > _health)
            _health = 0;
        else
            _health -= damage;
    }

    public void Heal(float healing)
    {
        if (healing + _health > MaxHealth)
            _health = MaxHealth;
        else
            _health += healing;
    }
}
