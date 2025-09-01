using Animancer;
using System;
using UnityEngine;

namespace Fisherman
{
    public class FishermanController : MonoBehaviour
    {
        [field: SerializeField] public AnimancerComponent Animator { get; private set; }
        [SerializeField] private Health health;

        public StateMachine<FishermanController> StateMachine { get; private set; }

        [field: Header("States")]
        [field: SerializeField] public IdleState IdleState { get; private set; } = new();
        [field: SerializeField] public ThrowTrashState ThrowTrashState { get; private set; } = new();
        [field: SerializeField] public GetHitState GetHitState { get; private set; } = new();

        private void Awake()
        {
            health.OnHealthChanged += Health_OnHealthChanged;

            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            StateMachine = new StateMachine<FishermanController>(this);

            IdleState.Init(this, StateMachine);
            ThrowTrashState.Init(this, StateMachine);
            GetHitState.Init(this, StateMachine);

            StateMachine.ChangeState(IdleState);
        }

        private void OnDestroy()
        {
            health.OnHealthChanged -= Health_OnHealthChanged;

            StateMachine.Destroy();
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        private void Health_OnHealthChanged(float beforeHealth, float afterHealth)
        {
            if(afterHealth < beforeHealth)
                StateMachine.ChangeState(GetHitState, true);
        }
    }
}