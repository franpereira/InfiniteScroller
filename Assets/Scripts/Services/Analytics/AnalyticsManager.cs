using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Analytics;
using UnityEngine;
using Kumi.Ingredients;
using Kumi.Core;
using System;
using Events = Kumi.Core.Events;

namespace Kumi.Services.Analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        string consentIdentifier;
        bool consentHasBeenChecked;

        async void Start()
        {
            InitializationOptions options = new();
            options.SetEnvironmentName("dev");
            try
            {
                await UnityServices.InitializeAsync(options);
                List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            }
            catch (ConsentCheckException e)
            {
                {
                    Debug.LogError(e.Message);
                }
            } 
        }

        private void OnEnable()
        {
           Events.End += EndEvent;
        }
        private void OnDisable()
        {
            Events.End -= EndEvent;
        }

        void EndEvent()
        {
            var parameters = new Dictionary<string, object>()
            {
                {"score", Score.Total},
                {"livesObtained", Lives.Obtained},
                {"durationUntilEnd", Convert.ToInt32(Duration.UntilEnd.TotalSeconds)}
            };
            AnalyticsService.Instance.CustomData("coreEnd", parameters);
        }
    }
}
