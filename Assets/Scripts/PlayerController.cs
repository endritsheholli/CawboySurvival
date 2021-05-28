using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	private Rigidbody2D rb2d;
	private Animator myAnimator;
	
	public float Speed = 5;
	
	private bool facingRight;
	
	[SerializeField]
	private Transform[]groundPoints;
	
	[SerializeField]
	private float groundRadius;
	
	[SerializeField]
	private LayerMask whatIsGround;
	
	private bool isGrounded;
	
	private bool jump;
	
	[SerializeField]
	private float jumpForce;
	
	int lives = 3;
    public Text textLives;
	public int score;
    public Text scoreText;
	
	Vector3 startPos;
	

	// Use this for initialization
	void Start () {
		//SaveScore();
		facingRight = true;
		rb2d = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		textLives.text = "Lives: " + lives;
        scoreText.text = "Score:" + score.ToString();
		startPos=transform.position;
		if(PlayerPrefs.HasKey("Score"))
		{
			if(Application.loadedLevel == 1)
			{
				PlayerPrefs.DeleteKey("Score");
				score = 0;
			}
			else{
				score = PlayerPrefs.GetInt("Score");
			}
		}
	}
	
	void Update()
	{
		scoreText.text = "Score:" + score.ToString();
		StoreHighscore(score);
		HandleInput();
	}
	void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag=="hud_gem_blue")
        {
			SoundManagerScript.PlaySound("getPoint");
			DestroyObject(coll.gameObject);
			score++;
            scoreText.text = "Score:" + score.ToString();

        }
        if(coll.gameObject.tag=="barnacle_dead")
        {
			SoundManagerScript.PlaySound("loseLife");
			lives--;
			textLives.text = "Lives:" + lives;
			if(lives == 0)
			{
				
				Destroy (gameObject);
				Application.LoadLevel(0);
			}
        }
		if(coll.gameObject.tag=="flag")
		{
			if(Application.loadedLevel == 1)
			{
				scoreText.text = "Score:" + score.ToString();
				SaveScore();
				Application.LoadLevel(4);
			}
			else if(Application.loadedLevel == 4)
			{
				scoreText.text = "Score:" + score.ToString();
				SaveScore();
				Application.LoadLevel(5);
			}
			else if (Application.loadedLevel == 5)
			{
				scoreText.text = "Score:" + score.ToString();
				SaveScore();
				lives++;
				Application.LoadLevel(6);
			}

		}
    }
	

	
	// Update is called once per frame
	void FixedUpdate ()
	{
		float horizontal = Input.GetAxis("Horizontal");
		isGrounded = IsGrounded();
		HandleMovement(horizontal);
		
		
		Flip(horizontal);
		HandleLayers();
		ResetValues();
	}
	
	private void HandleMovement(float horizontal)
	{
		if(rb2d.velocity.y < 0)
		{
			myAnimator.SetBool("Land", true);
		}
		rb2d.velocity = new Vector2(horizontal * Speed, rb2d.velocity.y);
		
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
		
		if(isGrounded && jump)
		{
			isGrounded = false;
			rb2d.AddForce(new Vector2(0,jumpForce));
			myAnimator.SetTrigger("jump");
		}
	}
	private void Flip(float horizontal)
	{
		if (horizontal>0 && !facingRight || horizontal < 0 && facingRight)
		{
			facingRight = !facingRight;
			
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}
	private bool IsGrounded ()
	{
		if(rb2d.velocity.y <=0)
		{
			foreach(Transform point in groundPoints)
			{
				Collider2D [] colliders  = Physics2D.OverlapCircleAll(point.position, groundRadius,whatIsGround);
				for(int i = 0; i<colliders.Length; i++)
				{
					if(colliders[i].gameObject != gameObject)
					{
						myAnimator.ResetTrigger("jump");
						myAnimator.SetBool("Land", false);
						return true;
					}
				}
			}
		}
		return false;
	}
	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}
	}
	private void ResetValues()
	{
		jump = false;
	}
	private void HandleLayers()
	{
		if(!isGrounded)
		{
			myAnimator.SetLayerWeight(1, 1);
		}
		else
		{
			myAnimator.SetLayerWeight(1,0);
		}
	}
	 void OnBecameInvisible()
    {
		
		SoundManagerScript.PlaySound("loseLife");
		
		lives--;
		textLives.text = "Lives:" + lives;
			if(lives == 0)

			{

				Application.LoadLevel(0);
			}
			else
			{
				transform.position=startPos;
			}
    }
	void SaveScore(){
		PlayerPrefs.SetInt("Score", score);
	}
	void StoreHighscore(int score)
	{
		int oldScore = PlayerPrefs.GetInt("Score", score); 
		PlayerPrefs.Save();
	}
}
