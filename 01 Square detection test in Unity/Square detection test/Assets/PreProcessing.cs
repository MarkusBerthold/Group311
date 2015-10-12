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
		for (int w = 0; w < i.GetLength(1); w++) {
			for (int h = 0; h < i.GetLength(0); h++) {
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
		
		for (int w = kw; w < i.GetLength(1) - kw; w++) {
			for (int h = kh; h < i.GetLength(0) - kh; h++) {
				
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

	public Color[,] Rgb2greyScale (Color[,] i)
	{
		for (int w = 0; w < i.GetLength(1); w++) {
			for (int h = 0; h < i.GetLength(0); h++) {
				float grey = (i [w, h].r + i [w, h].g + i [w, h].b) / 3;
				i [w, h].r = grey;
				i [w, h].g = grey;
				i [w, h].b = grey;
			}
		}
		return i;
	}

	public Color[,] Erode (Color[,] i) {

		Color[,] tmp = i;

		for (int w = 1; w < i.GetLength(1) - 1; w++) {
			for (int h = 1; h < i.GetLength(0) - 1; h++) {
				
				int count = 0;
				
				int [] neighborhood = new int[9];
				
				for (int j = -1; j <= 1; j++) {
					for (int l = -1; l <= 1; l++) {
						
						neighborhood [count] = (int) (tmp [w + j, h + l].r);
						count ++;

					}
				}

				neighborhood = BubbleSort ( neighborhood);

				//print (neighborhood[8]);

				/*for(int huh = 0; huh < neighborhood.Length; huh++)
				print (neighborhood[huh]);

				break;*/
				if (neighborhood[8] >= 1) {
					i[w,h].r = 0;
					i[w,h].g = 0;
					i[w,h].b = 0;
				}else {
					i[w,h].r = 1;
					i[w,h].g = 1;
					i[w,h].b = 1;
				}


			}
		}



		return i;
	}

	public Color[,] GetPixels2D (Texture2D i)
	{
		
		Color[,] texture2D = new Color[i.width, i.height];
		Color[] texture1D = i.GetPixels ();
		
		for (int w = 0; w < i.width; w++) {
			for (int h = 0; h < i.height; h++) {
				texture2D [w, h] = texture1D [w * i.width + h];
			}
		}
		return texture2D;
	}
	
	public void SetPixels2D (Color[,] i, Texture2D t)
	{
		Color[] texture1D = new Color[i.Length];
		
		for (int w = 0; w < i.GetLength(1); w++) {
			for (int h = 0; h < i.GetLength(0); h++) {
				texture1D [w * i.GetLength (1) + h] = i [w, h]; 
			}
		}
		
		t.SetPixels (texture1D);
		t.Apply ();
	}
}
