using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	public int PlayerSpeed = 10;
	private float MoveAmmount = 0.0f;
	private bool isActive = false;
	private bool isOutOfBounds = false;

	//Getters and Setters
	public bool IsActive
	{
		get
		{
			return isActive;
		}
		set
		{
			isActive = value;
		}
	}
	
	public bool OutOfBounds
	{
		get
		{
			return isOutOfBounds;
		}
		set
		{
			isOutOfBounds = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive)
		{
			if(Application.platform == RuntimePlatform.WP8Player)
			{
				MoveAmmount = (PlayerSpeed  * Input.acceleration.x )* Time.deltaTime;
				transform.Translate(Vector3.right * MoveAmmount);
				MoveAmmount = (PlayerSpeed * 0.5f * ((-Input.acceleration.z + 0.5f)*2)) * Time.deltaTime;
				transform.Translate(Vector3.forward * MoveAmmount);
			}
			else
			{
				MoveAmmount = (PlayerSpeed *Input.GetAxis("Horizontal")) * Time.deltaTime;
				transform.Translate(Vector3.right * MoveAmmount);
				MoveAmmount = (PlayerSpeed *Input.GetAxis("Vertical")) * Time.deltaTime;
				transform.Translate(Vector3.forward * MoveAmmount);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.collider.gameObject.tag == "OutOfRing")
		{
			isOutOfBounds = true;
		}
	}
}
