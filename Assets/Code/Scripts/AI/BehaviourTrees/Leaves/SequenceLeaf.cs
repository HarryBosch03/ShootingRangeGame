﻿using ShootingRangeGame.AI.BehaviourTrees.Core;

namespace ShootingRangeGame.AI.BehaviourTrees.Leaves
{
    public sealed class SequenceLeaf : CompositeLeaf
    {
        public override string Name => $"{base.Name} Sequence Leaf";
        protected override BehaviourTree.Result OnExecute(BehaviourTree tree) => 
            SimpleLoop(
                res => res == BehaviourTree.AbandonResponse.WithFailure,
                BehaviourTree.Result.Failure, 
                BehaviourTree.Result.Failure, 
                BehaviourTree.Result.Success);
    }
}