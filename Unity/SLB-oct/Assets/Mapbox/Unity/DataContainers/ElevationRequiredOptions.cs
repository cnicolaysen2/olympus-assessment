namespace Mapbox.Unity.Map
{
	using System;
	using UnityEngine;
	[Serializable]
	public class ElevationRequiredOptions : MapboxDataProperty
	{
		[Range(-20, 100)]
		[Tooltip("Multiplication factor to vertically exaggerate elevation on terrain, does not work with Flat Terrain.")]
		public float exaggerationFactor = 1;

        [Range(-10000, 10000)]
        public float displacement = 0;

		public override bool NeedsForceUpdate()
		{
			return true;
		}
	}
}
