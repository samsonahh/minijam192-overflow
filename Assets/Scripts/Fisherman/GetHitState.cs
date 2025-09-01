using Animancer;
using UnityEngine;

namespace Fisherman
{
    [System.Serializable]
    public class GetHitState : State<FishermanController>
    {
        [SerializeField] private ClipTransition clip;
        [SerializeField] private float speed = 2f;

        private float timer;
        private float duration;

        public override void Enter()
        {
            clip.Speed = speed;
            context.Animator.Play(clip, 0.1f);

            timer = 0f;
            duration = clip.Length / speed;

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