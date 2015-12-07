using UnityEngine;
using System.Collections;
using System.Linq;

public class PreProcessing : Singleton<PreProcessing>
{


    GameObject goal;
    int teleportMeanX;
    int teleportMeanZ;

	int globalLaserLabel;

    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }

    public float[] BubbleSort(float[] input)
    {
        for (int j = 0; j < input.Length; j++)
        {
            for (int i = 0; i < input.Length - 1 - j; i++)
            {
                float tmp;
                if (input[i] < input[i + 1])
                {
                    tmp = input[i];
                    input[i] = input[i + 1];
                    input[i + 1] = tmp;
                }
            }

        }

        return input;
    }

    public int[] BubbleSort(int[] input)
    {
        for (int j = 0; j < input.Length; j++)
        {
            for (int i = 0; i < input.Length - 1 - j; i++)
            {
                int tmp;
                if (input[i] < input[i + 1])
                {
                    tmp = input[i];
                    input[i] = input[i + 1];
                    input[i + 1] = tmp;
                }
            }

        }

        return input;
    }

    public Color[,] Threshold(Color[,] i, float t)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                i[w, h].r = i[w, h].r < t ? 0f : 1f;
                i[w, h].g = i[w, h].g < t ? 0f : 1f;
                i[w, h].b = i[w, h].b < t ? 0f : 1f;
            }
        }
        return i;
    }

    public Color[,] Rank(Color[,] i, int k, int t)
    {

        Color[,] tmp = i;

        int kw = k / 2;
        int kh = k / 2;

        //Double for loop through the pixels of the input image

        for (int w = kw; w < i.GetLength(0) - kw; w++)
        {
            for (int h = kh; h < i.GetLength(1) - kh; h++)
            {

                // Integer to be interated

                int count = 0;

                float[] neighborhood = new float[k * k];

                for (int j = -kw; j <= kw; j++)
                {
                    for (int l = -kh; l <= kh; l++)
                    {

                        neighborhood[count] = (tmp[w + j, h + l].r + tmp[w + j, h + l].g + tmp[w + j, h + l].b) / 3;
                        count++;
                    }
                }

                float result = 0f;

                switch (t)
                {
                    case 1: //median
                        neighborhood = BubbleSort(neighborhood);
                        result = neighborhood[neighborhood.Length / 2];
                        break;
                    case 2: // difference
                        result = neighborhood.Max() - neighborhood.Min();
                        break;
                    case 3:
                        neighborhood = BubbleSort(neighborhood);
                        if (neighborhood[neighborhood.Length] >= 1)
                        {
                            result = 1;

                        }
                        else
                        {
                            result = 0;
                        }




                        break;
                    default:
                        result = 0.5f;
                        break;
                }



                i[w, h].r = result;
                i[w, h].g = result;
                i[w, h].b = result;
            }
        }
        return i;
    }

    public Color[,] Rgb2greyScale(Color[,] i)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                float grey = (i[w, h].r + i[w, h].g + i[w, h].b) / 3;
                i[w, h].r = grey;
                i[w, h].g = grey;
                i[w, h].b = grey;
            }
        }
        return i;
    }

    public Color[,] simpleBrightness(Color[,] i, int b)
    {
        float c = b / 255f;
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {

                i[w, h].r += c;
                i[w, h].g += c;
                i[w, h].b += c;

            }
        }
        return i;
    }
    public Color[,] simpleContrast(Color[,] i, float b)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {

                i[w, h].r *= b;
                i[w, h].g *= b;
                i[w, h].b *= b;

            }
        }
        return i;
    }

    public void printSample(Color[,] i, int x, int y, int huh)
    {


        for (int w = x; w < x + huh; w++)
        {
            for (int h = y; h < y + huh; h++)
            {

                print(i[w, h]);

            }
        }


    }
	public Color[,] RGBErode2White(Color[,] i)
	{
		
		Color[,] tmp = i;
		Color[,] tmp2 = tmp;
		
		
		for (int w = 3; w < i.GetLength(0) - 3; w++)
		{
			for (int h = 3; h < i.GetLength(1) - 3; h++)
			{
				
				int count = 0;
				
				for (int j = -3; j <= 3; j++)
				{
					for (int l = -3; l <= 3; l++)
					{
						
						if(tmp[w + j, h + l] == Color.blue){
							count++;
						}
						
					}
				}
				
				if (count >= 4)
				{
					
					tmp2[w,h] = Color.white;
					i[w, h - 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w, h + 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w - 1 , h] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1 , h] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w -1 , h + 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1, h + 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w - 1 , h -1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1 , h -1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					
				}
			}
		}
		
		i = tmp2;
		
		return i;
	}
	public Color[,] RGBErodeBlack(Color[,] i)
	{
		
		Color[,] tmp = i;
		Color[,] tmp2 = tmp;
		
		
		for (int w = 3; w < i.GetLength(0) - 3; w++)
		{
			for (int h = 3; h < i.GetLength(1) - 3; h++)
			{
				
				int count = 0;
				
				for (int j = -3; j <= 3; j++)
				{
					for (int l = -3; l <= 3; l++)
					{
						
						if(tmp[w,h] != Color.black && tmp[w + j, h + l] == Color.black){
							tmp[w,h] = Color.white;
						}
						if(tmp[w,h] != Color.green && tmp[w + j, h + l] == Color.green){
							tmp[w,h] = Color.white;
						}
						if(tmp[w,h] != Color.red && tmp[w + j, h + l] == Color.red){
							tmp[w,h] = Color.white;
						}
					}
				}
			}
		}
		
		i = tmp2;
		
		return i;
	}
	public Color[,] RGBErodeRed(Color[,] i)
	{
		
		Color[,] tmp = i;
		Color[,] tmp2 = tmp;
		
		
		for (int w = 3; w < i.GetLength(0) - 3; w++)
		{
			for (int h = 3; h < i.GetLength(1) - 3; h++)
			{
				
				int count = 0;
				
				for (int j = -3; j <= 3; j++)
				{
					for (int l = -3; l <= 3; l++)
					{

						if(tmp[w,h] != Color.red && tmp[w + j, h + l] == Color.red){
							tmp[w,h] = Color.white;
						}
						
					}
				}
			}
		}
		
		i = tmp2;
		
		return i;
	}
	public Color[,] RGBErodeGreen(Color[,] i)
	{
		
		Color[,] tmp = i;
		Color[,] tmp2 = tmp;
		
		
		for (int w = 3; w < i.GetLength(0) - 3; w++)
		{
			for (int h = 3; h < i.GetLength(1) - 3; h++)
			{
				
				int count = 0;
				
				for (int j = -3; j <= 3; j++)
				{
					for (int l = -3; l <= 3; l++)
					{

						if(tmp[w,h] != Color.green && tmp[w + j, h + l] == Color.green){
							tmp[w,h] = Color.white;
						}
						
					}
				}
			}
		}
		
		i = tmp2;
		
		return i;
	}

	public Color[,] RGBErode2Blue(Color[,] i)
	{
		
		Color[,] tmp = i;
		Color[,] tmp2 = tmp;
		
		
		for (int w = 3; w < i.GetLength(0) - 3; w++)
		{
			for (int h = 3; h < i.GetLength(1) - 3; h++)
			{
				
				int count = 0;
				
				for (int j = -3; j <= 3; j++)
				{
					for (int l = -3; l <= 3; l++)
					{
						
						if(tmp[w + j, h + l] == Color.white){
							count++;
						}
						
					}
				}

				if (count >= 4)
				{

					tmp2[w,h] = Color.blue;
					i[w, h - 1] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w, h + 1] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w - 1 , h] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1 , h] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w -1 , h + 1] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1, h + 1] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w - 1 , h -1] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1 , h -1] = Color.blue; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					
				}
			}
		}
		
		i = tmp2;
		
		return i;
	}
	
	public Color[,] GreyScaleErode(Color[,] i)
	{
		
		Color[,] tmp = i;
		
		Color[,] result = new Color[i.GetLength(0), i.GetLength(1)];
		
		
		for (int w = 3; w < i.GetLength(0) - 3; w++)
		{
			for (int h = 3; h < i.GetLength(1) - 3; h++)
			{
				
				int count = 0;
				float sum = 0;
				
				for (int j = -3; j <= 3; j++)
                {
                    for (int l = -3; l <= 3; l++)
                    {

                        sum += (tmp[w + j, h + l].r);

                        count++;

                    }
                }




                if (sum <= 0f)
                {
                    result[w, h].r = 0f;
                    result[w, h].g = 0f;
                    result[w, h].b = 0f;
                }
                else
                {
                    result[w, h].r = 1f;
                    result[w, h].g = 1f;
                    result[w, h].b = 1f;
                }


            }
        }



        return result;
    }

    public Color[,] Dilate(Color[,] i)
    {

        Color[,] tmp = i;

        Color[,] result = new Color[i.GetLength(0), i.GetLength(1)];


        for (int w = 3; w < i.GetLength(0) - 3; w++)
        {
            for (int h = 3; h < i.GetLength(1) - 3; h++)
            {

                int count = 0;
                float sum = 0;


                for (int j = -3; j <= 3; j++)
                {
                    for (int l = -3; l <= 3; l++)
                    {

                        sum += (tmp[w + j, h + l].r);

                        count++;

                    }
                }




                if (sum < 49f)
                {
                    result[w, h].r = 0f;
                    result[w, h].g = 0f;
                    result[w, h].b = 0f;
                }
                else
                {
                    result[w, h].r = 1f;
                    result[w, h].g = 1f;
                    result[w, h].b = 1f;
                }


            }
        }



        return result;
    }

    public Color[,] GetPixels2D(Texture2D i)
    {

        Color[,] texture2D = new Color[i.width, i.height];
        Color[] texture1D = i.GetPixels();

        for (int w = 0; w < i.width; w++)
        {
            for (int h = 0; h < i.height; h++)
            {
                texture2D[w, h] = texture1D[h * i.width + w];
            }
        }
        return texture2D;
    }

    public void SetPixels2D(Color[,] i, Texture2D t)
    {
        Color[] texture1D = new Color[i.Length];

        int width = i.GetLength(0);
        int height = i.GetLength(1);

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                texture1D[h * width + w] = i[w, h];
            }
        }

        t.SetPixels(texture1D);
        t.Apply();
    }

    public Color[,] BlobExtraction(Color[,] i)
    {
        int label = 0;

        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                if (i[w, h].r == 1f)
                {
                    label++;
                    Grassfire(i, w, h, label * 10); //change number in case there is a lot of blobs
                }
            }
        }
        return i;
    }
    public void Grassfire(Color[,] i, int x, int y, int label)
    {
        int width = i.GetLength(0);
        int height = i.GetLength(1);

        i[x, y] = new Color(label / 255f + 0.2f, 0, 0);

        if (x + 1 < height && i[x + 1, y].r == 1f) //Changed height from width
        {
            Grassfire(i, x + 1, y, label);
        }
        if (x - 1 > 0 && i[x - 1, y].r == 1f)
        {
            Grassfire(i, x - 1, y, label);
        }
        if (y + 1 < width && i[x, y + 1].r == 1f) //Changed width from height
        {
            Grassfire(i, x, y + 1, label);
        }
        if (y - 1 > 0 && i[x, y - 1].r == 1f)
        {
            Grassfire(i, x, y - 1, label);
        }
    }
    public Color[,] Invert(Color[,] i)
    {
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                i[w, h].r = 1 - i[w, h].r;
                i[w, h].g = 1 - i[w, h].g;
                i[w, h].b = 1 - i[w, h].b;
            }
        }
        return i;
    }

    public Color[,] spawnDetection(Color[,] i)
    {
        int xValues = 0;
        int yValues = 0;
        int totalCounter = 0;
        int meanX = 0;
        int meanZ = 0;

        Color[,] temp = i;

        //temp = NormalizedRgb(temp);

        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {

                if (temp[w,h] == Color.green) // FOR GREEN
                {
                                      
                    xValues += w;
                    yValues += h;
                    totalCounter++;

                    i[w, h] = Color.white;
                   // i[w, h-1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson

                }
            }

        }
     
        meanX = xValues / totalCounter;
        meanZ = yValues / totalCounter;

        //Creation of the player
        GameObject player = (GameObject)Instantiate(Resources.Load("RobotV10"));
        player.transform.position = new Vector3(meanX, 5, meanZ);
        player.transform.localScale = new Vector3(7,7,7);

        //Creation of the teleportStarter
        GameObject teleportStarter = (GameObject)Instantiate(Resources.Load("teleportStarter"));
        teleportStarter.transform.position = new Vector3(meanX, -1.7f, meanZ);
        teleportStarter.transform.localScale = new Vector3(80, 80, 80);
        

        return i;
    }
    public Color[,] goalDetection(Color[,] i)
    {

        int xValues = 0;
        int yValues = 0;
        int totalCounter = 0;
        //int meanX = 0;
        //int meanZ = 0;
        int RGDIF = 0;

        Color[,] temp = i;


        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                RGDIF = (int) Mathf.Abs((temp[w, h].r *255) - (temp[w, h].g * 255));

                if (/*temp[w,h] == Color.yellow*/  temp[w, h].r > 0.55f && temp[w, h].g > 0.55f && temp[w, h].b < 0.35f && RGDIF < 55) // FOR YELLOW
                {

                    xValues += w;
                    yValues += h;
                    totalCounter++;

                    i[w, h] = Color.white;
				}
            }

        }

        teleportMeanX = xValues / totalCounter;
        teleportMeanZ = yValues / totalCounter;

        

        //Creation of the goal
        goal = (GameObject)Instantiate(Resources.Load("teleportGoalNoBattery"));
        goal.transform.position = new Vector3(teleportMeanX, -1.7f, teleportMeanZ);
        goal.transform.localScale = new Vector3(80, 80, 80);

        return i;
    }



    public Color[,] NormalizedRgb(Color[,] i)
    {

        float allColors;


        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
                allColors = i[w, h].r + i[w, h].g + i[w, h].b;

                if (allColors == 0)
                    i[w, h] = Color.black;

                i[w, h].r = (i[w, h].r) / allColors;
                i[w, h].g = (i[w, h].g) / allColors;
                i[w, h].b = (i[w, h].b) / allColors;
            }
        }
        return i;
    }
	public Color[,] batteryDetection(Color[,] i)
	{
		int xValues = 0;
		int yValues = 0;
		int totalCounter = 0;
		int meanX = 0;
		int meanZ = 0;
		
		Color[,] temp = i;
		
		//temp = NormalizedRgb(temp);
		
		for (int w = 0; w < i.GetLength(0); w++)
		{
			for (int h = 0; h < i.GetLength(1); h++)
			{
				
				if (temp[w, h].r < 0.6f&& temp[w, h].g < 0.5f && temp[w, h].g > 0.3f && temp[w, h].b > 0.3f && temp[w, h].b < 0.65f) // FOR BLUE
				{
					
					xValues += w;
					yValues += h;
					totalCounter++;
					
					i[w, h] = Color.white;
					i[w, h - 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w, h + 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w - 1 , h] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1 , h] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w -1 , h + 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1, h + 1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w - 1 , h -1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson
					i[w + 1 , h -1] = Color.white; // "I totally have a clue why this works" - Nils Emil Åberg Karlsson

				}
			}
			
		}
		
		meanX = xValues / (totalCounter);
		meanZ = yValues / (totalCounter);

		/**
		 * This is a little cube that we call a "Key".
		 * It can be picked up by a player object
		 * See "OnCollision" in player script.
		 */
		
		GameObject battery = (GameObject)Instantiate(Resources.Load("Battery"));
		battery.name = "Battery";
		battery.gameObject.tag = "Battery";
		battery.transform.position = new Vector3(meanX, 1, meanZ);
		battery.transform.localScale = new Vector3(10, 10, 10);
		battery.AddComponent<Rigidbody>().useGravity = false;
		battery.AddComponent<CapsuleCollider>();
		battery.GetComponent<CapsuleCollider>().height = 0.1f;
		battery.GetComponent<CapsuleCollider>().radius = 0.03f;
		battery.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.05f, 0);
		
		
		return i;
	}

    public Color[,] laserDetection(Color[,] i){



		int Blobcounter = 0;
		
		int [ ] totalpixels = new int [globalLaserLabel];
		int [ ] meanX = new int [globalLaserLabel];
		int [ ] meanY = new int [globalLaserLabel];

		for (int j = 0; j < globalLaserLabel; j++){

			totalpixels[j] = 0;
			meanX[j] = 0;
			meanY[j] = 0;
		}
		
		int [ ] xMax = new int [globalLaserLabel];
		int [ ] yMax = new int [globalLaserLabel];
		int [ ] xMin = new int [globalLaserLabel];
		int [ ] yMin = new int [globalLaserLabel];

		int [] scale = new int[globalLaserLabel]; 

		for (int j = 0; j < globalLaserLabel; j++){
			
			xMin [j] = i.GetLength(0);
			yMin [j] = i.GetLength(1);
		}

		int [ ] xDif = new int[globalLaserLabel];
		int [ ] yDif = new int[globalLaserLabel];

	for(int j = 1; j <= globalLaserLabel; j++){
        for (int w = 0; w < i.GetLength(0); w++)
        {
            for (int h = 0; h < i.GetLength(1); h++)
            {
					if (i[w, h].r <= j*10 / 255f + 0.22f && i[w, h].r >= j*10 / 255f + 0.18f && i[w, h].g == 0f && i[w, h].b == 0f) {

						print ("SOMETHING HAPPENED HERE 1");

						totalpixels[Blobcounter]++;
							
						meanX[Blobcounter] += w;
						meanY[Blobcounter] += h;

						if(w > xMax[Blobcounter])
							xMax[Blobcounter] = w;
								
						if(h > yMax[Blobcounter])
							yMax[Blobcounter] = h;
										
						if(w < xMin[Blobcounter])
							xMin[Blobcounter] = w;
												
						if(h < yMin[Blobcounter])
							yMin[Blobcounter] = h;

						i[w,h] = Color.white;
						//i[w,h-1] = Color.white;
						//i[w,h+1] = Color.white;
                }
            }
        }
			Blobcounter++;
	}
		for ( int j = 0; j < globalLaserLabel; j++){

			meanX[j] /= totalpixels[j];
			meanY[j] /= totalpixels[j];

			xDif[j] = xMax[j] - xMin[j];
			yDif[j] = yMax[j] - yMin[j];

			if((xDif[j] * yDif[j]) < totalpixels[j] +10 && (xDif[j] * yDif[j]) > totalpixels[j] -10 && xDif[j] < yDif[j] + 10 && xDif[j] > yDif[j] - 10 ){
				
				scale[j] = xDif[j];
				
			}
			
			else if ((xDif[j]*yDif[j])/3 > totalpixels[j] && xDif[j] < yDif[j] + 10 && xDif[j] > yDif[j] - 10  ){
				
				scale[j] = (int) Mathf.Sqrt( xDif[j] * xDif[j] + yDif[j] * yDif[j]);
				
			}
			
			else if(xDif[j] > yDif[j]){
				
				scale[j] = xDif[j];
				
			}
			else if(yDif[j] > xDif[j]){
				
				scale[j] = yDif[j];
				
			}

			GameObject laser = (GameObject)Instantiate(Resources.Load("Laser"));
			laser.transform.position = new Vector3(meanX[j], 1, meanY[j]);
			laser.transform.localScale = new Vector3(0.1f, scale[j]/3, 0.1f);


			//laser.transform.RotateAround(new Vector3(meanX[j], 1, meanY[j]), new Vector3(0,1,0), 20 * Time.deltaTime);

		}



                return i;
    }

    public GameObject getGoal()
    {
        return goal;
    }

    public int getTeleportMeanX(){
        return teleportMeanX;
    }

    public int getTeleportMeanZ()
    {
        return teleportMeanZ;
    }
	public Color[,] LaserBlobExtraction(Color[,] i)
	{
		int label = 0;
		
		for (int w = 0; w < i.GetLength(0); w++)
		{
			for (int h = 0; h < i.GetLength(1); h++)
			{
				if (i[w, h].r > 0.6f && i[w,h].g < 0.4f  &&i[w,h].b < 0.4f )
				{


					label++;
					globalLaserLabel++;

					LaserGrassfire(i, w, h, label); //change number in case there is a lot of blobs


				}
			}
		}


		return i;
	}
	public void LaserGrassfire(Color[,] i, int x, int y, int label)
	{
		int width = i.GetLength(0);
		int height = i.GetLength(1);

		print ("label within grassfire "+label);

		i[x, y] = new Color(label*10 / 255f + 0.2f, 0, 0);
		
		if (x + 1 < height && i[x + 1, y].r > 0.6f && i[x + 1, y].g < 0.4f  && i[x + 1, y].b < 0.4f ) //Changed height from width
		{
			LaserGrassfire(i, x + 1, y, label);
		}
		if (x - 1 > 0 && i[x - 1, y].r > 0.6f && i[x - 1, y].g < 0.4f  && i[x - 1, y].b < 0.4f)
		{
			LaserGrassfire(i, x - 1, y, label);
		}
		if (y + 1 < width && i[x, y + 1].r > 0.6f && i[x , y + 1].g < 0.4f  && i[x, y + 1].b < 0.4f) //Changed width from height
		{
			LaserGrassfire(i, x, y + 1, label);
		}
		if (y - 1 > 0 && i[x, y - 1].r  > 0.6f && i[x , y - 1].g < 0.4f  && i[x, y - 1].b < 0.4f)
		{
			LaserGrassfire(i, x, y - 1, label);
		}
	}

	public Color[,] threshGrey(Color [,] i){

		for(int w = 0; w < i.GetLength(0); w++){
			for(int h = 0; h <i.GetLength(1); h++){
			
				if(i[w,h].r < (i[w,h].g + 25/255f) && i[w,h].r > (i[w,h].g - 25/255f) && i[w,h].g < (i[w,h].b + 25/255f) && i[w,h].g > (i[w,h].b - 25/255f) && 
				                                                                                                                        i[w,h].b < (i[w,h].r + 25/255f) && i[w,h].b > (i[w,h].r - 25/255f) && i[w,h].r > 65/255f){
					i[w,h] = Color.white;
				}

			}
		}

				return i;
	}
	public Color[,] threshRGB(Color [,] i){
		
		for(int w = 0; w < i.GetLength(0); w++){
			for(int h = 0; h <i.GetLength(1); h++){

				if(i[w,h].r >= 0.4f){
					i[w,h].r = 1f;
				}else if(i[w,h].r <= 0.4f){
					i[w,h].r = 0f;
				}
				if(i[w,h].g >= 0.4f){
					i[w,h].g = 1f;
				}else if(i[w,h].g <= 0.4f){
					i[w,h].g = 0f;
				}
				if(i[w,h].b >= 0.4f){
					i[w,h].b = 1f;
				}else if(i[w,h].b <= 0.4f){
					i[w,h].b = 0f;
				}else{
					i[w,h] = Color.white;
				}
				
			}
		}
		
		return i;
	}
}
