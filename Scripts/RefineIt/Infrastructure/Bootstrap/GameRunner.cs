﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Bootstrap
{
    public class GameRunner : MonoBehaviour
    {
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();
            if(bootstrapper == null)
                SceneManager.LoadScene(0);
        }
    }
}