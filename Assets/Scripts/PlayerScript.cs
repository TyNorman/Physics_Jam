﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{
	public int PlayerNumber = 0;

	InputManager inputManager = InputManager.Instance;

	private const float MAXVELOCITY = 1.5f;

	public float currentVelocityX = 0;
	public float currentVelocityY = 0;

	public float acceleration = 2.5f;
	public float deceleration = 2.5f;

	private KeyCode previouslyHeldKey;

	// Use this for initialization
	void Start () 
	{
		inputManager.Key_Pressed += PlayerInput;
		inputManager.Key_Released += ApplyDeceleration;

		AttachBeam ();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	private void PlayerInput(int playerNum, KeyCode key)
	{
		Vector3 newPosition = gameObject.transform.position;
		int horizontalDirectionModifier = 0;
		int verticalDirectionModifier = 0;

		if (playerNum == PlayerNumber) 
		{
			if (key == inputManager.PlayerKeybindArray [playerNum].UpKey || key == inputManager.PlayerKeybindArray [playerNum].DownKey)
			{
				if(key == inputManager.PlayerKeybindArray [playerNum].UpKey)
					verticalDirectionModifier = 1;
				else
					verticalDirectionModifier = -1;

				if (currentVelocityY < MAXVELOCITY)
					currentVelocityY += acceleration * Time.deltaTime;
			}
			if (key == inputManager.PlayerKeybindArray [playerNum].LeftKey || key == inputManager.PlayerKeybindArray [playerNum].RightKey)
			{
				if(key == inputManager.PlayerKeybindArray [playerNum].LeftKey)
					horizontalDirectionModifier = -1;
				else
					horizontalDirectionModifier = 1;

				if (currentVelocityX < MAXVELOCITY)
					currentVelocityX += acceleration * Time.deltaTime;
			}
		}

		newPosition.x += currentVelocityX * horizontalDirectionModifier * Time.deltaTime;
		newPosition.y += currentVelocityY * verticalDirectionModifier * Time.deltaTime;
		gameObject.transform.position = newPosition;

		previouslyHeldKey = key;
	}

	private void ApplyDeceleration(int playerNum, KeyCode key)
	{
		Vector3 newPosition = gameObject.transform.position;
		int horizontalDirectionModifier = 0;
		int verticalDirectionModifier = 0;
		
		if (playerNum == PlayerNumber) 
		{
			if (key == previouslyHeldKey) //If the player let go of the key they were holding, decelerate
			{
				if(key == inputManager.PlayerKeybindArray [playerNum].UpKey)
					verticalDirectionModifier = 1;
				if (key == inputManager.PlayerKeybindArray [playerNum].DownKey)
					verticalDirectionModifier = -1;

				if(key == inputManager.PlayerKeybindArray [playerNum].LeftKey)
					horizontalDirectionModifier = -1;
				if(key == inputManager.PlayerKeybindArray [playerNum].RightKey)
					horizontalDirectionModifier = 1;
				
				if (currentVelocityY > 0)
					currentVelocityY -= deceleration * Time.deltaTime;
				
				if (currentVelocityX > 0)
					currentVelocityX -= deceleration * Time.deltaTime;
			}
		}

		if (currentVelocityX < 0)
			currentVelocityX = 0;
		if (currentVelocityY < 0)
			currentVelocityY = 0;

		newPosition.x += currentVelocityX * horizontalDirectionModifier * Time.deltaTime;
		newPosition.y += currentVelocityY * verticalDirectionModifier * Time.deltaTime;
		gameObject.transform.position = newPosition;
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//Replace with a list of collision objects that the player is unable to pass.
		switch (coll.gameObject.name) 
		{
		case "Player":
		case "Tile_Rock":
		case "Tile_RockCorner":
			currentVelocityX = 0;
			currentVelocityY = 0;
			break;
		}
	}

	void AttachBeam()
	{
		GameObject beam = GameObject.Instantiate (Resources.Load ("Prefabs/LineSegment")) as GameObject;
		beam.transform.position = gameObject.transform.position;
		beam.transform.localScale = new Vector2 (20, 1);
		beam.transform.parent = gameObject.transform;

		//Set beam colour
		SpriteRenderer beamRenderer = beam.transform.GetComponent<SpriteRenderer> ();
		beamRenderer.material.color = new Color (Random.value, Random.value, Random.value, 0.7f);
	}
}
