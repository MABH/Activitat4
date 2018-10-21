using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	//public float speed = 20f;				// The speed the rocket will fire at.
    public Sprite attackBarSprite;

    //private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
    private Animator anim;					// Reference to the Animator component.
    public delegate void GunFired();
    public event GunFired gunFired;
    private enum States { Down, Fire, Up }
    private States state = States.Up;
    public float speed = 0f;                // The speed the rocket will fire at.
    private float targetSpeed = 0f;
    private Transform attackBar;

    void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();

        GameObject attackBarObject = new GameObject("Power");
        attackBar = attackBarObject.transform;
        attackBar.SetParent(transform);
        attackBar.localPosition = Vector3.zero;
        attackBar.localRotation = Quaternion.identity;
        attackBar.localScale = Vector3.up * 2 + Vector3.forward;
        SpriteRenderer rend = attackBarObject.AddComponent<SpriteRenderer>();
        rend.sprite = attackBarSprite;
        rend.sortingLayerID = transform.root.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
        
        //playerCtrl = transform.root.GetComponent<PlayerControl>();
    }


    /*void Update ()
	{
		// If the fire button is pressed...
		if(Input.GetButtonDown("Fire1"))
		{
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");
			GetComponent<AudioSource>().Play();

			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
		}
	}*/

    private void Update()
    {
        if (targetSpeed > 0)
        {
            state = States.Down;
            if (speed >= targetSpeed) state = States.Fire;
        }
        switch (state)
        {
            case States.Down:
                speed += Time.deltaTime * 30;
                attackBar.localScale += Vector3.right * 0.01f;
                attackBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, attackBar.localScale.x);
                break;
            case States.Fire:
                Fire();
                state = States.Up;
                break;
        }
    }

    public void FireUp()
    {
        state = States.Fire;
    }
    public void FireDown()
    {
        state = States.Down;
    }

    public void Fire()
    {
        anim.SetTrigger("Shoot");
        GetComponent<AudioSource>().Play();
        Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, transform.rotation) as Rigidbody2D;
        if (transform.root.GetComponent<PlayerControl>().facingRight)
            bulletInstance.velocity = transform.right.normalized * speed;
        // else bulletInstance.velocity = new Vector2(-transform.right.x, transform.right.y).normalized * speed;
        else bulletInstance.velocity = new Vector2(-transform.right.x, -transform.right.y).normalized * speed;
        bulletInstance.GetComponentInChildren<Rocket>().ignoreTag = transform.root.tag;
        targetSpeed = speed = 0;
        attackBar.localScale = Vector3.up * 2 + Vector3.forward;
        if (gunFired != null)
        {
            gunFired();
        }
       /* bulletInstance.velocity = transform.right.normalized * speed;
        speed = 0;
        attackBar.localScale = Vector3.up * 2 + Vector3.forward;
        */
    }

    public void Fire(float targetSpeed)
    {
        this.targetSpeed = targetSpeed;
    }
}
