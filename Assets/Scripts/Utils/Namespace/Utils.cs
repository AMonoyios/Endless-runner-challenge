using System.Collections.Generic;
using UnityEngine;

// Namespace that holds all helper functions that improve development of make implementing stuff more convinient.
namespace Utils
{
	// Class that has helper functions for creating new gameobjects in scene
	public static class Create
	{
		public static GameObject NewGameObject(string name, Transform parent = null)
		{
			 return NewGameObject(name, Vector3.zero, Quaternion.identity, Vector3.one, parent);
		}
		public static GameObject NewGameObject(string name, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
		{
			GameObject gameObject = new()
			{
				name = name
			};
			gameObject.transform.SetPositionAndRotation(position, rotation);
			gameObject.transform.parent = parent;

			gameObject.transform.localScale = scale;

			return gameObject;
		}

		public static GameObject NewPrefab(GameObject prefab, Transform parent = null)
		{
			return NewPrefab(prefab, prefab.transform.position, prefab.transform.rotation, prefab.transform.localScale, parent);
		}
		public static GameObject NewPrefab(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
		{
			GameObject newPrefab = Object.Instantiate(prefab, position, rotation, parent);
			newPrefab.transform.localScale = scale;

			return newPrefab;
		}
	}

	// Class that holds extensions of data types. I used extensions in order to ensure the correct datatype mathing and prevent accidental call of methods from other data types.
	public static class Extensions
	{
		// Method that finds specific asset from pool given a string id
		public static SetupAsset FindById(this List<SetupAsset> setupAssets, string id)
		{
			foreach (SetupAsset asset in setupAssets)
			{
				if (asset.id == id)
				{
					return asset;
				}
			}

			return null;
		}

		// Method that rounds floats with a given precision decimal
		public static float RoundToDecimals(this float value, float precision, float threshold = 0.1f)
		{
            return value > threshold ? Mathf.Floor(value / precision) * precision : value;
        }

		// Method that finds the closest TerrainComponent for a given Transform
		public static TerrainComponent GetClosestTerrainComponent(this Transform terrainTransform)
		{
			TerrainComponent closest = null;
			float minDistance = Mathf.Infinity;
			Vector3 currentPosition = terrainTransform.position;

			foreach (IBehaviour behaviour in CustomBehaviourAssetsDatabase.Values)
			{
				TerrainComponent terrain = CustomBehaviourAssetsDatabase.GetBehaviour<TerrainComponent>(behaviour.GetGameObject);
				if (terrain == null)
					continue;

				float distance = Vector3.Distance(terrain.GetGameObject.transform.position, currentPosition);
				if (distance < minDistance)
				{
					closest = terrain;
					minDistance = distance;
				}
			}

			if (closest == null)
				Debug.LogError($"Failed to find the closest terrain component from {terrainTransform.gameObject.name}");

			return closest;
		}
	}

	// Helper functions for gizmos (Only for editor use)
	public static class GizmosExtra
	{
		public static void DrawSphereAboveObject(Transform target)
		{
			DrawSphereAboveObject(target, Color.red);
		}
		public static void DrawSphereAboveObject(Transform target, Color color)
		{
			Gizmos.color = color;
        	Vector3 gizmoPos = new(	target.position.x,
                               		target.position.y + target.localScale.y,
                               		target.position.z);
        	Gizmos.DrawSphere(gizmoPos, 0.25f);
		}

		public static void DrawOutlinedCube(Transform target, Color facesColor, Color edgesColor)
		{
			Gizmos.color = facesColor;
        	Gizmos.DrawCube(target.position, target.localScale);
        	Gizmos.color = edgesColor;
        	Gizmos.DrawWireCube(target.position, target.localScale);
		}

		public static void DrawYLevelLine(Transform target, float yLevel, Color color)
		{
			Gizmos.color = color;
			Vector3 leftPoint = new
			(
				x: target.position.x - (target.localScale.x / 2.0f),
				y: yLevel,
				z: target.position.z
			);
			Vector3 rightPoint = new
			(
				x: target.position.x + (target.localScale.x / 2.0f),
				y: yLevel,
				z: target.position.z
			);
			Gizmos.DrawLine(leftPoint, rightPoint);
		}
	}

	public static class ColorExtra
	{
		public static Color Brown
		{
			get { return new Color(0.5f, 0.25f, 0.016f, 1.0f); }
		}

		public static Color Orange
		{
			get { return new Color(1.0f, 0.55f, 0.1f, 1.0f); }
		}
	}
}
