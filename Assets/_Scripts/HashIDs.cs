﻿using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {

	public int dyingState;
	public int deadBool;
	public int locomotionState;
	public int speedFloat;
	public int shotFloat;
	public int aimWeightFloat;
	public int shotClicked;

	void Awake()
	{
		dyingState = Animator.StringToHash ("Base Layer.Dying");
		deadBool = Animator.StringToHash ("Dead");
		locomotionState = Animator.StringToHash ("Base Layer.Locomotion");
		speedFloat = Animator.StringToHash ("Speed");
		shotFloat = Animator.StringToHash ("Shot");
		aimWeightFloat = Animator.StringToHash("AimWeight");
		shotClicked = Animator.StringToHash ("shotClicked");
	}
}
