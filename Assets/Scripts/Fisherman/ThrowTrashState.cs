using Animancer;
using System.Collections.Generic;
using UnityEngine;

namespace Fisherman
{
    [System.Serializable]
    public class ThrowTrashState : State<FishermanController>
    {
        [SerializeField] private ClipTransition clip;
        [SerializeField] private FloatRange clipSpeedRange = new FloatRange(0.9f, 1.8f);
        [SerializeField] private List<GameObject> trashPrefabs;
        [SerializeField] private StringAsset throwTrashEvent;
        [SerializeField] private Transform handTransform;
        [SerializeField] private FloatRange throwForceRange = new FloatRange(5f, 10f);
        [SerializeField] private float rotationSpeed = 15f;

        private float timer;
        private float duration;

        private Vector3 randomThrowDirection;

        public override void Enter()
        {
            clip.Speed = clipSpeedRange.RandomValue();
            clip.Events.SetCallback(throwTrashEvent, ThrowTrash);
            context.Animator.Play(clip, 0.2f);

            timer = 0f;
            duration = clip.Length / clip.Speed;

            float randomAngle = Random.Range(0f, 360f);
            randomThrowDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        }

        public override void Exit()
        {
            clip.Events.RemoveCallback(throwTrashEvent, ThrowTrash);
        }

        public override void Update()
        {
            Quaternion targetRotation = Quaternion.LookRotation(randomThrowDirection, Vector3.up);
            context.transform.localRotation = Quaternion.Slerp(context.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

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

        private void ThrowTrash()
        {
            Rigidbody trashRigidbody = GameObject.Instantiate(GetTrashPrefab(), handTransform.position, Random.rotation).GetComponent<Rigidbody>();
            
            float randomThrowForce = throwForceRange.RandomValue();
            trashRigidbody.AddForce(randomThrowForce * randomThrowDirection, ForceMode.Impulse);
        }

        private GameObject GetTrashPrefab() => trashPrefabs[Random.Range(0, trashPrefabs.Count)];
    }
}