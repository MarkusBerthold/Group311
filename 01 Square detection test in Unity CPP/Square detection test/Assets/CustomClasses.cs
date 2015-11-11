using UnityEngine;
using System.Collections;

public class CustomClasses : Singleton<CustomClasses> {

	public class Container{

		GameObject parent;

		int [] xValues, yValues;

		Container(int nameNum){

			parent = GameObject.CreatePrimitive(PrimitiveType.Cube);

			parent.name = "Container"+nameNum;

			xValues = new int[1000];
			yValues = new int[1000];
		}

	}
}
