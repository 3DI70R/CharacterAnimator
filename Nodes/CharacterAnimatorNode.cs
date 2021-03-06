﻿using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
	public partial class CharacterAnimator
	{
		private abstract class NodeBase : INode
		{
			private double speed = 1.0;

			public abstract Playable Playable { get; }
			public abstract StreamType[] OutputTypes { get; }

			public double Speed
			{
				get { return speed; }
				set { Playable.SetSpeed(value); }
			}
		}
	}
}