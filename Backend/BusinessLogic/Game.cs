using System.Collections.Generic;
using System.Linq;
using Backend.BusinessLogic.Dots;
using Persistence.Models;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// A domain model representing a game, implementing validation logic.
    /// </summary>
    public class Game
    {
        public const int MaxNumberExpressions = 5;
        private static Parser parser = new Parser();

        private List<IDot> dots;
        private IExpression[] expressions;
        private string[] expressionStrings;

        private Game(List<IDot> dots, string[] expressionStrings)
        {
            this.dots = dots;
            this.expressionStrings = expressionStrings;

            this.expressions = new IExpression[MaxNumberExpressions];
            for (int i = 0; i < this.expressions.Length; i++)
            {
                var expressionString = this.expressionStrings[i];
                if (!string.IsNullOrEmpty(expressionString) && parser.IsParseable(expressionString))
                {
                    this.expressions[i] = Game.parser.Parse(expressionString);
                }
            }
        }

        /// <summary>
        /// Given a valid model, provides a game from it.
        /// </summary>
        /// <param name="model">The model to use as a  basis for the game.</param>
        /// <returns>A <see cref="IMaybe{T}"/> containing a game, if model is valid.</returns>
        public static IMaybe<Game> FromModel(GameModel model)
        {
            if (model.ExpressionStrings.Count != Game.MaxNumberExpressions)
            {
                return new None<Game>();
            }

            var game = new Game(
                model.DotModels.Select(
                    dm => dm.Kind == DotKind.Good
                    ? new GoodDot(dm.X, dm.Y) as IDot
                    : new BadDot(dm.X, dm.Y) as IDot)
                    .ToList(),
                model.ExpressionStrings.ToArray());

            return new Some<Game>(game);
        }

        /// <summary>
        /// Creates a new game with the number of dots given.
        /// </summary>
        /// <param name="goodDotsNumber">Number of good dots to be in the game.</param>
        /// <param name="badDotsNumber">Number of bad dots to be in the game.</param>
        /// <returns>A <see cref="IMaybe{T}"/> containing a game.</returns>
        public static IMaybe<Game> NewRandom(int goodDotsNumber = 8, int badDotsNumber = 2)
        {
            var generator = new RandomDotsGenerator(goodDotsNumber, badDotsNumber);

            var game = new Game(generator.Generate(), new string[MaxNumberExpressions]);

            return new Some<Game>(game);
        }
    }
}
