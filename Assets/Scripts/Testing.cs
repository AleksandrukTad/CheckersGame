using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

	public void redirectToSurvey(){
		try
		{
			Debug.Log("Redirecting...");
			System.Diagnostics.Process.Start("https://goo.gl/forms/qzzVA7Gi4VVuuZ2o1");
		}
		catch
		{}
	}
}
