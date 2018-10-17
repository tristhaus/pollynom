using Persistence.Models;
using TestInfrastructure;

namespace PersistenceTest.Helper
{
    internal static class GameModelEqualityChecker
    {
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
