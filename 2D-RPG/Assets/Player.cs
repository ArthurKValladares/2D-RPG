using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet input;
    public Vector2 moveInput { get; private set; }

    private StateMachine sm;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }

    public Rigidbody2D rb;

    [field: SerializeField] public float moveSpeed { get; private set; } = 1.0f;

    private void Awake()
    {
        input = new PlayerInputSet();

        sm = new StateMachine();
        idleState = new Player_IdleState(this, sm);
        moveState = new Player_MoveState(this, sm);

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => {
            moveInput = ctx.ReadValue<Vector2>();
        };
        input.Player.Movement.canceled += ctx => {
            moveInput = Vector2.zero;
        };
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        sm.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        sm.UpdateActiveState();
    }
}
