﻿using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;
using UnityEngine.Playables;

namespace ThreeDISevenZeroR.CharacterAnimator
{
	public partial class CharacterAnimator : MonoBehaviour
	{
		private const string graphName = "Character";
    
		private enum StreamType
		{
			Unknown = -1,
			Animation = 0,
			IK = 1
		}

		[SerializeField]
		private Animator playableAnimator;
    
		private PlayableGraph graph;
		private AnimationPlayableOutput animatorOutput;
		private AnimationScriptPlayable animationPostprocess;
		private ScriptPlayable<IKAnimationOutputBehavior> animationOutput;

		private ScriptPlayableOutput ikScriptOutput;
		private LayerMixerNode rootLayerMixer;

		public ILayerMixerNode Root => rootLayerMixer;

		private void Awake()
		{
			graph = PlayableGraph.Create(graphName);
			graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

			var job = new AnimationIKApplyJobPlayable();
			animationPostprocess = AnimationScriptPlayable.Create(graph, job, 1);
			animationOutput = ScriptPlayable<IKAnimationOutputBehavior>.Create(graph, 
				new IKAnimationOutputBehavior { postprocess = animationPostprocess });
			
			animationOutput.SetTraversalMode(PlayableTraversalMode.Mix);
			animationPostprocess.ConnectInput(0, animationOutput, 0);
			
			animatorOutput = AnimationPlayableOutput.Create(graph, graphName + ".Animation", playableAnimator);
			animatorOutput.SetSourcePlayable(animationPostprocess);
	
			rootLayerMixer = new LayerMixerNode(graph, gameObject);
			AttachOutputs(rootLayerMixer);
   
			graph.Play();
		}

		private void AttachOutputs(NodeBase node)
		{
			var types = node.OutputTypes;
			var playable = node.Playable;
        
			for(var i = 0; i < types.Length; i++)
			{
				var t = types[i];

				switch (t)
				{
					case StreamType.Animation:
					case StreamType.IK:
						animationOutput.AddInput(playable, i, 1f);
						break;
				}
			}
		}
		
		private void OnDestroy()
		{
			graph.Destroy();
		}
	}
}