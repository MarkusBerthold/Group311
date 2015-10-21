using UnityEngine;
using System.Collections;
using System.Linq;

public class PreProcessing : Singleton<PreProcessing> {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float[] BubbleSort(float[] input) {
		for (int j = 0; j < input.Length; j++) {
			for (int i = 0; i < input.Length-1-j; i++) {
				float tmp;
				if (input [i] < input [i + 1]) {
					tmp = input [i];
					input [i] = input [i + 1];
					input [i + 1] = tmp;
				}
			}
			
		}
		
		return input;
	}

	public int[] BubbleSort(int[] input) {
		for (int j = 0; j < input.Length; j++) {
			for (int i = 0; i < input.Length-1-j; i++) {
				int tmp;
				if (input [i] < input [i + 1]) {
					tmp = input [i];
					input [i] = input [i + 1];
					input [i + 1] = tmp;
				}
			}
			
		}
		
		return input;
	}
	
	public Color[,] Threshold (Color[,] i, float t)
	{
		for (int w = 0; w < i.GetLength(0); w++) {
			for (int h = 0; h < i.GetLength(1); h++) {
				i [w, h].r = i [w, h].r < t ? 0f : 1f;
				i [w, h].g = i [w, h].g < t ? 0f : 1f;
				i [w, h].b = i [w, h].b < t ? 0f : 1f;
			}
		}
		return i;
	}

	public Color[,] Rank (Color[,] i, int k, int t)
	{
		
		Color[,] tmp = i;
		
		int kw = k / 2;
		int kh = k / 2;

		//Double for loop through the pixels of the input image
		
		for (int w = kw; w < i.GetLength(0) - kw; w++) {
			for (int h = kh; h < i.GetLength(1) - kh; h++) {

				// Integer to be interated

				int count = 0;
				
				float [] neighborhood = new float[k * k];
				
				for (int j = -kw; j <= kw; j++) {
					for (int l = -kh; l <= kh; l++) {
						
						neighborhood [count] = (tmp [w + j, h + l].r + tmp [w + j, h + l].g + tmp [w + j, h + l].b) / 3;
						count ++;
					}
				}
				
				float result = 0f;
				
				switch (t) {
				case 1: //median
					neighborhood = BubbleSort (neighborhood);
					result = neighborhood [neighborhood.Length / 2];
					break;
				case 2: // difference
					result = neighborhood.Max () - neighborhood.Min ();
					break;
				case 3:
					neighborhood = BubbleSort (neighborhood);
					if (neighborhood[neighborhood.Length] >= 1) {
						result = 1;

					} else {
						result = 0;
					}




					break;
				default:
					result = 0.5f;
					break;
				}
				
				
				
				i [w, h].r = result;
				i [w, h].g = result;
				i [w, h].b = result;
			}
		}
		return i;
	}

	public Color[,] Rgb2greyScale (Color[,] i){
		for (int w = 0; w < i.GetLength(0); w++) {
			for (int h = 0; h < i.GetLength(1); h++) {
				float grey = (i [w, h].r + i [w, h].g + i [w, h].b) / 3;
				i [w, h].r = grey;
				i [w, h].g = grey;
				i [w, h].b = grey;
			}
		}
		return i;
	}

	public Color [,] simpleBrightness(Color [,] i, int b){
		float c = b / 255f;
		for(int w = 0; w < i.GetLength(0); w++){
			for(int h = 0; h < i.GetLength(1); h++){
				
				i[w,h].r += c;
				i[w,h].g += c;
				i[w,h].b += c;
				
			}
		}
		return i;
	}
	public Color [,] simpleContrast(Color [,] i, float b){
		for(int w = 0; w < i.GetLength(0); w++){
			for(int h = 0; h < i.GetLength(1); h++){
				
				i[w,h].r *= b;
				i[w,h].g *= b;
				i[w,h].b *= b;
				
			}
		}
		return i;
	}

	public void printSample (Color [,] i, int x, int y, int huh){


		for (int w = x; w < x+huh; w++) {
			for (int h = y; h < y+huh; h++) {

				print (i[w,h]);

			}
		}


	}

	public Color[,] Erode (Color[,] i) {

		Color[,] tmp = i;

		Color[,] result = new Color[i.GetLength(0), i.GetLength(1)];


		for (int w = 3; w < i.GetLength(0) - 3; w++) {
			for (int h = 3; h < i.GetLength(1) - 3; h++) {
				
				int count = 0;
				float sum = 0;
				
				int [] neighborhood = new int[49];
				
				for (int j = -3; j <= 3; j++) {
					for (int l = -3; l <= 3; l++) {

						sum += (tmp [w + j, h + l].r);

						count ++;

					}
				}
			


			
				if (sum <= 0f) {
					result[w,h].r = 0f;
					result[w,h].g = 0f;
					result[w,h].b = 0f;
				}else {
					result[w,h].r = 1f;
					result[w,h].g = 1f;
					result[w,h].b = 1f;
				}


			}
		}



		return result;
	}

	public Color[,] Dilate (Color[,] i) {
		
		Color[,] tmp = i;
		
		Color[,] result = new Color[i.GetLength(0), i.GetLength(1)];
		
		
		for (int w = 3; w < i.GetLength(0) - 3; w++) {
			for (int h = 3; h < i.GetLength(1) - 3; h++) {
				
				int count = 0;
				float sum = 0;
				
				int [] neighborhood = new int[49];
				
				for (int j = -3; j <= 3; j++) {
					for (int l = -3; l <= 3; l++) {
						
						sum += (tmp [w + j, h + l].r);
						
						count ++;
						
					}
				}
				
				
				
				
				if (sum < 49f) {
					result[w,h].r = 0f;
					result[w,h].g = 0f;
					result[w,h].b = 0f;
				}else {
					result[w,h].r = 1f;
					result[w,h].g = 1f;
					result[w,h].b = 1f;
				}
				
				
			}
		}
		
		
		
		return result;
	}

	public Color[,] GetPixels2D (Texture2D i)
	{
		
		Color[,] texture2D = new Color[i.width, i.height];
		Color[] texture1D = i.GetPixels ();
		
		for (int h = 0; h < i.height; h++) {
			for (int w = 0; w < i.width; w++) {
				texture2D [w, h] = texture1D [h * i.width + w];
			}
		}
		return texture2D;
	}
	
	public void SetPixels2D (Color[,] i, Texture2D t)
	{
		Color[] texture1D = new Color[i.Length];

		int width = i.GetLength (0);
		int height = i.GetLength (1);
		
		for (int h = 0; h < height; h++) {
			for (int w = 0; w < width; w++) {
				texture1D [h * width + w] = i [w, h]; 
			}
		}
		
		t.SetPixels (texture1D);
		t.Apply ();
	}
}
