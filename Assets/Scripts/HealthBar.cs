using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField]private newController controller;
	[SerializeField] private Image healthBarTotal;

	private void Update()
	{
		healthBarTotal.fillAmount = controller.health / 100f;
	}
}
