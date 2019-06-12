using System.Collections;
using System.Collections.Generic;
using SceneManagers;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float MaxHealth;

    private Menu.PauseMenu _deathMenu;
    private float _health;
    private bool _dead = false;
    public ParticleSystem DeathParticle;

    private void Awake()
    {
        _deathMenu = GameObject.Find("Canvas").GetComponent<Menu.PauseMenu>();
        _health = MaxHealth;
    }

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
        Time.timeScale = 0.1f;
        DeathParticle.Play();
        yield return new WaitForSecondsRealtime(1);
        var enemies = GameObject.FindGameObjectsWithTag("enemy");
        Time.timeScale = 1.0f;

        foreach (var enemy in enemies)
        {
            StartCoroutine(enemy.GetComponent<EnemyMove>().Die(enemy.transform.position));
            yield return new WaitForSecondsRealtime(0.5f);
        }
        GameObject.FindObjectOfType<Enemy.WaveManager>().SetDead();
        yield return new WaitForSecondsRealtime(2);
 
        _deathMenu.GameOver();
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
