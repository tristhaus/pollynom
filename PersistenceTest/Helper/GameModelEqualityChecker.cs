using Persistence.Models;
using TestInfrastructure;

namespace PersistenceTest.Helper
{
    /// <summary>
    /// Implements the checking of two <see cref="GameModel"/> instances for equality.
    /// </summary>
    internal static class GameModelEqualityChecker
    {
        /// <summary>
        /// Checks the equality using the <see cref="DoubleEquality.IsApproximatelyEqual(double, double)"/> method.
        /// </summary>
        /// <param name="model1">First model to check.</param>
        /// <param name="model2">Second model to check.</param>
        /// <returns><c>true</c> if the models are approximately equal.</returns>
        public static bool AreApproximatelyEqual(GameModel model1, GameModel model2)
        {
            int count1 = model1.DotModels.Count;
            if (count1 != model2.DotModels.Count)
            {
                return false;
            }

            for (int i = 0; i < count1; i++)
            {
                if (!DoubleEquality.IsApproximatelyEqual(model1.DotModels[i].X, model2.DotModels[i].X)
                    || !DoubleEquality.IsApproximatelyEqual(model1.DotModels[i].Y, model2.DotModels[i].Y))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
