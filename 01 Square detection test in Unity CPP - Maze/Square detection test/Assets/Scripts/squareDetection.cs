using UnityEngine;
using System.Collections;
using UnityEditor;

public class squareDetection : MonoBehaviour {


    /**
	 * - We're importing a texture (image), it's public so that we can input any picture.
	 * We create a new texture whihch is empty.
	 * We create an array of Colors, which holds the RGB values for each pixel
	 * We create a 2D array of integers that will hold the greyscale from Color
	 */
    public Texture2D inputTex;
    private Texture2D texture;

    TextureImporter importer;
    TextureImporterSettings importerSettings;

    Color[,] image;
    GameObject parent;
    bool hasBeenDone = false;

    GameObject[] parents;

    void Start() {

        /*
		 - We set the empty texture to the inputTex (this might create a copy by reference problem)
		 - We set the renderer of the gameobject this script is attached to, to the texture (input texture)
		 - We set the pix array of Colors to the texture of the texture (input texture)
		*/

        /*
         - This is not used really/anymore
         - We create a TextureImporter which looks at a specific picture for when chaning its settings.
         - The idea is to automatically change the settings of a picture but failed.
         - Sets the settings to "Advanced", EncodeRGB to "off", Read/Write enabled, disable mipMap, compression to RGBA32, FilterMode to Point, and remove N^2 Scale.
        */
        importer = AssetImporter.GetAtPath("Assets/Resources/16.png") as TextureImporter;
        importerSettings = new TextureImporterSettings();
        importerSettings.rgbm = TextureImporterRGBMMode.Off;
        importer.SetTextureSettings(importerSettings);
        importer.textureType = TextureImporterType.Advanced;
        importer.isReadable = true;
        importer.mipmapEnabled = false;
        importer.textureFormat = TextureImporterFormat.RGBA32;
        importer.filterMode = FilterMode.Point;
        importer.npotScale = TextureImporterNPOTScale.None;




        texture = new Texture2D(inputTex.width, inputTex.height, TextureFormat.RGBA32, false);

        Color[,] image = PreProcessing.Instance.GetPixels2D(inputTex);


        image = PreProcessing.Instance.threshGrey(image);
        image = PreProcessing.Instance.threshRGB(image);
        image = PreProcessing.Instance.RGBErodeBlack(image);

        image = PreProcessing.Instance.spawnDetection(image);

        image = PreProcessing.Instance.batteryDetection(image);

        image = PreProcessing.Instance.LaserBlobExtraction(image);
        image = PreProcessing.Instance.laserDetection(image);

        image = PreProcessing.Instance.goalDetection(image);

        PreProcessing.Instance.SetPixels2D(image, texture);

        this.GetComponent<Renderer>().material.mainTexture = texture;

        /*
		- Both of our 2D arrays of integers take the height and width of the image pixels
		- The array of tags gets 10 pixels worth of padding, otherwise it will go out of bounds later
		*/


        int pixIndexCounter = 0;
        int[] pixIndexX;
        int[] pixIndexY;


        for (int w = 1; w < texture.width; w++) {
            for (int h = 1; h < texture.height; h++) {
                if (image[w, h].r == 0f) {
                    pixIndexCounter++;
                }
            }
        }

        pixIndexX = new int[pixIndexCounter];
        pixIndexY = new int[pixIndexCounter];

        int counter3 = 0;
        for (int w = 1; w < texture.width; w++) {
            for (int h = 1; h < texture.height; h++) {

                if (image[w, h].r == 0f) {
                    pixIndexY[counter3] = h;
                    pixIndexX[counter3] = w;
                    counter3++;
                }
            }
        }
        /*
         - We create empty GameObjects based on how many cubes there are divided by 1000. 
         - Then we create 1 more assigned for the last cubes that did not come in 1000
         - These are the parents of the cubes.
        */
        parents = new GameObject[(pixIndexCounter / 1000) + 1];



        for (int p = 0; p < parents.Length; p++) {
            parents[p] = (GameObject)Instantiate(Resources.Load("walls"));

            parents[p].name = "Parent";
        }

        int parentCounter = 0;
        int countTo1000 = 0;

        for (int h = 0; h < pixIndexY.Length; h++) {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(pixIndexX[h], 12.4f, pixIndexY[h]);
            cube.transform.localScale = new Vector3(1, 25, 1);

            cube.transform.parent = parents[parentCounter].transform;

            countTo1000++;

            if (countTo1000 == 1000) {
                parentCounter++;
                countTo1000 = 0;
            }
        }

        /*
         - 
        */
        for (int p = 0; p < parents.Length; p++) {
            parents[p].GetComponent<MeshFilter>().mesh = parents[p].GetComponent<Mesh>();

            parents[p].AddComponent<combineMesh>();
        }


        /*
         - Destroys all the "children" meaning that we delete all individual cubes after they all have went through MeshCombine()
        */
        for (int p = 0; p < parents.Length; p++) {
            foreach (Transform child in parents[p].transform) {
                GameObject.Destroy(child.gameObject);
            }
        }
    }


    // Update is called once per frame

    void Update() {
        /**
		 * The only thing we do in Update so far, is to give all parents a new MeshCollider,
		 * since it didnt work any other way, this is a hot fix.
		 */

        if (hasBeenDone == false) {
            for (int p = 0; p < parents.Length; p++)
                parents[p].AddComponent<MeshCollider>();

            hasBeenDone = true;
        }
    }
}