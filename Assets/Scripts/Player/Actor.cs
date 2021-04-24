﻿using UnityEngine;

namespace Actors {
    [RequireComponent (typeof (Rigidbody2D))]
    [RequireComponent (typeof (BoxCollider2D))]
    public class Actor : MonoBehaviour {
        [Header ("Speed & SubPixel Movement Counter")]
        public Vector2 Speed; // The Speed of the Actor
        public Vector2 movementCounter = Vector2.zero; // Counter for subpixel movement

        [Header ("Collision Layers")]
        public LayerMask solid_layer; // The layer on which solids are placed

        [SerializeField, Header ("Collider")]
        protected Collider2D myCollider; // Cached collider (only use BoxCollider2D for actors, since we use center and extents to check for collisions) 

        protected void Awake () {
            // If the collider has not been assigned manually we'll try to get the collider2d component of this object
            if (myCollider == null) {
                myCollider = GetComponent<Collider2D> ();
                if (myCollider == null) {
                    Debug.Log ("This Actor has no Collider2D component");
                }
            }
        }

        // Function to move the Actor Horizontally, this only stores the float value of the movement to allow for subpixel movement and calls the MoveHExact function to do the actual movement
        public bool MoveH (float moveH) {
            this.movementCounter.x = this.movementCounter.x + moveH;
            int num = (int) Mathf.Round (this.movementCounter.x);
            if (num != 0) {
                this.movementCounter.x = this.movementCounter.x - (float) num;
                return this.MoveHExact (num);
            }
            return false;
        }

        // Function to move the Actor Vertically, this only stores the float value of the movement to allow for subpixel movement and calls the MoveVExact function to do the actual movement
        public bool MoveV (float moveV) {
            this.movementCounter.y = this.movementCounter.y + moveV;
            int num = (int) Mathf.Round (this.movementCounter.y);
            if (num != 0) {
                this.movementCounter.y = this.movementCounter.y - (float) num;
                return this.MoveVExact (num);
            }
            return false;
        }

        // Function to move the Actor Horizontally an exact integer amount
        public virtual bool MoveHExact (int moveH) {
            int num = (int) Mathf.Sign ((float) moveH);
            while (moveH != 0) {
                bool solid = CheckColAtPlace (Vector2.right * (float) num * 5, solid_layer);

                if (solid) {
                    // Debug.Log("Entrou");
                    this.movementCounter.x = 0f;
                    return true;
                }

                moveH -= num;
                transform.position = new Vector2 (transform.position.x + (float) num, transform.position.y);
            }
            return false;
        }

        // Function to move the Actor Vertically an exact integer amount
        public bool MoveVExact (int moveV) {
            int num = (int) Mathf.Sign ((float) moveV);
            while (moveV != 0) {
                bool solid = CheckColAtPlace (Vector2.up * (float) num * 5, solid_layer);

                if (solid) {
                    this.movementCounter.y = 0f;
                    return true;
                }
                moveV -= num;
                transform.position = new Vector2 (transform.position.x, transform.position.y + (float) num);
            }
            return false;
        }

        // Helper function to check if there is any collision within a given layer in a set direction (only use up, down, left, right)
        public bool CheckColInDir (Vector2 dir, LayerMask layer) {
            Vector2 leftcorner = Vector2.zero;
            Vector2 rightcorner = Vector2.zero;

            if (dir.x > 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
                rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x + .5f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
            } else if (dir.x < 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x - .5f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
                rightcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
            } else if (dir.y > 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y + .5f);
                rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y);
            } else if (dir.y < 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y);
                rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y - .5f);
            }

            return Physics2D.OverlapArea (leftcorner, rightcorner, layer);
        }

        // The same as CheckColInDir but it returns a Collider2D array of the colliders you're collisioning with
        public Collider2D[] CheckColsInDirAll (Vector2 dir, LayerMask layer) {
            Vector2 leftcorner = Vector2.zero;
            Vector2 rightcorner = Vector2.zero;

            if (dir.x > 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
                rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x + .5f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
            } else if (dir.x < 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x - .5f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
                rightcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
            } else if (dir.y > 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y + .5f);
                rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y);
            } else if (dir.y < 0) {
                leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y);
                rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y - .5f);
            }

            return Physics2D.OverlapAreaAll (leftcorner, rightcorner, layer);
        }

        // Checks if there is a collision on top of the actor in a given layer (specially good to check if your are on top of a oneway/fallthrough platform or going through it)
        public bool CollisionSelf (LayerMask layer) {
            Vector2 leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .5f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f);
            Vector2 rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .5f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f);
            return Physics2D.OverlapArea (leftcorner, rightcorner, layer);
        }

        // Helper function to check if there is any collision within a given layer in a set direction (only use up, down, left, right)
        public bool CheckColAtPlaceOld (Vector2 extraPos, LayerMask layer) {
            Vector2 leftcorner = Vector2.zero;
            Vector2 rightcorner = Vector2.zero;

            leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f) + extraPos;
            rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f) + extraPos;

            return Physics2D.OverlapArea (leftcorner, rightcorner, layer);
        }

        // Helper function to check if there is any collision within a given layer in a set direction (only use up, down, left, right)
        public bool CheckColAtPlace (Vector2 extraPos, LayerMask layer) {
            var result = false;
            Vector2 leftcorner = Vector2.zero;
            Vector2 rightcorner = Vector2.zero;

            leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .8f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f) + extraPos;
            rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .8f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f) + extraPos;

            if (Physics2D.OverlapArea (leftcorner, rightcorner, layer)) {
                var cols = Physics2D.OverlapAreaAll (leftcorner, rightcorner, layer);

                if (cols.Length > 0) {
                    foreach (Collider2D c in cols) {
                        if (c != myCollider) {
                            // Debug.Log (leftcorner);
                            // Debug.Log (rightcorner);
                            // Debug.Log ("Cabou");

                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        // Helper function to check if there is any collision within a given layer in a set direction (only use up, down, left, right)
        public Collider2D[] CheckColAtPlaceAll (Vector2 extraPos, LayerMask layer) {
            Vector2 leftcorner = Vector2.zero;
            Vector2 rightcorner = Vector2.zero;

            leftcorner = new Vector2 (myCollider.bounds.center.x - myCollider.bounds.extents.x + .1f, myCollider.bounds.center.y + myCollider.bounds.extents.y - .1f) + extraPos;
            rightcorner = new Vector2 (myCollider.bounds.center.x + myCollider.bounds.extents.x - .1f, myCollider.bounds.center.y - myCollider.bounds.extents.y + .1f) + extraPos;

            return Physics2D.OverlapAreaAll (leftcorner, rightcorner, layer);
        }

        // Checks if there is a collision on top of the actor in a given layer (specially good to check if your are on top of a oneway/fallthrough platform or going through it)
        public bool CollisionAtPlace (Vector2 position, LayerMask layer) {
            Vector2 leftcorner = new Vector2 (position.x - 1, position.y + 1);
            Vector2 rightcorner = new Vector2 (position.x + 1, position.y - 1);
            return Physics2D.OverlapArea (leftcorner, rightcorner, layer);
        }
    }
}
