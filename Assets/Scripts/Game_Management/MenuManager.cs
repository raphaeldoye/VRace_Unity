using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private EventSystem eventSystem;

	[Header("MENU")]
	[SerializeField] private Canvas menuCanvas;
	[SerializeField] private List<Button> menuButtons;

	[Header("CAR")]
	[SerializeField] private Canvas carCanvas;
	[SerializeField] private Button carsSubmit;
	[SerializeField] private List<GameObject> cars;

	[Header("MAP")]
	[SerializeField] private Canvas mapCanvas;
	[SerializeField] private Button mapsSubmit;
	[SerializeField] private List<GameObject> maps;

	[Header("OPTIONS")]
	[SerializeField] private Canvas optionCanvas;
	[SerializeField] private Slider lapsSlider;
	[SerializeField] private Button optionSubmit;

	private void Start()
	{
		OpenGameMenu();
	}

	public void SetCanvasState(CanvasGroup cg, bool state)
	{
		cg.alpha = state ? 1 : 0;
		cg.interactable = state;
		cg.blocksRaycasts = state;
	}

	private void DeactivateAll()
	{
		SetCanvasState(menuCanvas.GetComponent<CanvasGroup>(), false);
		SetCanvasState(carCanvas.GetComponent<CanvasGroup>(), false);
		SetCanvasState(mapCanvas.GetComponent<CanvasGroup>(), false);
		SetCanvasState(optionCanvas.GetComponent<CanvasGroup>(), false);

		carsSubmit.enabled = false;
		mapsSubmit.enabled = false;
		optionSubmit.enabled = false;

		for (int i = 0; i < cars.Count; i++)
		{
			cars[i].SetActive(false);
		}

		for (int i = 0; i < maps.Count; i++)
		{
			maps[i].SetActive(false);
		}

		for (int i = 0; i < menuButtons.Count; i++)
		{
			menuButtons[i].enabled = false;
		}
	}

    public void OpenCarsMenu()
	{
		DeactivateAll();
		SetCanvasState(carCanvas.GetComponent<CanvasGroup>(), true);
		cars[(int)GameRules.instance.selectedCar].gameObject.SetActive(true);
		carsSubmit.enabled = true;
	}

	public void OpenMapsMenu()
	{
		DeactivateAll();
		SetCanvasState(mapCanvas.GetComponent<CanvasGroup>(), true);
		maps[(int)GameRules.instance.selectedMap].gameObject.SetActive(true);
		mapsSubmit.enabled = true;
	}

	public void OpenGameMenu()
	{
		DeactivateAll();
		SetCanvasState(menuCanvas.GetComponent<CanvasGroup>(), true);

		for (int i = 0; i < menuButtons.Count; i++)
		{
			menuButtons[i].enabled = true;
		}
	}

	public void OpenOptionsMenu()
	{
		DeactivateAll();
		SetCanvasState(optionCanvas.GetComponent<CanvasGroup>(), true);
		lapsSlider.value = GameRules.instance.GetMaxLap();
		optionSubmit.enabled = true;
	}

	public void SelectCar(int car)
	{
		GameRules.instance.selectedCar = (GameRules.CarsList)car;
		PlayerPrefs.SetInt("selectedCar", car);

		for (int i = 0; i < cars.Count; i++)
		{
			cars[i].SetActive(car==i);
		}
	}

	public void SelectMap(int map)
	{
		GameRules.instance.selectedMap = (GameRules.MapsList)map;
		PlayerPrefs.SetInt("selectedMap", map);

		for (int i = 0; i < maps.Count; i++)
		{
			maps[i].SetActive(map == i);
		}
	}
}
