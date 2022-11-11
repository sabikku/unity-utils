using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/**
 * Calculating the sum of gameObject hierarchy tree renderers bounds.
 */

public static class RenderersBounds {
	public static Bounds MaximumBounds(GameObject gameObject) {
		float minX = float.MaxValue;
		float minY = float.MaxValue;
		float minZ = float.MaxValue;

		float maxX = float.MinValue;
		float maxY = float.MinValue;
		float maxZ = float.MinValue;
		
		Renderer[] renderers = gameObject.transform.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in renderers) {
			if (renderer is ParticleSystemRenderer || renderer is TrailRenderer) {
				continue;
			}

			Vector3 rMin = renderer.bounds.min - gameObject.transform.position;
			Vector3 rMax = renderer.bounds.max - gameObject.transform.position;

			minX = Mathf.Min(minX, rMin.x);
			minY = Mathf.Min(minY, rMin.y);
			minZ = Mathf.Min(minZ, rMin.z);

			maxX = Mathf.Max(maxX, rMax.x);
			maxY = Mathf.Max(maxY, rMax.y);
			maxZ = Mathf.Max(maxZ, rMax.z);
		}

		Vector3 min = new Vector3(minX, minY, minZ);
		Vector3 max = new Vector3(maxX, maxY, maxZ);

		Vector3 center = (min + max) / 2f;
		Vector3 size = max - min;

		return new Bounds(center, size);
	}

	public static Vector3 MiddlePoint(GameObject gameObject) {
		return MaximumBounds(gameObject).center;
	}
}