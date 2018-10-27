using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Backend.BusinessLogic;
using Backend.BusinessLogic.Expressions;
using Persistence;
using Persistence.Models;

namespace Backend.Controller
{
    /// <summary>
    /// Controls the business logic on behalf of the view.
    /// </summary>
    public class PollyController
    {
        /// <summary>
        /// Dimensions of the coordinate system in business logical units.
        /// </summary>
        private const float StartX = -11f;
        private const float EndX = 11f;
        private const float StartY = PollyController.StartX;
        private const float EndY = PollyController.EndX;
        private const float TickInterval = 1f;

        /// <summary>
        /// When exceeding this absolute limit in terms of y-value,
        /// no attempt at using the point is made
        /// </summary>
        private const float Limits = 1000f;

        private readonly CoordinateSystemInfo coordinateSystemInfo = new CoordinateSystemInfo()
        {
            StartX = PollyController.StartX,
            EndX = PollyController.EndX,
            StartY = PollyController.StartY,
            EndY = PollyController.EndY,
            TickInterval = PollyController.TickInterval,
        };

        /// <summary>
        /// The game currently in progress.
        /// </summary>
        private Game game;

        /// <summary>
        /// The game repository for obtaining persisted games.
        /// </summary>
        private IGameRepository gameRepository;

        /// <summary>
        /// The parser for textual representations of <see cref="IExpression"/>.
        /// </summary>
        private Parser parser;

        /// <summary>
        /// The expression currently active.
        /// </summary>
        private IExpression[] expressions;

        /// <summary>
        /// The lists of points.
        /// </summary>
        private List<ListPointLogical>[] points;

        /// <summary>
        /// The list of drawable dots.
        /// </summary>
        private List<IDrawDot> drawDots;

        /// <summary>
        /// The current score.
        /// </summary>
        private int score = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PollyController"/> class.
        /// </summary>
        /// <param name="game">A game that shall be respected by the <see cref="PollyController"/> instance.</param>
        /// <param name="gameRepository">The game repository to be used, default used if null.</param>
        public PollyController(Game game = null, IGameRepository gameRepository = null)
        {
            this.gameRepository = gameRepository ?? new OnDiskGameRepository();
            this.parser = new Parser();

            this.ClearInput();
            this.game = game ?? Game.NewRandom();

            this.UpdateData();
        }

        /// <summary>
        /// Gets the information as needed for the drawing of the coordinate system.
        /// </summary>
        public CoordinateSystemInfo CoordinateSystemInfo => this.coordinateSystemInfo;

        /// <summary>
        /// Gets the current score.
        /// </summary>
        public int Score => this.score;

        /// <summary>
        /// Gets the maximum number of expressions handled by this controller.
        /// </summary>
        public int MaxExpressionCount => Game.MaxExpressionCount;

        /// <summary>
        /// Gets the textual representations of expressions contained.
        /// </summary>
        public ReadOnlyCollection<string> ExpressionStrings => new ReadOnlyCollection<string>(this.game.ExpressionStrings);

        /// <summary>
        /// Gets the drawable dots.
        /// </summary>
        public ReadOnlyCollection<IDrawDot> DrawDots => new ReadOnlyCollection<IDrawDot>(this.drawDots ?? new List<IDrawDot>(0));

        /// <summary>
        /// Initialize a new random game.
        /// </summary>
        public void NewRandomGame()
        {
            this.ClearInput();
            this.game = Game.NewRandom();

            this.UpdateData();
        }

        /// <summary>
        /// Save the currently held game under the given path.
        /// </summary>
        /// <param name="path">The path to save/overwrite in.</param>
        public void SaveGame(string path)
        {
            var gameModel = this.game.GetModel();
            this.gameRepository.SaveGame(gameModel, path);
        }

        /// <summary>
        /// Load game from the given path.
        /// </summary>
        /// <param name="path">The path to load from.</param>
        public void LoadGame(string path)
        {
            this.ClearInput();

            var model = this.gameRepository.LoadGame(path);
            var maybeGame = Game.FromModel(model);

            if (maybeGame.HasValue)
            {
                this.game = maybeGame.Value;
            }
            else
            {
                throw new InvalidOperationException($"{path} did not contain a valid game");
            }

            this.LoadStringExpressionsFromGame();

            this.UpdateData();
        }

