using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int V = Animator.StringToHash("v");
        private static readonly int U = Animator.StringToHash("u");
        
        private Animator _animator;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void PlayHit() => 
            _animator.SetTrigger(Hit);

        public void PlayDeath() =>
            _animator.SetTrigger(Die);

        public void MoveForward()
        {
            _animator.SetBool(IsMoving, true);
            _animator.SetFloat(V, 1);
            _animator.SetFloat(U, 0);
        }
        
        public void MoveBackward()
        {
            _animator.SetBool(IsMoving, true);
            _animator.SetFloat(V, -1);
            _animator.SetFloat(U, 0);
        }
        
        public void MoveLeft()
        {
            _animator.SetBool(IsMoving, true);
            _animator.SetFloat(V, 0);
            _animator.SetFloat(U, -1);
        }
        
        public void MoveRight()
        {
            _animator.SetBool(IsMoving, true);
            _animator.SetFloat(V, 0);
            _animator.SetFloat(U, 1);
        }

        public void StopMoving()
        {
            _animator.SetBool(IsMoving, false);
            _animator.SetFloat(V, 0);
            _animator.SetFloat(U, 0);
        }
    }
}