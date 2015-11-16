using UnityEngine;
using System.Collections;
using System;
public class ParticleSystem_earn : MonoBehaviour
{
		public GameObject particleCube;
		public GameObject particleGui;
		static ParticleSystem_earn instance;
		public static ParticleSystem_earn Instance {
				get {
						if (instance == null) {
								instance = FindObjectOfType (typeof(ParticleSystem_earn)) as ParticleSystem_earn;
								if (instance == null)
										throw new InvalidOperationException ("No instance in scene!");
						}
						return instance;
				}
		}
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
		public GameObject onCube (Vector3 pos)
		{
				return Instantiate (particleCube, pos, Quaternion.identity) as GameObject;
		}
		public GameObject incMoney (Vector3 pos)
		{
				return Instantiate (particleGui, pos, Quaternion.identity) as GameObject;
		}
}
