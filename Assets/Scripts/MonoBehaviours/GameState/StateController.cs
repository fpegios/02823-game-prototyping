using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

	public static StateController stateController;

	private Dictionary<LevelName, bool> _completedLevels;
	public Dictionary<LevelName, bool> CompletedLevels{
		get { return _completedLevels; }
	}

	void Awake()
	{
		if(stateController == null){
			DontDestroyOnLoad(gameObject);
			stateController = this;
			_completedLevels = new Dictionary<LevelName,bool>(){
				{LevelName.Level1, false},
				{LevelName.Level2, false},
				{LevelName.Level3, false},
				{LevelName.Level4, false},
				{LevelName.Level5, false}
			};
		}
		else{
			Destroy(gameObject);
		}
	}

}
