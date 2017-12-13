using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateController : MonoBehaviour {

	public static StateController instance;
	public Level currentLevel;

	public bool HasCompletedIntro {get;set;}

	public List<Level> Levels{
		get; private set;
	}
	public int PlayerPositionInMapScene {get; set;}

	void Awake()
	{
		if(instance == null){
			DontDestroyOnLoad(gameObject);
			instance = this;
			Levels = CreateAndPopulateLevelList();
			PlayerPositionInMapScene = 0;
			HasCompletedIntro = false;
		}
		else{
			Destroy(gameObject);
		}
	}

	public void SetLevelCompletedAndUnlockNextLevel(int levelNo){
		var level = Levels.Find(x => x.LevelNo == levelNo);
		level.IsComplete = true;

		var nextLevel = Levels.Find(x => x.LevelNo == (levelNo + 1));
		if(nextLevel != null){
			nextLevel.IsLocked = false;
		}
	}

	public Level GetMaxReachedLevel(){
		var unlockedLevels = Levels.FindAll(x => x.IsLocked == false);
		var maxReachedLevel = unlockedLevels.Aggregate((l1,l2) => l1.LevelNo > l2.LevelNo ? l1 : l2);
		return maxReachedLevel;
	}

	private List<Level> CreateAndPopulateLevelList(){
		Level level0 = ScriptableObject.CreateInstance<Level>();
		level0.Init(0, false);

		Level level1 = ScriptableObject.CreateInstance<Level>();
		level1.Init(1, false);

		Level level2 = ScriptableObject.CreateInstance<Level>();
		level2.Init(2, true);

		Level level3 = ScriptableObject.CreateInstance<Level>();
		level3.Init(3, true);

		Level level4 = ScriptableObject.CreateInstance<Level>();
		level4.Init(4, true);

		Level level5 = ScriptableObject.CreateInstance<Level>();
		level5.Init(5, true);
		
		return new List<Level>(){level0, level1, level2, level3, level4, level5};
	}

}
