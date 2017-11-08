using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : ScriptableObject {

	public int LevelNo {get; private set;}
	public bool IsLocked {get; set;}
	public bool IsComplete{get; set;}

	public void Init(int levelNo, bool isLocked){
		this.LevelNo = levelNo;
		this.IsLocked = isLocked;
	}

	public string GetLevelName(){
		return "Level" + this.LevelNo;
	}
}
