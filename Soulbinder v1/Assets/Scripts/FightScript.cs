using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightScript : MonoBehaviour
{
    // Components
    public P2FightScript p2;
    public Rigidbody2D rb;

    // UI
    public Text whoWon;
    public Button retry;
    public Text dmg1;
    public Text dmg2;
    public Slider healthBar;
    public Slider attackBar;
    public Slider dashSlider;

    // General ariables
    public float moveSpeed = 5f;
    public bool isGrounded = false;
    public float health;
    public float maxHealth = 200f;
    bool gameOver = false;

    // Attack variables
    private bool attacking = false;
    private bool regenerating = false;
    public float attackMeter;
    public float maxAttackMeter = 500;
    public float damage = 20f;
    public float spend = 6f;
    public float regen = 1f;
    public float damageOverTime;
    public float dist;

    // Dash variables
    private bool dashing = false;
    private bool regeningDash = false;
    public float dashMeter;
    public float maxDashMeter = 500;
    public float spendDash = 4f;
    public float regenDash = 1f;

    // Line
    private LineRenderer line;
    public Transform player2;
    private int currLines = 0;
    public Material material;

    void Start()
    {
        health = maxHealth;
        attackMeter = maxAttackMeter;
        dashMeter = maxDashMeter;
        rb = GetComponent<Rigidbody2D>();
        CreateLine();
    }

    void Update()
    {
        // Game over
        if (health <= 0 && gameOver == false)
        {
            gameOver = true;
            whoWon.gameObject.SetActive(true);
            whoWon.text = "Player2 wins!";
            retry.gameObject.SetActive(true);
        } else if (p2.health <= 0 && gameOver == false)
        {
            gameOver = true;
            whoWon.gameObject.SetActive(true);
            whoWon.text = "Player1 wins!";
            retry.gameObject.SetActive(true);
        }

        //Damage text 
        dmg1.text = damage.ToString();
        dmg2.text = p2.damage.ToString();

        // Sliders
        healthBar.value = health;
        healthBar.maxValue = maxHealth;
        attackBar.value = attackMeter;
        attackBar.maxValue = maxAttackMeter;
        dashSlider.value = dashMeter;
        dashSlider.maxValue = maxDashMeter;

        Attack();
        Dash();
        Jump();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, player2.transform.position);
    }

    void FixedUpdate()
    {
        health -= damageOverTime;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Vertical") && isGrounded == true)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
        }
    }

    void CreateLine()
    {
        line = new GameObject("Bind" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.useWorldSpace = true;
        line.numCapVertices = 50;
    }

    void Attack()
    {
        // Inputs
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (attacking == false && regenerating == false)
            {
                attacking = true;
            }
        }
        // Change damage based on distance
        dist = Vector2.Distance(transform.position, player2.transform.position);
        if (dist <= 2)
        {
            damage = 500;
        }
        else if (dist <= 4)
        {
            damage = 250;
        }
        else if (dist <= 6)
        {
            damage = 130f;
        }
        else if (dist <= 8)
        {
            damage = 70f;
        }
        else if (dist > 8)
        {
            damage = 40;
        }
        // Attackmeter maximum and minimum
        if (attackMeter > maxAttackMeter)
        {
            moveSpeed = 5;
            attackMeter = maxAttackMeter;
            regenerating = false;
            attacking = false;
        }
        else if (attackMeter < 0f)
        {
            attackMeter = 0f;
            regenerating = true;
            attacking = false;
        }
        // Spend attacking mode
        if (attacking == true && regenerating == false)
        {
            p2.health -= damage * Time.deltaTime;
            attackMeter -= spend * Time.deltaTime;
        }
        // Attackmeter regen
        else if (attacking == false && regenerating == true)
        {
            moveSpeed = 3;
            attackMeter += regen * Time.deltaTime;
        }
    }

    void Dash()
    {
        // Inputs
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dashing == false && regeningDash == false)
            {
                if (attacking == false && regenerating == false)
                {
                    dashing = true;
                }
            }
        }
        // Dash maximum and minimum
        if (dashMeter > maxDashMeter)
        {
            dashMeter = maxDashMeter;
            regeningDash = false;
            dashing = false;
        }
        else if (dashMeter < 0f)
        {
            dashMeter = 0f;
            regeningDash = true;
            dashing = false;
        }
        // Spend dashing mode
        if (dashing == true && regeningDash == false)
        {
            moveSpeed = 10;
            dashMeter -= spendDash * Time.deltaTime;
        }
        // Dashmeter regen
        else if (dashing == false && regeningDash == true)
        {
            moveSpeed = 5;
            dashMeter += regenDash * Time.deltaTime;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}