using UnityEngine;
using System.Collections;

public class squareDetection : MonoBehaviour
{

	
	/*
 - We're importing a texture (image), its public so that we can input any picure.
 - We create a new texture whihch is empty.
 - We create an array of Colors, which holds the RGB values for each pixel
 - We create a 2D array of integers that will hold the greyscale from Color
 - We create another 2D array which is to hold all the tag-values of the pixels
 - We create min and max values for detected rectangles
 - We create a integer that will hold the currect tag number
 - We create a CubeCorner array to hold any amount of rectangle objects to be detected
*/
	public Texture2D inputTex;
	Texture2D texture;
	Color[] pix;
	int[,] positionValue;
	int[,] tags;
	int xmin, xmax, ymin, ymax;
	int tagCheck = 0;
	CubeCorner[] cubecorners;
	
	void Start ()
	{
		
		/*
		 - We set the empty texture to the inputTex (this might create a copy by reference problem)
		 - We set the renderer of the gameobject this script is attached to, to the texture (input texture)
		 - We set the pix array of Colors to the texture of the texture (input texture)
		*/
		texture = inputTex;
		GetComponent<Renderer> ().material.mainTexture = texture;
		pix = texture.GetPixels ();
		
		/*
		- Both of our 2D arrays of integers take the height and width of the image pixels
		- The array of tags gets 10 pixels worth of padding, otherwise it will go out of bounds later
		*/
		
		positionValue = new int[texture.width, texture.height];
		tags = new int[texture.width + 1, texture.height + 1];
		
		
		/*
		- We loop through the width and height of the image
		- We set the position values to the greyscale of the image, 0-255
		- We set all tags to 0
		*/
		
		for (int i = 1; i < texture.width; i++) {
			for (int j = 1; j < texture.height; j++) {
				positionValue [i, j] = (int)(pix [i + (pix.Length / texture.height * j)].grayscale * 255);
				tags [i, j] = 0;
				
				/*
		- If a pixel is not white (some thresholding has to be done before this)
		*/
				if (positionValue [i, j] < 255) {
					/*
		- We loop through a 3*3 kernel, which is applied to the pixel which was not white
		*/
					for (int ky=-1; ky <= 1; ky++) {
						for (int kx=-1; kx <= 1; kx++) {
							/*
		- If any of the 9 pixels from the kernel has a tag
		*/
							if (tags [i + ky, j + kx] != 0) {
								/*
		- We then tag our current non-white pixel with the same tag
		*/
								tags [i, j] = tags [i + ky, j + kx];
							}
						}
					}
					
					
					/*
		- If none of the neighbours has a tag, we create a new one by incrementing tagCheck
		- And we then tag our non-white pixel with the new tag
		*/
					if (tags [i, j] == 0) {

						tags [i, j] = tagCheck;
						tagCheck++;
					}
				}
			}
		}
		
		/*
		- We set an array of CubeCorners to the size of TagCheck, which is the total amount of objects
		- We set the values of all corners to their theoretical maximum value, either 0 or the size of the input image
		*/
		
		cubecorners = new CubeCorner[tagCheck];

		for (int k = 0; k < tagCheck; k++) {
			cubecorners[k] = new CubeCorner();
		}

		for (int k = 0; k < tagCheck; k++) {

			cubecorners [k].xmin = texture.width;
			cubecorners [k].xmax = 0;
			cubecorners [k].ymin = texture.height;
			cubecorners [k].ymax = 0;

		}
		
		/*
		- Loop through all the pixels
		- Loop through the cube-objects
		*/
		
		for (int i = 1; i < texture.width; i++) {
			for (int j = 1; j < texture.height; j++) {
				for (int k = 1; k < cubecorners.Length; k++) {
					
					/*
		- I ask - If there is something tagged as k (0 -> amount of objects), 
		- then set the corresponding cubecorner (k) values:
		- if the i or j is lower or higher than the min or max of the object

		- This will create values for all four corner of a a rectangle of any detected object
		- Ready to be created in to a cube later on
		*/
					if (tags [i, j] == k && i < cubecorners [k].xmin) {
						cubecorners [k].xmin = i;
					}
					if (tags [i, j] == k && i > cubecorners [k].xmax) {
						cubecorners [k].xmax = i;
					}
					if (tags [i, j] == k && j < cubecorners [k].ymin) {
						cubecorners [k].ymin = j;
					}
					if (tags [i, j] == k && j > cubecorners [k].ymax) {
						cubecorners [k].ymax = j;
					}
				}
			}
		}

		/*for (int i = 1; i < texture.width; i++) {
			for (int j = 1; j < texture.height; j++) {
				for (int k = 1; k < cubecorners.Length; k++) {

					}
					if (tags [i, j] == k && j < cubecorners [k].ymin) {
						cubecorners [k].ymin = j;
					}
					if (tags [i, j] == k && j > cubecorners [k].ymax) {
						cubecorners [k].ymax = j;
					}
				}
			}
		}*/
		
		/*
		 *This is a test 
		 *to check the numbers of all the cubes
		 */
		for (int k = 1; k < cubecorners.Length; k++) {
			print ("Cube " + k + " xmin value is: " + cubecorners [k].xmin);
			print ("Cube " + k + " xmax value is: " + cubecorners [k].xmax);
			print ("Cube " + k + " ymin value is: " + cubecorners [k].ymin);
			print ("Cube " + k + " ymax value is: " + cubecorners [k].ymax);

			//starting to create cubes

			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);

			cube.transform.position = new Vector3 ((cubecorners [k].xmax + cubecorners [k].xmin) / 2, 0, (cubecorners [k].ymax + cubecorners [k].ymin) / 2);
			cube.transform.localScale = new Vector3 (cubecorners [k].xmax - cubecorners [k].xmin, 1, cubecorners [k].ymax - cubecorners [k].ymin);
			cube.AddComponent<Rigidbody> ().useGravity = false;
			cube.GetComponent<Rigidbody> ().isKinematic = true;
				
			Color newColor = new Color ();
			newColor = new Color (tagCheck, tagCheck, tagCheck, 1.0f);
			cube.GetComponent<Renderer> ().material.color = newColor;  //Will give cubes color after merging
		}		
	}

	
	// Update is called once per frame
	void Update ()
	{
		
		/*
 - This is a test, the test will print the following if and only if the image has 7 objects
*/
		if (tagCheck == 7) {
			print ("tagchch" + tagCheck);
		}
	}
}
/*
 *A CubeCorner Class, only used to store the four corners of a rectangle, to be come a cube 
 */

public class CubeCorner{
	public int xmin, xmax, ymin, ymax;
	
	void setXmin(int i){
		this.xmin = i;
	}
	void setXmax(int i){
		this.xmax = i;
	}
	void setYmin(int i){
		this.ymin = i;
	}
	void setYmax(int i){
		this.ymax = i;
	}
	int getXmin(){
		return xmin;
	}
	int getXmax(){
		return xmax;
	}
	int getYmin(){
		return ymin;
	}
	int getYmax(){
		return ymax;
	}

}

