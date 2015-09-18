using UnityEngine;
using System.Collections;

public class Globals {
	
	public enum InputTypes{
		KEYBOARD,
		VOICE
	}

	public static InputTypes inputType = InputTypes.VOICE;
	public static bool isServer = false;
}
