using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Persistence.Models;

namespace Backend.BusinessLogic
{
    /// <summary>
    /// A domain model representing a game, implementing validation logic.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The maximum number of expressions possible in a game.
        /// </summary>
        public const int MaxExpressionCount = 5;
        private const string PrivateKey = "aa9442f85ecaf18816b0f4ea1d561538109e6fa37171a545ed81b22b463bb375";

        private Guid id;
        private string signature;
        private readonly List<IDot> dots;
        private readonly string[] expressionStrings;

        private Game(List<IDot> dots, string[] expressionStrings, Guid? id = null)
        {
            this.dots = dots;
            this.expressionStrings = expressionStrings;
            this.id = id ?? Guid.NewGuid();

            this.FillSignature();
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
        /// Gets the textual representation of the game id.
        /// </summary>
        public string Id => this.id.ToString("N");

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
                model.DotModels
                    .Select(dm => new Dot(dm) as IDot)
                    .ToList(),
                model.ExpressionStrings.ToArray(),
                model.Id);

            return game.signature.ToLowerInvariant() == model.Signature.ToLowerInvariant()
                ? new Some<Game>(game) as IMaybe<Game>
                : new None<Game>() as IMaybe<Game>;
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

            var game = new Game(generator.Generate(), new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, });

            return game;
        }

        /// <summary>
        /// Gets the <see cref="GameModel"/> representing this game.
        /// </summary>
        /// <returns>The model underlying this game.</returns>
        public GameModel GetModel()
        {
            GameModel gameModel = new GameModel()
            {
                Id = this.id,
                Signature = this.signature,
            };
            gameModel.ExpressionStrings = this.expressionStrings.ToList();
            gameModel.DotModels.AddRange(
                this.dots.Select(d => d.GetModel()));

            return gameModel;
        }

        private void FillSignature()
        {
            var bytes = Encoding.UTF8.GetBytes(Game.PrivateKey).ToList();
            bytes.AddRange(this.id.ToByteArray());

            foreach (var dot in this.dots)
            {
                bytes.AddRange(Encoding.UTF8.GetBytes(dot.Kind.ToString()));
                bytes.AddRange(BitConverter.GetBytes(dot.Position.Item1));
                bytes.AddRange(BitConverter.GetBytes(dot.Position.Item2));
            }

            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes.ToArray());

            this.signature = BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
