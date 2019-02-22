using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlashingBackground : MonoBehaviour
{
	[SerializeField] private Image backgroundImage;
	[SerializeField] private float maxIntervalTime = 3.0f;
	[SerializeField] private float maxAlphaIntervalTime = 0.5f;
	[SerializeField] private int maxBlinkingIteration = 6;

	void Start()
    {
		StartCoroutine(Blinking());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

	private IEnumerator Blinking()
	{
		float time;
		int blinkingIteration;
		while (true)
		{
			time = Random.value*maxIntervalTime;
			yield return new WaitForSeconds(time);
			blinkingIteration = Mathf.FloorToInt(Random.value*maxBlinkingIteration);


			for (int i = 0; i < blinkingIteration; i++)
			{
				time = Random.value*maxAlphaIntervalTime;
				yield return new WaitForSeconds(time);
				float newAlpha = Random.value;
				SetAlpha(newAlpha);
			}

			SetAlpha(0.0f);
		}
	}

	private void SetAlpha(float newAlpha)
	{	
		Color newColor = backgroundImage.color;
		newColor.a = newAlpha;
		backgroundImage.color = newColor;
	}
}
