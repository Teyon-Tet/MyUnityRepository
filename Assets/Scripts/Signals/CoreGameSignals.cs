using System;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        //Singleton
        public static CoreGameSignals Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public UnityAction<int> onLevelInitialize = delegate { };
        public UnityAction onClearActiveLevel = delegate { };
        public Func<int> onGetLevelValue = delegate { return 0; };
        public UnityAction onNextLevel = delegate { };
        public UnityAction onRestartLevel = delegate { };
        public UnityAction onReset = delegate { };

    }
}