using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public enum PlayerStates
	{
		IDLE,
		WALK_LEFT,
		WALK_RIGHT,
		JUMP,
		PUNCH_LEFT,
		PUNCH_RIGHT
	}

	public enum PlayerType
	{
		ROBOT,
		DUDE
	}

	public float speed = 0.2f;
	public float jumpForce = 1f;
	public float moveForce = 1f;
	
	Player player;
	Rigidbody2D rigidBody;
	NetworkView networkView;
	bool inAir = false;
	Vector3 currentPos;
	public GameObject playerFoot;
	public PlayerType playerType;
	public GameObject FBSWNPOINTGO;
	public int health = 100;
	bool facingLeft = false;
	static Stack<string> messageList = new Stack<string> ();
	public Animator animator;
	public SoundLoader soundLoader;
	public int currentPower = 0;
	public int maxPower = 50000;
	public int playerID = 0;
	//public MicrophoneInput microphoneInput;

	// Use this for initialization
	void Start () {
		player = this as Player;
		rigidBody = this.GetComponent<Rigidbody2D> ();
		networkView = this.GetComponent<NetworkView> ();
		currentPos = player.transform.position;
		playerID = NetworkManager.nextPlayerID ();
		playerType = PlayerType.DUDE;
		animator = this.gameObject.GetComponent<Animator>();
		soundLoader = GameObject.Find("Audio Source").GetComponent<SoundLoader>();
	//	microphoneInput = GameObject.Find("Audio Source").GetComponent<MicrophoneInput>();

		foreach (Transform child in this.transform) {
			if(child.name == "PlayerFoot")
			{
				playerFoot = child.gameObject;
			}
			if(child.name == "FBSpwnPNT")
			{
				FBSWNPOINTGO = child.gameObject;
			}
		}
	}
	

	// Update is called once per frame
	void Update () {

		if (networkView.isMine) {
			InputMovement ();
			currentPos = player.transform.position;
			checkInAir ();
			//updatePower();
		};

	}

	void updatePower()
	{
	//	currentPower += (int)microphoneInput.loudness;

	
	}

	public static void AddMessage(string message)
	{
		Debug.Log ("Add Message: " + message);
		messageList.Push (message);
	}

	void InputMovement()
	{
		switch (Globals.inputType) 
		{
		case Globals.InputTypes.KEYBOARD: 
			InputKeyBoardMovemement();
			break;
		case Globals.InputTypes.VOICE:
			InputVoiceMovemement();
			break;
		}
	}

	void InputVoiceMovemement()
	{
		if (messageList.Count == 0)
			return;

		string message = messageList.Pop ();
		
		if (message.Equals("left")) {
			moveLeft();
		}
		
		if (message.Equals("right")) {
			moveRight();
		}
		
		if (message.Equals("jump")) {
			jump();
		}

		if (message.Equals("jump back")) {
			moveLeft();
			jump();
		}

		if (message.Equals("jump forward")) {
			moveRight();
			jump();
		}

		if (message.Equals("fire ball")) {
			fire();
		}

		if (message.Equals("punch")) {
			punch();
		}
		if (message.Equals("kick")) {
			kick();
		}

	}

	void punch()
	{
		soundLoader.playPunch ();
	}
	
	void kick()
	{
		soundLoader.playKick ();
	}

	void InputKeyBoardMovemement()
	{
		Vector3 currentPos;

		if (Input.GetKey (KeyCode.A)) {
			moveLeft();
		}

		if (Input.GetKey (KeyCode.D)) {
			moveRight();
		}

		if (Input.GetKey (KeyCode.W)) {
			jump();
		}

		if (Input.GetKey (KeyCode.Space)) {
			fire();
		}

		if (Input.GetKey (KeyCode.B)) {
			punch();
		}
		
		if (Input.GetKey (KeyCode.N)) {
			kick();
		}
	}

	void moveLeft()
	{
		TurnLeft ();

		if (Globals.inputType == Globals.InputTypes.KEYBOARD) {
			currentPos.x -= speed;
			player.transform.position = currentPos;

		} else {
			rigidBody.AddForce(new Vector2((moveForce * -1) * 3, 0f), ForceMode2D.Impulse);
		}

		if (FBSWNPOINTGO.transform.localPosition.x > 0) {
			Vector3 tmpVec = FBSWNPOINTGO.transform.localPosition;
			tmpVec.x *= -1;
			FBSWNPOINTGO.transform.localPosition = tmpVec;
		}

		facingLeft = true;
	}

	void moveRight()
	{
		TurnRight ();

		if (Globals.inputType == Globals.InputTypes.KEYBOARD) {
			currentPos.x += speed;
			player.transform.position = currentPos;
		} else {
			rigidBody.AddForce(new Vector2(moveForce * 3, 0f), ForceMode2D.Impulse);
		}

		if (FBSWNPOINTGO.transform.localPosition.x < 0) {
			Vector3 tmpVec = FBSWNPOINTGO.transform.localPosition;
			tmpVec.x *= -1;
			FBSWNPOINTGO.transform.localPosition = tmpVec;
		}
		
		facingLeft = false;
	}

	void jump()
	{
		if (!inAir) {
			Debug.Log("Jumping");

			animator.CrossFade("jump_start_up_forward", 0f);

			if (Globals.inputType == Globals.InputTypes.VOICE) {
				rigidBody.AddForce (new Vector2 (0f, jumpForce * 10), ForceMode2D.Impulse);
			} else {
				rigidBody.AddForce (new Vector2 (0f, jumpForce), ForceMode2D.Impulse);
			}

		} else {
				Debug.Log("Not Jumping");
		}

	}

	void checkInAir()
	{
		//int layerMask = ~(1 << 8);
		

		RaycastHit2D hit = Physics2D.Raycast (playerFoot.transform.position, -Vector2.up, 5);

		if (hit.collider != null) {
			int distance = (int)(hit.point.y - currentPos.y);
			//Debug.Log("Hit y: " + hit.point.y + " current y: " + currentPos.y + " name: " + hit.transform.name + " dist: " + distance);
			
			if (distance == 0) {
				inAir = false;
				//Debug.Log ("Not In Air: " + distance + " Hit player: " + hit.transform.name);
			} else {
				inAir = true;
				//Debug.Log ("In Air: " + distance);
			}
		} else {
			inAir = true;
		}
	}

	void fire()
	{
		GameObject fireBall = Instantiate(Resources.Load ("FireBall") as GameObject);
		FireBall fBallScript = fireBall.GetComponentInChildren<FireBall> ();
		Vector3 fireBallPos = this.transform.position;

		soundLoader.playShoot ();

		fireBallPos.y += 0.5f;

		if (facingLeft) {
			fireBallPos.x -= 1.5f;
		} else {
			fireBallPos.x += 1.5f;
		}

		animator.CrossFade ("idle_forward", 0f);

		fireBall.transform.position = fireBallPos;
		fBallScript.playerId = playerID;
		fBallScript.y_direction = 0;
		fBallScript.tag = "FireBall";

		if (facingLeft) {
			fBallScript.x_direction = -1;
		} else {
			fBallScript.x_direction = 1;
		}

		fBallScript.startFireBall ();
	}

	public void hitPlayer(int damage)
	{
		Debug.Log ("Player: " + playerID + " hit. HP from: " + health + " to: " + (health-damage) + "");

		health -= damage;

		if(health <= 0)
		{
			playerDie();
		}
	}

	void TurnLeft()
	{
		Debug.Log ("Turning Left, current x scale is: " + this.gameObject.transform.localScale.x);
		if (this.gameObject.transform.localScale.x > 0) {
			Vector3 tempVec = this.gameObject.transform.localScale;
			tempVec.x *= -1;
			this.gameObject.transform.localScale = tempVec;
		}
	}

	void TurnRight()
	{
		if (this.gameObject.transform.localScale.x < 0) {
			Vector3 tempVec = this.gameObject.transform.localScale;
			tempVec.x *= -1;
			this.gameObject.transform.localScale = tempVec;
		}
	}


	void playerDie()
	{
		Destroy (this.gameObject);
	}
}
