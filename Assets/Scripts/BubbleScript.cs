using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour {

    private float count = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (count < 6f)
        {
            count += Time.deltaTime;
            StartCoroutine(Blink(count));
        }
        else
        {
            count = 0;
            DisableShield();
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("MovingEnemy") || other.transform.CompareTag("Rock"))
        {
            Destroy(other.gameObject);
            DisableShield();
        }
    }

    private void DisableShield()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator Blink(float count)
    {
        while (count >= 4.5f & count < 6f)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.3f);
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.3f);
        }
        
    }

}
