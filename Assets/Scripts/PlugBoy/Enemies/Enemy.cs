using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlugBoy.Characters;

namespace PlugBoy.Enemies
{

	public abstract class Enemy : MonoBehaviour
	{

		public abstract Collider2D Collider2D { get; }

		public abstract void Kill ( Character target );

	}

}