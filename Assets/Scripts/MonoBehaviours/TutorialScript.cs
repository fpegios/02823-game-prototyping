using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {
	private Rigidbody2D rb;

	private Text speechText;
	private Queue<string> speeches;

  	void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
		speechText = GameObject.Find("Speech").GetComponent<Text>();
    }

	void Start()
	{
		speeches = new Queue<string>(new [] {
			"Press SPACE to jump",
			"Good. You can also hold SPACE for a longer jump",
			"Tapping SPACE will let you jump only a little",
			"Now watch out for enemies!",
			"Press SPACE on the trampoline to jump even higher",
			"Powerups can be collected. Consume with 'R'"
		});
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("TutorialTrigger")){
			var speech = speeches.Dequeue();
			speechText.text = speech;
		}
	}
}
