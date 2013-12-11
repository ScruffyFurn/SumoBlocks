using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
	
	//Public Variables 
	public int AISpeed = 1;
	public float surgeMultiplier = 2;

	//Private Variables
	private Transform PlayerTransfrom; 
	private bool isActive = false;
	private bool isOutOfBounds = false;
	private bool surging = false;
	private bool dodging = false;
	private float surgeTime;
	private float dodgeTime;

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
		PlayerTransfrom = GameObject.FindWithTag("Player").transform;
		surgeTime = Random.Range(2f,5f);
		dodgeTime = Random.Range (1f,3f);
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive) {
			//updating manuver timers
			surgeTime-=Time.deltaTime;
			dodgeTime-=Time.deltaTime;
			
			if(surgeTime <= 0)
			{
				//toggle surging status
				surging = !surging;
				surgeTime = Random.Range(2f,5f);
			}
			
			if(dodgeTime <= 0)
			{
				//toggle dodge status
				dodging = !dodging;
				dodgeTime = Random.Range (1f,3f);
			}
			
			//surge forward
			if(surging)
			{
				transform.position = Vector3.MoveTowards(transform.position,
				                                         new Vector3(PlayerTransfrom.position.x,0,PlayerTransfrom.position.z), 
				                                         AISpeed * surgeMultiplier * Time.deltaTime);
			}
			//dodge manuever
			else if(dodging)
			{
				//determin which way to dodge
				Vector3 direction = Vector3.Cross(PlayerTransfrom.position-transform.position,transform.up);
				transform.position = Vector3.MoveTowards(transform.position,
				                                         transform.position+direction, 
				                                         AISpeed * Time.deltaTime);

			}
			//normal forward move
			else
			{
				transform.position = Vector3.MoveTowards(transform.position,
				                                         new Vector3(PlayerTransfrom.position.x,0,PlayerTransfrom.position.z), 
				                                         AISpeed * Time.deltaTime);
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
