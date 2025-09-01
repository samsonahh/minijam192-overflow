using UnityEngine;

namespace Fisherman
{
    [System.Serializable]
    public class IdleState : State<FishermanController>
    {
        [SerializeField] private AnimationClip clip;

        [SerializeField] private FloatRange attackIntervalRange = new FloatRange(1.5f, 3f);
        private float attackTimer = 0f;
        private float currentAttackInterval;

        public override void Enter()
        {
            context.Animator.Play(clip, 0.25f);

            attackTimer = 0f;
            currentAttackInterval = attackIntervalRange.RandomValue();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            attackTimer += Time.deltaTime;
            if(attackTimer > currentAttackInterval)
            {
                stateMachine.ChangeState(context.ThrowTrashState);
                return;
            }
        }

        public override void FixedUpdate()
        {

        }
    }
}