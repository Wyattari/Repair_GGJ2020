﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class MovingSphere : MonoBehaviour {

	[SerializeField, Range(0f, 100f)] float maxSpeed = 10f;
	[SerializeField, Range(0f, 100f)] float maxAcceleration = 10f, maxAirAcceleration = 1f;
	[SerializeField, Range(0f, 10f)] float jumpHeight = 2f;
	[SerializeField, Range(0, 5)] int maxAirJumps = 0;
	[SerializeField, Range(0, 90)] float maxGroundAngle = 25f, maxStairsAngle = 50f;
	[SerializeField, Range(0f, 100f)] float maxSnapSpeed = 100f;
	[SerializeField, Min(0f)] float probeDistance = 1f;
	[SerializeField] LayerMask probeMask = -1, stairsMask = -1;

	Rigidbody body;
	Vector3 velocity, desiredVelocity;
	bool desiredJump;
	Vector3 contactNormal, steepNormal;
	int groundContactCount, steepContactCount;
	bool OnGround => groundContactCount > 0;
	bool OnSteep => steepContactCount > 0;
	int jumpPhase;
	float minGroundDotProduct, minStairsDotProduct;
	int stepsSinceLastGrounded, stepsSinceLastJump;
    
	public GameObject PrefabCharacter;
    private GameObject PlayerRoot;

	private Vector2 playerInput;

	public int PlayerId;

    [SerializeField] AnimationCurve fallVelocity;
    float fallTime;

    void OnValidate () {
		minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
		minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
	}

	void Awake () {
		body = GetComponent<Rigidbody>();
		OnValidate();

        PlayerRoot = Instantiate(PrefabCharacter);
		Subscribe();
	}

	void OnDestroy() {
		Destroy(PlayerRoot);
		Unsubscribe();
	}

	void Subscribe() {
		Unsubscribe();
		GameManager.Instance.Events.OnPlayerMove += Events_OnPlayerMove;
		GameManager.Instance.Events.OnPlayerJump += Events_OnPlayerJump;
	}

	void Unsubscribe() {
		GameManager.Instance.Events.OnPlayerMove -= Events_OnPlayerMove;
		GameManager.Instance.Events.OnPlayerJump -= Events_OnPlayerJump;
	}

	void Events_OnPlayerMove(int playerId, Vector2 move) {
		if (playerId != PlayerId) { return; }

		playerInput = move;
	}

	void Events_OnPlayerJump(int playerId) {
		if (playerId != PlayerId) { return; }

		desiredJump = true;
	}

	void Update () {
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);

		desiredVelocity =
			new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        PlayerRoot.transform.position = transform.position;
	}

	void FixedUpdate () {
		UpdateState();
		AdjustVelocity();

		if (desiredJump) {
			desiredJump = false;
			Jump();
		}

		body.velocity = velocity;
		ClearState();
	}

	void ClearState () {
		groundContactCount = steepContactCount = 0;
		contactNormal = steepNormal = Vector3.zero;
	}

	void UpdateState () {
		stepsSinceLastGrounded += 1;
		stepsSinceLastJump += 1;
		velocity = body.velocity;
		if (OnGround || SnapToGround() || CheckSteepContacts()) {
			stepsSinceLastGrounded = 0;
			if (stepsSinceLastJump > 1) {
				jumpPhase = 0;
			}
			if (groundContactCount > 1) {
				contactNormal.Normalize();
			}
		}
		else {
			contactNormal = Vector3.up;
		}
        if (!OnGround) {
            if (body.velocity.y <= 0) {
                fallTime += Time.deltaTime;
                velocity += new Vector3(0, -Mathf.Lerp(0, .5f, fallVelocity.Evaluate(fallTime*2)), 0);
            }
        }
    }

	bool SnapToGround () {
		if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2) {
			return false;
		}
		float speed = velocity.magnitude;
		if (speed > maxSnapSpeed) {
			return false;
		}
		if (!Physics.Raycast(
			body.position, Vector3.down, out RaycastHit hit,
			probeDistance, probeMask
		)) {
			return false;
		}
		if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer)) {
			return false;
		}

		groundContactCount = 1;
		contactNormal = hit.normal;
		float dot = Vector3.Dot(velocity, hit.normal);
		if (dot > 0f) {
			velocity = (velocity - hit.normal * dot).normalized * speed;
		}
		return true;
	}

	bool CheckSteepContacts () {
		if (steepContactCount > 1) {
			steepNormal.Normalize();
			if (steepNormal.y >= minGroundDotProduct) {
				steepContactCount = 0;
				groundContactCount = 1;
				contactNormal = steepNormal;
				return true;
			}
		}
		return false;
	}

	void AdjustVelocity () {
		Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

		float currentX = Vector3.Dot(velocity, xAxis);
		float currentZ = Vector3.Dot(velocity, zAxis);

		float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
		float maxSpeedChange = acceleration * Time.deltaTime;

		float newX =
			Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
		float newZ =
			Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

		velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
	}

	bool isJumping = false;
	void Jump () {
		Vector3 jumpDirection;
		if (OnGround) {
			jumpDirection = contactNormal;
		}
		else if (OnSteep) {
			jumpDirection = steepNormal;
			jumpPhase = 0;
		}
		else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps) {
			if (jumpPhase == 0) {
				jumpPhase = 1;
			}
			jumpDirection = contactNormal;
		}
		else {
			return;
		}
        fallTime = 0;
        isJumping = true;
		try {
			AudioController.Instance.PlayJump();
		} catch (NullReferenceException) {
			Debug.LogWarning("Couldn't play jump sound. Is AudioController in the scene?");
		}
		stepsSinceLastJump = 0;
		jumpPhase += 1;
		float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
		jumpDirection = (jumpDirection + Vector3.up).normalized;
		float alignedSpeed = Vector3.Dot(velocity, jumpDirection);
		if (alignedSpeed > 0f) {
			jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
		}
		velocity += jumpDirection * jumpSpeed;
	}

	void OnCollisionEnter (Collision collision) {
		if (isJumping) {
			isJumping = false;
			AudioController.Instance.PlayLand();
		}
		EvaluateCollision(collision);
	}

	void OnCollisionStay (Collision collision) {
		EvaluateCollision(collision);
	}

	void EvaluateCollision (Collision collision) {
		float minDot = GetMinDot(collision.gameObject.layer);
		for (int i = 0; i < collision.contactCount; i++) {
			Vector3 normal = collision.GetContact(i).normal;
			if (normal.y >= minDot) {
				groundContactCount += 1;
				contactNormal += normal;
			}
			else if (normal.y > -0.01f) {
				steepContactCount += 1;
				steepNormal += normal;
			}
		}
	}

	Vector3 ProjectOnContactPlane (Vector3 vector) {
		return vector - contactNormal * Vector3.Dot(vector, contactNormal);
	}

	float GetMinDot (int layer) {
		return (stairsMask & (1 << layer)) == 0 ?
			minGroundDotProduct : minStairsDotProduct;
	}
}
