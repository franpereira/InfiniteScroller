using System;

namespace Kumi.Core
{
    public static class Events
    {
        /// <summary>
        /// When a new game is beginning.
        /// </summary>
        public static event Action Begin;
        
        /// <summary>
        /// When something that could make the player lose a life has happened.
        /// </summary>
        public static event Action EndConditionCheck;
        
        /// <summary>
        /// When the player has run out of lives but could get a bonus one.
        /// </summary>
        public static event Action ContinueQuestion;
        
        /// <summary>
        /// Whether the player will have an extra life or not
        /// </summary>
        public static event Action<bool> ContinueAnswer;

        /// <summary>
        /// When after losing a life, the player still has more left.
        /// </summary>
        public static event Action Resurrection;

        /// <summary>
        /// When definitely a game has ended.
        /// </summary>
        public static event Action End;
        
        /// <summary>
        /// When the scene will restart.
        /// </summary>
        public static event Action Restart;

        internal static void InvokeBegin() => Begin?.Invoke();
        internal static void InvokeEndConditionCheck() => EndConditionCheck?.Invoke();
        internal static void InvokeContinueQuestion() => ContinueQuestion?.Invoke();
        internal static void InvokeContinueAnswer(bool answer) => ContinueAnswer?.Invoke(answer);
        internal static void InvokeResurrection() => Resurrection?.Invoke();
        internal static void InvokeEnd() => End?.Invoke();
        internal static void InvokeRestart() => Restart?.Invoke();
    }
}
