using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	public float force = 10f;
	public float forceOffset = 0.1f;


	// Use this for initialization
	void Start () {
		
	}

	void OnCollisionStay(Collision col) {
		Vector3 point =col.contacts[0].point;
		point +=col.contacts[0].normal * forceOffset;
		if (col.collider.gameObject.tag == "Web") {
			MeshDeformer deformer = col.collider.gameObject.GetComponent<MeshDeformer> ();
			deformer.AddDeformingForce (point, force);
		}

	}


	void HandleInput () {
		
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
			if (deformer) {
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce(point, force);
			}
		}

	}

	// Update is called once per frame
	void Update () {
		//if (Input.GetMouseButton (0)) HandleInput ();


	}
}
