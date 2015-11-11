using UnityEngine;
using System.Collections;


public class squareDetection : MonoBehaviour
{

	
	/*
 - We're importing a texture (image), its public so that we can input any picture.
 - We create a new texture whihch is empty.
 - We create an array of Colors, which holds the RGB values for each pixel
 - We create a 2D array of integers that will hold the greyscale from Color
 - We create another 2D array which is to hold all the tag-values of the pixels
 - We create min and max values for detected rectangles
 - We create a integer that will hold the currect tag number
 - We create a CubeCorner array to hold any amount of rectangle objects to be detected
*/
	public Texture2D inputTex;
	private Texture2D texture;
	Color[] pix;
	Color[,] image;
	int[,] positionValue;
	int[,] tags;
	int xmin, xmax, ymin, ymax;
	int tagCheck = 0;
//	CubeCorner[] cubecorners;

	int label;
	
	void Start ()
	{
		
		/*
		 - We set the empty texture to the inputTex (this might create a copy by reference problem)
		 - We set the renderer of the gameobject this script is attached to, to the texture (input texture)
		 - We set the pix array of Colors to the texture of the texture (input texture)
		*/

		texture = new Texture2D (inputTex.width, inputTex.height);

		Color[,] image = PreProcessing.Instance.GetPixels2D (inputTex);

        image = PreProcessing.Instance.colorDetection(image);

		image = PreProcessing.Instance.Rgb2greyScale (image);

		image = PreProcessing.Instance.simpleBrightness (image, -30);

		image = PreProcessing.Instance.simpleContrast (image, 1.3f);


		image = PreProcessing.Instance.Rank (image, 3, 1);

		
		image = PreProcessing.Instance.Threshold (image, 0.8f);

		image = PreProcessing.Instance.Invert (image);

		image = PreProcessing.Instance.BlobExtraction(image);





		//PreProcessing.Instance.printSample (image, 300,50,10);

		//image = PreProcessing.Instance.Dilate (image);
		//image = PreProcessing.Instance.Dilate (image);
		//image = PreProcessing.Instance.Dilate (image);




		PreProcessing.Instance.SetPixels2D (image, texture);



		this.GetComponent<Renderer> ().material.mainTexture = texture;
		pix = texture.GetPixels ();
		
		/*
		- Both of our 2D arrays of integers take the height and width of the image pixels
		- The array of tags gets 10 pixels worth of padding, otherwise it will go out of bounds later
		*/
		
		positionValue = new int[texture.width, texture.height];
		//tags = new int[texture.width + 1, texture.height + 1];
		
		
		/*
		- We loop through the width and height of the image
		- We set the position values to the greyscale of the image, 0-255
		- We set all tags to 0
		*/
		
		for (int i = 1; i < texture.width; i++) {
			for (int j = 1; j < texture.height; j++) {
				//positionValue [i, j] = (int)(pix [i + (pix.Length / texture.height * j)].grayscale * 255);
				//tags [i, j] = 0;
				
				/*
		- If a pixel is not white (some thresholding has to be done before this)
		*/
				//if (image [i, j].r < 1) {
					/*
		- We loop through a 3*3 kernel, which is applied to the pixel which was not white
		*/
					//label++;
					//PreProcessing.Instance.Grassfire(image, i,j, label);
					
					
					/*
		- If none of the neighbours has a tag, we create a new one by incrementing tagCheck
		- And we then tag our non-white pixel with the new tag
		*/
					/*if (tags [i, j] == 0) {

						tags [i, j] = tagCheck;
						tagCheck++;
					}*/

			}
		}

        /*
		- We set an array of CubeCorners to the size of TagCheck, which is the total amount of objects
		- We set the values of all corners to their theoretical maximum value, either 0 or the size of the input image
		*/

        /*cubecorners = new CubeCorner[tagCheck];

		for (int k = 0; k < tagCheck; k++) {
			cubecorners[k] = new CubeCorner();
		}

		for (int k = 0; k < tagCheck; k++) {

			cubecorners [k].xmin = texture.width;
			cubecorners [k].xmax = 0;
			cubecorners [k].ymin = texture.height;
			cubecorners [k].ymax = 0;

		}
		*/

        /*
		- Loop through all the pixels
		- Loop through the cube-objects
		*/

        /*for (int i = 1; i < texture.width; i++) {
			for (int j = 1; j < texture.height; j++) {
				for (int k = 1; k < cubecorners.Length; k++) {
					

		- I ask - If there is something tagged as k (0 -> amount of objects), 
		- then set the corresponding cubecorner (k) values:
		- if the i or j is lower or higher than the min or max of the object

		- This will create values for all four corner of a a rectangle of any detected object
		- Ready to be created in to a cube later on
		*/
        /*
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
		}*/

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
        /*for (int k = 1; k < cubecorners.Length; k++) {
			print ("Cube " + k + " xmin value is: " + cubecorners [k].xmin);
			print ("Cube " + k + " xmax value is: " + cubecorners [k].xmax);
			print ("Cube " + k + " ymin value is: " + cubecorners [k].ymin);
			print ("Cube " + k + " ymax value is: " + cubecorners [k].ymax);

			//starting to create cubes
		}*/


        int counter = 0;
		int counter2 = 0;

		int xMiddle = 0;
		int yMiddle = 0;

		/*for (int h = 1; h < texture.height; h++){
			for(int w = 1; w < texture.width; w++){
				
				
				
				if(image[w,h].r != 0f){
				
					xMiddle += w;
					yMiddle += h;

					counter2++;


					//start finding pixels
					//find middle
					
					
				}
			}
		}*/
		//xMiddle /= counter2;
		//yMiddle /= counter2;

        


        GameObject parent = GameObject.CreatePrimitive(PrimitiveType.Cube); // maybe not make cube but empty game object
        

		parent.AddComponent<Rigidbody>().useGravity = false;
		parent.GetComponent<Rigidbody>().isKinematic = true;
		parent.AddComponent<MeshCollider>();
		parent.AddComponent<MeshFilter>();
		parent.GetComponent<BoxCollider>().enabled = false;
        //parent.AddComponent<MeshCollider>();
        //parent.transform.position = new Vector3(xMiddle,0,yMiddle);



        for (int h = 1; h < texture.height; h++){
			for(int w = 1; w < texture.width; w++){

                

			if(image[w,h].r != 0f){
			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);

					cube.transform.position = new Vector3 (w, 0, h);
					cube.transform.localScale = new Vector3 (1, 1, 1);
                    //cube.AddComponent<Rigidbody> ().useGravity = false;
                    //cube.GetComponent<Rigidbody> ().isKinematic = true;
					//cube.AddComponent<MeshCollider>();

                    
                    
                    counter++;

                    cube.transform.parent = parent.transform;


                }
            }
		}


		parent.AddComponent<combineMesh>();
		parent.GetComponent<MeshCollider>().sharedMesh = parent.GetComponent<MeshFilter>().mesh;
        
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


        

		//transform.GetComponent<MeshFilter>().mesh = new Mesh();
		//transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combineMesh.Instance.combine);
		//transform.gameObject.SetActive(true);

        //parent.AddComponent<MeshCollider>();


        //print(counter);
        


        GameObject key = GameObject.CreatePrimitive (PrimitiveType.Cube);
		key.gameObject.tag = "Key";
		key.transform.position = new Vector3 (335, 1, 200);
		key.transform.localScale = new Vector3 (1, 1, 1);
		key.AddComponent<Rigidbody> ().useGravity = true;
	}
	
	
	// Update is called once per frame

	void Update ()
	{

		
		/*
 - This is a test, the test will print the following if and only if the image has 7 objects
*/
		/*if (tagCheck == 7) {
			print ("tagchch" + tagCheck);
		}*/
	}
}
/*
 *A CubeCorner Class, only used to store the four corners of a rectangle, to be come a cube 
 */


/*
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
*/
