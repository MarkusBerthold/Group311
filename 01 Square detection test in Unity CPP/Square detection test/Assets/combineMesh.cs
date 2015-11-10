using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class combineMesh : Singleton<combineMesh> {
	void Start() {



		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            //combine[i].transform = Matrix4x4.TRS(gameObject.transform.InverseTransformPoint(gameObject.transform.position), Quaternion.Inverse(gameObject.transform.rotation), Vector3.one);
        

        meshFilters[i].gameObject.SetActive(true);
			i++;
		}
		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		transform.gameObject.SetActive(true);
        
        
	}
}