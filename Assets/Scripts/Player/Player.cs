using System.Collections;
using utils;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;
using Audio;

namespace Actors {
    public class Player : Actor {
        // Run Speed & Acceleration
        [Title ("Run Settings")]
        public float MaxRun = 90f; // Maximun Horizontal Run Speed
        public float RunAccel = 1000f; // Horizontal Acceleration Speed
        public float RunReduce = 400f; // Horizontal Acceleration when you're already when your horizontal speed is higher or equal to the maximun

        [Title ("Dash Settings")]
        public float DashSpeed = 240f; // Speed/Force which you dash at
        public float EndDashSpeed = 160f; // Extra Speed Boost when the dash ends (2/3 of the dash speed recommended)
        public float EndDashUpMult = 0.75f; // Multiplier applied to the Speed after a dash ends if the direction you dashed at was up
        public float DashTime = 0.15f; // The total time which a dash lasts for
        public float DashCooldown = 0.4f; // The Cooldown time of a dash

        [Title ("Joystick config")]
        public Vector2 DeadZone;

        [Title ("Squash & Stretch")]
        public Transform SpriteHolder; // Reference to the transform of the child object which holds the sprite renderer of the player
        public Vector2 SpriteScale = Vector2.one; // The current X and Y scale of the sprite holder (used for Squash & Stretch)

        [Title("Audio")]
        public AudioClip dashClip;
        public AudioClip hitClip;

        // Helper private Variables
        private int moveX; // Variable to store the horizontal Input each frame
        private int moveY; // Variable to store the vectical Input each frame
        private Vector2 DashDir; // Here we store the direction in which we are dashing
        private float dashCooldownTimer = 0f; // Timer to store how much cooldown has the dash

        private Animator anim;
        private Vector2 raw;

        private float facing {
            get {
                return Mathf.Sign (transform.localScale.x);
            }
        }

        // Check if we should/can dash (the dash button has been pressed & If the cooldown has been completed)
        public bool CanDash {
            get {
                return Input.GetButtonDown ("Dash") && dashCooldownTimer <= 0f && (raw.x != 0 || raw.y != 0);
            }
        }

        public enum States {
            Idle,
            Walk,
            Dash
        }
        public StateMachine<States> fsm;
        new void Awake () {
            base.Awake ();
            fsm = StateMachine<States>.Initialize (this);
            anim = GetComponent<Animator> ();
        }

        void Start () {
            fsm.ChangeState (States.Idle);
            // Timer.Instance.totalTime.Value = 60;
        }

        void Idle_Update () {
            Normal_Update ();
        }

        void Walk_Update () {
            Normal_Update ();
        }

        void Normal_Update () {
            if (CanDash) {
                fsm.ChangeState (States.Dash, StateTransition.Overwrite);
                return;
            }

            if (dashCooldownTimer > 0) {
                dashCooldownTimer -= Time.deltaTime;
            }

            raw.y = Input.GetAxisRaw ("Vertical");
            raw.x = Input.GetAxisRaw ("Horizontal");

            if (raw.magnitude > 1f) {
                raw.Normalize ();
            }

            Speed.x = Calc.Approach (Speed.x, MaxRun * raw.x, RunReduce * Time.deltaTime);
            Speed.y = Calc.Approach (Speed.y, MaxRun * raw.y, RunReduce * Time.deltaTime);

            if (Speed.magnitude == 0) {
                fsm.ChangeState (States.Idle);
            } else {
                fsm.ChangeState (States.Walk);
            }
        }

        void LateUpdate () {
            // Do all the movement on the actor (base)
            // Horizontal

            var moveh = base.MoveH (Speed.x * Time.deltaTime);
            if (moveh) {
                Speed.x = 0;
            }

            // Vertical
            var movev = base.MoveV (Speed.y * Time.deltaTime);
            if (movev) {
                Speed.y = 0;
            }

            // if (Speed.x != 0) {
            //     var scale = transform.localScale;
            //     scale.x = Mathf.Sign (Speed.x);
            //     transform.localScale = scale;
            // }

            UpdateSprite ();
        }

        void UpdateSprite () {
            if (fsm.State != States.Dash) {
                SpriteScale.x = Calc.Approach (SpriteScale.x, 1f, 0.04f);
                SpriteScale.y = Calc.Approach (SpriteScale.y, 1f, 0.04f);
            }

            // Set the SpriteHolder scale to the target scale
            var targetSpriteHolderScale = new Vector3 (SpriteScale.x, SpriteScale.y, 1f);
            if (SpriteHolder.localScale != targetSpriteHolderScale) {
                SpriteHolder.localScale = targetSpriteHolderScale;
            }

            if (fsm.State == States.Idle) {
                anim.Play("Idle");
            } else if (fsm.State == States.Walk) {
                anim.Play("Walk");
                
                var direction = this.Speed.normalized;

                anim.SetFloat("PositionX", direction.x);
                anim.SetFloat("PositionY", direction.y);
            }
        }

        private IEnumerator Dash_Enter () {
            // AudioManager.Instance.PlaySfx(dashClip);
            dashCooldownTimer = DashCooldown;
            Speed = Vector2.zero;
            DashDir = Vector2.zero;
            Vector2 value = raw;

            value.Normalize ();
            Vector2 vector = value * DashSpeed;
            Speed = vector;
            DashDir = value;
            // if (DashDir. != 0f) {
            // Facing = (Facings) Mathf.Sign (DashDir.x);
            // }

            // Squash & Stretch
            if (Mathf.Abs(value.x) > Mathf.Abs(value.y)) {
                SpriteScale = new Vector2 (1.2f, 0.8f);
            } else {
                SpriteScale = new Vector2 (.8f, 1.2f);
            }

            // Screenshake
            // if (PixelCamera.instance != null) {
            //     PixelCamera.instance.DirectionalShake (DashDir);
            // }

            yield return new WaitForSeconds (DashTime);

            // Wait one extra frame
            yield return null;

            if (DashDir.y >= 0f) {
                Speed = DashDir * EndDashSpeed;
            }
            if (Speed.y > 0f) {
                Speed.y = Speed.y * EndDashUpMult;
            }

            fsm.ChangeState (States.Idle, StateTransition.Overwrite);
            yield break;
        }
    }
}
