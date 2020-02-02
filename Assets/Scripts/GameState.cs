using System;
using System.Collections.Generic;
using UnityEngine;

namespace State {
	[Serializable]
	public class GameState {
		public List<Player> Players = new List<Player>();
		public int Segment = 0;
	}

	[Serializable]
	public class Player {
		public int Id;
		public bool IsAlive;
		public bool HasWon;
		public GameObject GameObject;
	}
}

