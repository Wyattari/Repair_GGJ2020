using System;
using System.Collections.Generic;
using UnityEngine;

namespace State {
	[Serializable]
	public class GameState {
		public List<Player> Players = new List<Player>();
	}

	[Serializable]
	public class Player {
		public int Id;
		public Color Color;
		public bool IsAlive;
		public GameObject GameObject;
	}
}

