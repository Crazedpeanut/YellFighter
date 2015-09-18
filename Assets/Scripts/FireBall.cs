using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	GameObject fireBall;

	public int playerId;
	public float speed = 0.2f;
	public int x_direction = -1;
	public int y_direction = 0;

	public int damage = 20;

	bool isStarted = false;

	// Use this for initialization
	void Start () {
		fireBall = (GameObject)this.transform.gameObject;
	}

	public void startFireBall()
	{
		isStarted = true;
	}

	// Update is called once per frame
	void Update () {
		if (isStarted) {
			Vector3 currentPos = fireBall.transform.position;
			
			currentPos.y += speed * y_direction;
			currentPos.x += speed * x_direction;
			
			fireBall.transform.position = currentPos;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Player") {
			GameObject playerGO = collision.gameObject;
			Player player = playerGO.GetComponent<Player>();

			if(player.playerID == this.playerId)
			{			

			}
			else
			{
				player.hitPlayer(damage);
				Explode ();
			}
	
		}

		Debug.Log ("Collision with tag: " + collision.transform.tag);
	}

	void Explode()
	{
		Destroy (this.gameObject);
	}
}
