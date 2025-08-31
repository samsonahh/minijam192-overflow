using UnityEngine;

namespace Fisherman
{
    [System.Serializable]
    public class IdleState : State<FishermanController>
    {
        [SerializeField] private AnimationClip clip;

        public override void Enter()
        {
            context.Animator.Play(clip);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }

        public override void FixedUpdate()
        {

        }
    }
}