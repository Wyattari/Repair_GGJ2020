using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

public class BaseBehaviour : MonoBehaviour {
	protected GameEvents events {
		get { return GameManager.Instance.Events; }
	}

	protected GameState state {
		get { return GameManager.Instance.State; }
	}
}