        /// <summary>
        /// Tests the expression for parseability.
        /// </summary>
        /// <param name="textRepresentation">The textual representation of the expression.</param>
        /// <returns><c>true</c> if string is parseable.</returns>
        public bool TestExpression(string textRepresentation)
        {
            return this.parser.IsParseable(textRepresentation);
        }

        /// <summary>
        /// Updates the expression at the index and triggers the updating of the resulting data.
        /// </summary>
        /// <param name="index">The index to be updated.</param>
        /// <param name="textRepresentation">The textual representation of the expression.</param>
        public void SetExpressionAtIndex(int index, string textRepresentation)
        {
            this.game.ExpressionStrings[index] = string.IsNullOrWhiteSpace(textRepresentation) ? string.Empty : textRepresentation;
            this.expressions[index] = string.IsNullOrWhiteSpace(textRepresentation) ? null : this.parser.Parse(textRepresentation);
        }

        /// <summary>
        /// Get the lists of plottable points in terms of business logic units.
        /// </summary>
        /// <param name="index">The index of the internally held <see cref="IExpression"/>.</param>
        /// <returns>A list of point list, in terms of business logic units.</returns>
        public List<ListPointLogical> GetListsOfLogicalPointsByIndex(int index)
        {
            if (!this.expressions[index].IsNullOrInvalidExpression())
            {
                return this.points[index];
            }
            else
            {
                return new List<ListPointLogical>(0);
            }
        }

        /// <summary>
        /// Drives the updating process by delegating to other private methods.
        /// </summary>
        public void UpdateData()
        {
            this.UpdateGraphs();
            this.UpdateDotsAndScore();
        }

        private void UpdateGraphs()
        {
            this.points = new List<ListPointLogical>[Game.MaxExpressionCount];

            for (int expressionIndex = 0; expressionIndex < Game.MaxExpressionCount; ++expressionIndex)
            {
                if (!this.expressions[expressionIndex].IsNullOrInvalidExpression())
                {
                    PointListGenerator pointListGenerator = new PointListGenerator(this.expressions[expressionIndex], PollyController.StartX, PollyController.EndX, PollyController.Limits);
                    this.points[expressionIndex] = pointListGenerator.ObtainListsOfLogicalPoints();
                }
                else
                {
                    this.points[expressionIndex] = new List<ListPointLogical>(0);
                }
            }
        }

        private void UpdateDotsAndScore()
        {
            this.drawDots = new List<IDrawDot>(this.game.Dots.Count);
            List<int> numbersOfGoodHits = new List<int>(Game.MaxExpressionCount);
            List<int> numbersOfBadHits = new List<int>(Game.MaxExpressionCount);

            foreach (var dot in this.game.Dots)
            {
                this.drawDots.Add(new DrawDot(dot.Position.Item1, dot.Position.Item2, dot.Radius, dot.Kind));
            }

            for (int expressionIndex = 0; expressionIndex < Game.MaxExpressionCount; ++expressionIndex)
            {
                var expression = this.expressions[expressionIndex];

                if (!expression.IsNullOrInvalidExpression())
                {
                    int countOfGoodHits = 0;
                    int countOfBadHits = 0;
                    for (int dotIndex = 0; dotIndex < this.drawDots.Count; ++dotIndex)
                    {
                        DrawDot drawDot = this.drawDots[dotIndex] as DrawDot;
                        if (!drawDot.IsHit)
                        {
                            bool isHit = this.game.Dots[dotIndex].IsHit(expression, this.points[expressionIndex]);
                            if (isHit)
                            {
                                drawDot.IsHit = true;
                                if (this.game.Dots[dotIndex].Kind == DotKind.Good)
                                {
                                    ++countOfGoodHits;
                                }
                                else
                                {
                                    ++countOfBadHits;
                                }
                            }
                        }
                    }

                    numbersOfGoodHits.Add(countOfGoodHits);
                    numbersOfBadHits.Add(countOfBadHits);
                }
            }

            this.score = ScoreCalculator.CalculateScore(numbersOfGoodHits, numbersOfBadHits);
        }

        private void ClearInput()
        {
            this.expressions = new IExpression[Game.MaxExpressionCount];
            this.points = new List<ListPointLogical>[Game.MaxExpressionCount];
        }

        private void LoadStringExpressionsFromGame()
        {
            for (int i = 0; i < this.game.ExpressionStrings.Length; i++)
            {
                this.SetExpressionAtIndex(i, this.game.ExpressionStrings[i] ?? string.Empty);
            }
        }
    }
}
