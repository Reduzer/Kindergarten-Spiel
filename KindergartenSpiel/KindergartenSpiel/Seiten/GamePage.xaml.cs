using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KindergartenSpiel.Seiten
{
    /// <summary>
    /// Interaktionslogik für GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        // Globale Spielvariablen
        private string _targetShapeName;
        private int _score = 0;
        private DispatcherTimer _memoryTimer;
        private DispatcherTimer _spawnTimer;
        private DispatcherTimer _crossTimer;
        private Random _random = new Random();
        private List<string> _availableShapes = new List<string> { "Kreis", "Quadrat", "Dreieck" };


        public GamePage()
        {
            InitializeComponent();


            _memoryTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _memoryTimer.Tick += MemoryTimer_Tick;


            _spawnTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _spawnTimer.Tick += SpawnTimer_Tick;

            _crossTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _crossTimer.Tick += CrossTimer_Tick;


            ScoreLabel.Text = _score.ToString();
            StatusLabel.Text = "Klicke auf Play!";
        }

        private void CrossTimer_Tick(object sender, EventArgs e)
        {
            _crossTimer.Stop();
            // Blendet das rote Kreuz aus
            RedCrossOverlay.Visibility = Visibility.Collapsed;
        }


        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            _score = 0;
            ScoreLabel.Text = _score.ToString();

            GameCanvas.Children.Clear();
            StartButton.IsEnabled = false;
            StartMemoryPhase();
        }

        private void StartMemoryPhase()
        {
            _targetShapeName = _availableShapes[_random.Next(_availableShapes.Count)];


            Shape targetShape = CreateShape(_targetShapeName);

            targetShape.Width = 100;
            targetShape.Height = 100;


            if (targetShape is Polygon poly)
            {

                poly.Points = new PointCollection
                {
                    new Point(50, 0),
                    new Point(100, 100),
                    new Point(0, 100)
                };
            }

            TargetShapeText.Content = targetShape;
            TargetShapeDisplay.Visibility = Visibility.Visible;
            _memoryTimer.Start();
        }

        private void MemoryTimer_Tick(object sender, EventArgs e)
        {
            _memoryTimer.Stop();


            TargetShapeDisplay.Visibility = Visibility.Collapsed;
            TargetShapeText.Content = null;

            StartGamePhase();
        }

        private void StartGamePhase()
        {
            StatusLabel.Text = $"Klicken Sie auf die Form: {_targetShapeName}";

            ClearFallingShapes();

            _spawnTimer.Start();
        }

        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            SpawnRandomShape();
        }

        private void SpawnRandomShape()
        {
            // Wählt ne zufällige Form aus
            string type = _availableShapes[_random.Next(_availableShapes.Count)];
            SpawnShape(type);
        }

        private void ClearFallingShapes()
        {

            var shapesToRemove = GameCanvas.Children.OfType<Shape>().ToList();
            foreach (var shape in shapesToRemove)
            {
                GameCanvas.Children.Remove(shape);
            }
        }

        private void GameOver()
        {
            _memoryTimer.Stop();
            _spawnTimer.Stop();

            StatusLabel.Text = $"Spiel vorbei! Score: {_score}";

            ClearFallingShapes();

            RedCrossOverlay.Visibility = Visibility.Visible;
            _crossTimer.Start();

            StartButton.IsEnabled = true;
        }

        //Erstellung einer Form
        private Shape CreateShape(string type)
        {
            Shape shape;
            SolidColorBrush fillBrush;

            switch (type)
            {
                case "Kreis":
                    fillBrush = Brushes.Yellow;
                    shape = new Ellipse
                    {
                        Width = 80,
                        Height = 80,
                        Fill = fillBrush,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3
                    };
                    break;

                case "Quadrat":
                    fillBrush = Brushes.Green;
                    shape = new Rectangle
                    {
                        Width = 80,
                        Height = 80,
                        Fill = fillBrush,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3
                    };
                    break;

                case "Dreieck":
                    fillBrush = Brushes.Red;
                    shape = new Polygon
                    {
                        Points = new PointCollection
                        {
                            new Point(40,0),
                            new Point(80,80),
                            new Point(0,80)
                        },
                        Fill = fillBrush,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3
                    };
                    break;

                default:
                    throw new ArgumentException($"Unbekannter Formtyp: {type}");
            }

            // Name der Form speichern
            shape.Tag = type;
            return shape;
        }



        private void SpawnShape(string type)
        {
            Shape shape = CreateShape(type);

            // zufällige Startposition
            double x = _random.Next(0, (int)(GameCanvas.ActualWidth - 80));
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, -100);

            shape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;

            GameCanvas.Children.Add(shape);

            StartFallAnimation(shape);
        }

        private void StartFallAnimation(UIElement shape)
        {
            double canvasHeight = GameCanvas.ActualHeight;

            var anim = new DoubleAnimation
            {
                From = -10,
                To = canvasHeight + 100,
                Duration = TimeSpan.FromSeconds(4),
                FillBehavior = FillBehavior.Stop
            };

            anim.Completed += (s, e) =>
            {

                if (GameCanvas.Children.Contains(shape))
                {
                    GameCanvas.Children.Remove(shape);
                }
            };

            shape.BeginAnimation(Canvas.TopProperty, anim);
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!_spawnTimer.IsEnabled)
            {
                return;
            }

            Shape clickedShape = sender as Shape;

            if (clickedShape == null) return;


            GameCanvas.Children.Remove(clickedShape);

            string clickedShapeName = clickedShape.Tag?.ToString();

            if (clickedShapeName == _targetShapeName)
            {
                // Richtig
                _score++;
                ScoreLabel.Text = _score.ToString();
                StatusLabel.Text = $"Richtig! Weiter so. (Ziel: {_targetShapeName})";
            }
            else
            {
                // Falsch
                StatusLabel.Text = $"Falsch geklickt! Du hast {clickedShapeName} statt {_targetShapeName} gewählt.";
                GameOver();
            }
        }
    }
}

