using UnityEngine;
using System.Collections;

public class MoveSegment : MonoBehaviour {
	public Vector3 endPoint;
	public float speed;

	

	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Start()
	{
		Vector3 pointA = transform.position;
		while (true) {
			yield return StartCoroutine(MoveObject(transform, pointA, endPoint, speed));
			yield return StartCoroutine(MoveObject(transform, endPoint, pointA, speed));
		}
	}

	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		float i = 0.0f;
		float rate = 1.0f / time;

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			this.transform.localPosition = Vector3.Lerp(startPos, endPos, i);
			yield return null;
		}
	}
}
