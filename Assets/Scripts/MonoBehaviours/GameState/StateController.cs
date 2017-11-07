using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

	public static StateController stateController;

	private Dictionary<Level, bool> _completedLevels;
	public Dictionary<Level, bool> CompletedLevels{
		get { return _completedLevels; }
	}

	void Awake()
	{
		if(stateController == null){
			DontDestroyOnLoad(gameObject);
			stateController = this;
			_completedLevels = new Dictionary<Level,bool>(){
				{Level.Level1, false},
				{Level.Level2, false},
				{Level.Level3, false},
				{Level.Level4, false},
				{Level.Level5, false}
			};
		}
		else{
			Destroy(gameObject);
		}
	}

}
