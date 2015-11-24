using UnityEngine;
using System.Collections;


public class squareDetection : MonoBehaviour
{


    /**
	 * - We're importing a texture (image), its public so that we can input any picture.
	 * We create a new texture whihch is empty.
	 * We create an array of Colors, which holds the RGB values for each pixel
	 * We create a 2D array of integers that will hold the greyscale from Color
	 */
    public Texture2D inputTex;
    private Texture2D texture;
    Color[,] image;
    GameObject parent;
    bool hasBeenDone = false;

    GameObject[] parents;

    void Start()
    {

        /*
		 - We set the empty texture to the inputTex (this might create a copy by reference problem)
		 - We set the renderer of the gameobject this script is attached to, to the texture (input texture)
		 - We set the pix array of Colors to the texture of the texture (input texture)
		*/



        texture = new Texture2D(inputTex.width, inputTex.height);

        Color[,] image = PreProcessing.Instance.GetPixels2D(inputTex);

        image = PreProcessing.Instance.spawnDetection(image);
        image = PreProcessing.Instance.goalDetection(image);

		image = PreProcessing.Instance.LaserBlobExtraction(image);
		//image = PreProcessing.Instance.laserDetection(image);

        image = PreProcessing.Instance.Rgb2greyScale(image);
   
        image = PreProcessing.Instance.simpleBrightness(image, -30);
   
        image = PreProcessing.Instance.simpleContrast(image, 1.3f);

        //image = PreProcessing.Instance.Rank (image, 3, 1);

        image = PreProcessing.Instance.Threshold(image, 0.8f);

        //image = PreProcessing.Instance.Dilate (image);

        //image = PreProcessing.Instance.Erode(image);

        image = PreProcessing.Instance.Invert(image);

        //image = PreProcessing.Instance.BlobExtraction(image);

        //PreProcessing.Instance.printSample (image, 300,50,10);

        //image = PreProcessing.Instance.Dilate (image);
        //image = PreProcessing.Instance.Dilate (image);
        //image = PreProcessing.Instance.Dilate (image);

        PreProcessing.Instance.SetPixels2D(image, texture);

        this.GetComponent<Renderer>().material.mainTexture = texture;

        /*
		- Both of our 2D arrays of integers take the height and width of the image pixels
		- The array of tags gets 10 pixels worth of padding, otherwise it will go out of bounds later
		*/


        int pixIndexCounter = 0;
        int[] pixIndexX;
        int[] pixIndexY;


        for (int w = 1; w < texture.width; w++)
        {
            for (int h = 1; h < texture.height; h++)
            {
                if (image[w, h].r != 0f)
                {
                    pixIndexCounter++;
                }
            }
        }

        pixIndexX = new int[pixIndexCounter];
        pixIndexY = new int[pixIndexCounter];

        int counter3 = 0;
        for (int w = 1; w < texture.width; w++)
        {
            for (int h = 1; h < texture.height; h++)
            {

                if (image[w, h].r != 0f)
                {
                    pixIndexY[counter3] = h;
                    pixIndexX[counter3] = w;
                    counter3++;
                }
            }
        }

        parents = new GameObject[(pixIndexCounter / 1000) + 1];



        for (int p = 0; p < parents.Length; p++)
        {
            parents[p] = (GameObject)Instantiate(Resources.Load("walls"));
            //parents[p].AddComponent<MeshRenderer>();
            //parents[p].GetComponent<Renderer>().material.color = new Color(255,255,255);
            parents[p].name = "Parent";
            //parents[p].AddComponent<MeshFilter>();





            //parents[p].AddComponent<MeshCollider>();
            //parents[p].GetComponent<MeshRenderer>().receiveShadows = false;
            //parents[p].GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //parents[p].GetComponent<MeshRenderer>().reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            //parents[p].GetComponent<Renderer>().material.color = new Color(0.5f,0.5f,0.5f,1);
            //Texture2D tex = Resources.Load("wallTex") as Texture2D;
            //parents[p].GetComponent<Renderer>().material.mainTexture = Resources.Load("wallTex") as Texture2D;

            /*
            Texture2D text = (Texture2D) Resources.Load("wallTex.png");
            //text = Instantiate(parents[p].GetComponent<Renderer>().material.mainTexture); //clone the material 
            parents[p].GetComponent<Renderer>().material.mainTexture = text; //set the material equal to the clone
            */



        }
        int parentCounter = 0;
        int countoTo1000 = 0;

        for (int h = 0; h < pixIndexY.Length; h++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(pixIndexX[h], 12.4f, pixIndexY[h]);
            cube.transform.localScale = new Vector3(1, 25, 1);

            //cube.AddComponent<Rigidbody> ().useGravity = false;
            //cube.GetComponent<Rigidbody> ().isKinematic = true;
            //cube.AddComponent<MeshCollider>();

            cube.transform.parent = parents[parentCounter].transform;

            countoTo1000++;

            if (countoTo1000 == 1000)
            {
                parentCounter++;
                countoTo1000 = 0;


            }

        }


        for (int p = 0; p < parents.Length; p++)
        {
            parents[p].GetComponent<MeshFilter>().mesh = parents[p].GetComponent<Mesh>();

            parents[p].AddComponent<combineMesh>();
            //parents[p].GetComponent<MeshCollider>().sharedMesh = parents[p].GetComponent<MeshFilter>().mesh;
        }



        //parent.GetComponent<MeshFilter>().mesh

        for (int p = 0; p < parents.Length; p++)
        {
            foreach (Transform child in parents[p].transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        /**
		 * This is a little cube that we call a "Key".
		 * It can be picked up by a player object
		 * See "OnCollision" in player script.
		 */





        GameObject battery = (GameObject)Instantiate(Resources.Load("Battery"));
        battery.name = "Battery";
        battery.gameObject.tag = "Battery";
        battery.transform.position = new Vector3(335, 1, 200);
        battery.transform.localScale = new Vector3(10, 10, 10);
        battery.AddComponent<Rigidbody>().useGravity = false;
        battery.AddComponent<CapsuleCollider>();
        battery.GetComponent<CapsuleCollider>().height = 0.1f;
        battery.GetComponent<CapsuleCollider>().radius = 0.03f;
        battery.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.05f, 0);

    }


    // Update is called once per frame

    void Update()
    {
        /**
		 * The only thing we do in Update so far, is to give all parents a new MeshCollider,
		 * since it didnt work any other way, this is a hot fix.
		 */

        if (hasBeenDone == false)
        {
            for (int p = 0; p < parents.Length; p++)
                parents[p].AddComponent<MeshCollider>();

            hasBeenDone = true;
        }
    }
}