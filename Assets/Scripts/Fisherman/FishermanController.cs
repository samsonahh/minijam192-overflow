using Animancer;
using UnityEngine;

namespace Fisherman
{
    public class FishermanController : MonoBehaviour
    {
        [field: SerializeField] public AnimancerComponent Animator { get; private set; }

        public StateMachine<FishermanController> StateMachine { get; private set; }

        [field: Header("States")]
        [field: SerializeField] public IdleState IdleState { get; private set; } = new();
        [field: SerializeField] public ThrowTrashState ThrowTrashState { get; private set; } = new();
        [field: SerializeField] public HarpoonState HarpoonState { get; private set; } = new();

        private void Awake()
        {
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            StateMachine = new StateMachine<FishermanController>(this);

            IdleState.Init(this, StateMachine);
            ThrowTrashState.Init(this, StateMachine);
            HarpoonState.Init(this, StateMachine);

            StateMachine.ChangeState(IdleState);
        }

        private void OnDestroy()
        {
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
    }
}