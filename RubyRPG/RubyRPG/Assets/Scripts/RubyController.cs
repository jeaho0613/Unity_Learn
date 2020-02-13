using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public GameObject HPbar;
    public GameObject projectilePrefab;
    public float speed = 3.0f;
    public AudioClip attackaudio;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
       
    public int currentHealth;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;

    Animator animator;
    AudioSource audioSource;
    Vector2 lookDirection = new Vector2(1, 0);

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 500);

        animator.SetTrigger("Launch");
    }

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
        
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = rigidbody2d.position;

        position = position + move * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);


        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
            PlaySound(attackaudio);
            
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit =
                Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
                lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider !=null)
            {
                NonPlayerCharacter character =
                    hit.collider.GetComponent<NonPlayerCharacter>();
                if(character !=null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
