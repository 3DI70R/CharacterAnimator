﻿using UnityEngine;
using UnityEngine.Animations;
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
		private AnimationPlayableOutput animationOutput;
		private LayerMixerNode _rootLayerMixer;

		public ILayerMixer RootLayer
		{
			get { return _rootLayerMixer; }
		}
		
		private void Awake()
		{
			graph = PlayableGraph.Create(graphName);
			animationOutput = AnimationPlayableOutput.Create(graph, graphName + ".Animation", playableAnimator);

			_rootLayerMixer = new LayerMixerNode(graph, gameObject);
			AttachOutputs(_rootLayerMixer);
   
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
						animationOutput.SetSourcePlayable(playable, i);
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