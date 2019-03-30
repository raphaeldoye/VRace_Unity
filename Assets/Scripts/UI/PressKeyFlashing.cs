using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class PressKeyFlashing : MonoBehaviour
{
	[SerializeField] private string nextScene;
	[SerializeField] private float flashDuration;
	private CanvasGroup cg;
	private int fade = -1;

	void Start()
    {
		cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
		if (Input.anyKey)
			ChangeScene();

		Flashing();
    }

	private void Flashing()
	{
		float newValue = cg.alpha + ((Time.deltaTime / flashDuration) * fade);

		if (newValue <= 0)
		{
			cg.alpha = 0;
			fade = 1;
		}
		else if(newValue >= 1)
		{
			cg.alpha = 1;
			fade = -1;
		}
		else
		{
			cg.alpha = newValue;
		}
	}

	private void ChangeScene()
	{
		SceneManager.LoadScene(nextScene);
	}
}
