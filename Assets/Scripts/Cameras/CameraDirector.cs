using Cinemachine;
using Kumi.Characters;
using Kumi.Core;
using Kumi.Ingredients;
using System;
using System.Collections;
using UnityEngine;

namespace Kumi.Cameras
{
    /// <summary>
    /// Main controller of the camera.
    /// </summary>
    public class CameraDirector : MonoBehaviour
    {
        public static CameraDirector Instance;

        /// <summary>
        /// When the height of the camera has exceeded the player's height by more than a certain distance.
        /// </summary>
        public static event Action CameraOvercamePlayer;

        [SerializeField] CinemachineVirtualCamera gameplayCam;
        [SerializeField] CMGameplay gameplayScript;
        [SerializeField] CinemachineVirtualCamera welcomeCam;
        [SerializeField] CinemachineVirtualCamera pauseCam;

        CinemachineBasicMultiChannelPerlin noise;

        bool canTriggerEnd; //If the CameraOvercamePlayer event should try to end the game.
        float savedSpeed; //The speed of the scrolling previous to stopping it.
        bool resumingFromContinue; //If the game is resuming from a Continue Event.
        float savedIncrease; //Increase in speed that shouldn't be applied instantly. 

        private void Awake() => Instance = this;
        private void Start()
        {
            noise = gameplayCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            StartCoroutine(FovSetter());
        }

        private void OnEnable()
        {
            Events.Begin += OnBegin;
            Events.ContinueQuestion += OnContinueQuestion;
            Events.ContinueAnswer += OnContinueAnswer;
            Events.Resurrection += OnResurrection;
            Events.End += OnEnd;
            Difficulty.Increased += IncreaseSpeed;
            TimePause.Toggled += OnTimePauseToggled;
        }
        private void OnDisable()
        {
            Events.Begin -= OnBegin;
            Events.ContinueQuestion -= OnContinueQuestion;
            Events.ContinueAnswer -= OnContinueAnswer;
            Events.Resurrection -= OnResurrection;
            Events.End -= OnEnd;
            Difficulty.Increased -= IncreaseSpeed;
            TimePause.Toggled -= OnTimePauseToggled;
        }

        readonly WaitForSecondsRealtime waitForFov = new(1f);


        //Adjust the FOV depending on the screen size.  
        IEnumerator FovSetter()
        {
            while (true)
            {
                const float hFov = 56.86f - 0.06f; //Reference value for 9:16 and Z=-10
                float aspectRatio = gameplayCam.m_Lens.Aspect < 0.5006878f ? gameplayCam.m_Lens.Aspect : 0.5006878f;
                float vFov = CameraHelper.VFov(hFov, aspectRatio);
                gameplayCam.m_Lens.FieldOfView = vFov;
                yield return waitForFov;
            }
            
        }

        private void FixedUpdate()
        {
            if (canTriggerEnd == false || IsPlayerUnderCamera == false) return;
            canTriggerEnd = false;
            CameraOvercamePlayer?.Invoke();
        }
        bool IsPlayerUnderCamera => transform.position.y > PlayerCharacter.Instance.transform.position.y + 16f;

        const float initialSpeed = 0.4f;
        void OnBegin()
        {
            StartCoroutine(DisableWelcomeCam());
            gameplayScript.ScrollSpeed = initialSpeed;
            gameplayScript.ScrollEnabled = true;
            canTriggerEnd = true;
            Debug.Log($"CAMERA ASPECT: {gameplayCam.m_Lens.Aspect}");
        }

        
        IEnumerator DisableWelcomeCam()
        {
            //Must wait a frame or the transition won't occur.
            yield return null;
            welcomeCam.gameObject.SetActive(false);
        }

        void OnContinueQuestion()
        {
            StartCoroutine(StopScroll());
            canTriggerEnd = false;
        }

        void OnContinueAnswer(bool answer) => resumingFromContinue = answer;

        void OnResurrection()
        {
            StartCoroutine(ResumeScroll());
            if (resumingFromContinue) gameplayScript.ScrollEnabled = true;
            StartCoroutine(DelayedTriggerEndEnabling());
        }

        //Avoiding the camera to trigger the end event immediately after a resurrection.
        IEnumerator DelayedTriggerEndEnabling()
        {
            yield return new WaitForSecondsRealtime(1f);
            canTriggerEnd = true;
        }

        void OnEnd()
        {
            gameplayScript.ScrollEnabled = true; //Because it could have been disabled with a pause
            StartCoroutine(StopScroll());        //and we want to make sure the stop animation happens. 
            canTriggerEnd = false;
        }

        void IncreaseSpeed()
        {
            float add = 0;
            switch (Difficulty.Value)
            {
                case <= 8:
                    add = 0.12f;
                    break;
                case <= 16:
                    add = 0.04f;
                    break;
                case <= 24:
                    add = 0.01f;
                    break;
                case <= 32:
                    add = 0.005f;
                    break;
            }

            if (resumingFromContinue) savedIncrease += add; //If the speed is recovering from a continue resurrection, add it later to avoid messing with the transition. 
            else gameplayScript.ScrollSpeed += add;
        }


        readonly WaitForFixedUpdate waitForScroll = new();
        readonly WaitForFixedUpdate waitForShake = new();

        ///Shake effect for the camera.
        public void Shake(float amplitude, int steps) => StartCoroutine(ShakeRoutine(amplitude, steps));
        IEnumerator ShakeRoutine(float amplitude, int steps)
        {
            for (float i = 0f; i <= 1f; i += 1f / steps)
            {
                noise.m_AmplitudeGain = Mathf.Lerp(1f, 0f, i);
                yield return waitForShake;
            }

            noise.m_AmplitudeGain = 0f;
        }

        /// Smoothly stops the scroll going from the current speed to 0.
        IEnumerator StopScroll()
        {
            if (gameplayScript.ScrollSpeed == 0f) yield break;

            savedSpeed = gameplayScript.ScrollSpeed; //Saves the current speed for resuming to it later.
            const int steps = 180;
            for (float i = 0f; i <= 1f; i += 1f / steps)
            {
                gameplayScript.ScrollSpeed = Mathf.Lerp(savedSpeed, 0f, i);
                yield return waitForScroll;
            }
            gameplayScript.ScrollSpeed = 0f;
        }

        /// Smoothly resumes the scroll from 0 to the saved speed.
        IEnumerator ResumeScroll()
        {
            savedSpeed = savedSpeed > 0 ? savedSpeed : gameplayScript.ScrollSpeed;
            gameplayScript.ScrollSpeed = 0;
            const int steps = 360;
            for (float i = 0f; i <= 1f; i += 1f / steps)
            {
                gameplayScript.ScrollSpeed = Mathf.Lerp(0f, savedSpeed, i);
                yield return waitForScroll;
            }
            gameplayScript.ScrollSpeed = savedSpeed;
            if (savedIncrease > 0f)
            {
                yield return waitForScroll;
                gameplayScript.ScrollSpeed += savedIncrease;
                savedIncrease = 0;
            }
            resumingFromContinue = false;
        }

        void OnTimePauseToggled(bool paused)
        {
            gameplayScript.ScrollEnabled = !paused;
            pauseCam.gameObject.SetActive(paused);
        }
    }
}