using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public const int MaxExpressionCount = 5;
        private static Parser parser = new Parser();

        private List<IDot> dots;
        private IExpression[] expressions;
        private string[] expressionStrings;

        private Game(List<IDot> dots, string[] expressionStrings)
        {
            this.dots = dots;
            this.expressionStrings = expressionStrings;

            this.expressions = new IExpression[MaxExpressionCount];
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
        /// Gets the dots contained in the game.
        /// </summary>
        public ReadOnlyCollection<IDot> Dots => new ReadOnlyCollection<IDot>(this.dots);

        /// <summary>
        /// Gets the textual representations contained in the game.
        /// </summary>
        public string[] ExpressionStrings => this.expressionStrings;

        /// <summary>
        /// Given a valid model, provides a game from it.
        /// </summary>
        /// <param name="model">The model to use as a  basis for the game.</param>
        /// <returns>A <see cref="IMaybe{T}"/> containing a game, if model is valid.</returns>
        public static IMaybe<Game> FromModel(GameModel model)
        {
            if (model.ExpressionStrings.Count != Game.MaxExpressionCount)
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
        public static Game NewRandom(int goodDotsNumber = 8, int badDotsNumber = 2)
        {
            var generator = new RandomDotsGenerator(goodDotsNumber, badDotsNumber);

            var game = new Game(generator.Generate(), new string[MaxExpressionCount]);

            return game;
        }

        /// <summary>
        /// Gets the <see cref="GameModel"/> representing this game.
        /// </summary>
        /// <returns>The model underlying this game.</returns>
        public GameModel GetModel()
        {
            GameModel gameModel = new GameModel();
            gameModel.ExpressionStrings = this.expressionStrings.ToList();
            gameModel.DotModels.AddRange(
                this.dots.Select(d => new DotModel()
                {
                    Kind = d.Kind,
                    X = d.Position.Item1,
                    Y = d.Position.Item2
                }));

            return gameModel;
        }
    }
}
