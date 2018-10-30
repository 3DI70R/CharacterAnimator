using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
	public partial class CharacterAnimator
	{
		private class AnimationClipNode : NodeBase, IAnimationClipNode
		{
			private static readonly StreamType[] clipOutputTypes =
			{
				StreamType.Animation
			};

			private AnimationClipPlayable clipPlayable;
			private AnimationClip animationClip;

			public AnimationClip Clip
			{
				get { return animationClip; }
			}

			public bool FootIk
			{
				get { return clipPlayable.GetApplyFootIK(); }
				set { clipPlayable.SetApplyFootIK(value); }
			}

			public bool PlayableIk
			{
				get { return clipPlayable.GetApplyPlayableIK(); }
				set { clipPlayable.SetApplyPlayableIK(value); }
			}

			public override Playable Playable
			{
				get { return clipPlayable; }
			}

			public override StreamType[] OutputTypes
			{
				get { return clipOutputTypes; }
			}

			public AnimationClipNode(PlayableGraph graph, AnimationClip clip)
			{
				animationClip = clip;
				clipPlayable = AnimationClipPlayable.Create(graph, clip);
			}
		}
	}
}