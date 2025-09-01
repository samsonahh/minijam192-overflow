using Animancer;
using UnityEngine;

namespace Fisherman
{
    [System.Serializable]
    public class GetHitState : State<FishermanController>
    {
        [SerializeField] private ClipTransition clip;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float duration = 2f;

        private float timer;

        public override void Enter()
        {
            clip.Speed = speed;
            context.Animator.Play(clip, 0.1f);

            timer = 0f;

            GameObject.FindAnyObjectByType<WaterWave>().PulseFromEpicenter();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                stateMachine.ChangeState(context.IdleState);
                return;
            }
        }

        public override void FixedUpdate()
        {

        }
    }
}