using System;
using System.Collections;
using Kumi.Core;
using UnityEngine;

namespace Kumi.Ingredients
{
    /// <summary>
    /// Stores and changes the global difficulty value of the game.
    /// </summary>
    public class Difficulty : MonoBehaviour
    {
        public static event Action Increased;
        public static int Value { get; private set; }

        readonly WaitForSeconds waitFor = new(7.5f); //How often the value should increase.

        private void Awake() => Value = 0;
        private void OnEnable()
        {
            Events.Begin += Resume;
            Events.Resurrection += Resume;
            Events.ContinueQuestion += Stop;
            Events.End += Stop;
        }
        private void OnDisable()
        {
            Events.Begin -= Resume;   
            Events.Resurrection -= Resume;
            Events.ContinueQuestion -= Stop;
            Events.End -= Stop;
        }

        void Resume() => StartCoroutine(IncreaseRoutine());
        void Stop() => StopAllCoroutines();

        IEnumerator IncreaseRoutine()
        {
            while (true)
            {
                yield return waitFor;
                Value++;
                Debug.Log($"Difficulty increased: {Value}");
                Increased?.Invoke();
            }
        }
    }
}