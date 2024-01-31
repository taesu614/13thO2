using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
	[SerializeField]
	private	RouletteJ	rouletteJ;
	[SerializeField]
	private	Button		buttonSpin;

	private void Awake()
	{
		buttonSpin.onClick.AddListener(()=>
		{
			buttonSpin.interactable = false;
			rouletteJ.Spin(EndOfSpin);
		});
	}

	private void EndOfSpin(RoulettePieceData selectedData)
	{
		buttonSpin.interactable = true;

		Debug.Log($"{selectedData.index}:{selectedData.description}");
	}
}

