using System;
using UnityEngine;

namespace CodeBase.Common
{
    public class PlayerLostPopup : MonoBehaviour
    {
        private void Awake() => 
            DontDestroyOnLoad(this);

        public void Show() => 
            gameObject.SetActive(true);
        
        public void Hide() => 
            gameObject.SetActive(false);
    }
}