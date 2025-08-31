using Animancer;
using UnityEngine;

namespace Fisherman
{
    [System.Serializable]
    public class HarpoonState : State<FishermanController>
    {
        [SerializeField] private ClipTransition clip;
        [SerializeField] private FloatRange clipSpeedRange = new FloatRange(0.9f, 1.8f);
        [SerializeField] private StringAsset harpoonEvent;
        [SerializeField] private Transform handTransform;
        [SerializeField] private GameObject harpoonPrefab;

        private float timer;
        private float duration;
        public override void Enter()
        {
            clip.Speed = clipSpeedRange.RandomValue();
            clip.Events.SetCallback(harpoonEvent, Harpoon);
            context.Animator.Play(clip, 0.2f);

            timer = 0f;
            duration = clip.Length / clip.Speed;
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if(timer > duration)
            {
                stateMachine.ChangeState(context.IdleState);
                return;
            }
        }

        public override void FixedUpdate()
        {

        }

        private void Harpoon()
        {
            Rigidbody trashRigidbody = GameObject.Instantiate(harpoonPrefab, handTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
            trashRigidbody.AddForce(200f * context.transform.forward, ForceMode.Impulse);
        }
    }
}