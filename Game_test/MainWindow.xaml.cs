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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game_test {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Rectangle[][] cubes = new Rectangle[5][];
        Game game;
        private bool isPressed = false;
        private bool first_init = true;


        public MainWindow() {
            InitializeComponent();
            Instruction.Text = "Используйте клиавиши W(вверх) S(вниз)\nA(влево) D(вправо) для перемещения.\n" +
                "Для того чтобы переместить блок нужно\nнажать ПРОБЕЛ и затем нажать на\nклавишу перемещения.";
            game = new Game();
            refreshGameField();

        }

        #region Initialization of game's field
        public void refreshGameField() {
            for (int i = 0; i < 5; i++) {
                if (first_init)
                    cubes[i] = new Rectangle[5];
                for (int j = 0; j < 5; j++) {
                    if (first_init)
                        cubes[i][j] = new Rectangle();
                    cubes[i][j].Width = cubes[i][j].Height = 70;
                    cubes[i][j].RadiusX = 10;
                    cubes[i][j].RadiusY = 10;
                    cubes[i][j].Stroke = null;
                    switch (game.getTile(i, j).Type) {
                        case Tile.TileTypes.YELLOW:
                            cubes[i][j].Fill = new SolidColorBrush(Colors.Yellow);
                            break;
                        case Tile.TileTypes.GREEN:
                            cubes[i][j].Fill = new SolidColorBrush(Colors.Green);
                            break;
                        case Tile.TileTypes.RED:
                            cubes[i][j].Fill = new SolidColorBrush(Colors.Red);
                            break;
                        case Tile.TileTypes.BLOCK:
                            cubes[i][j].Fill = new ImageBrush {
                                ImageSource = new BitmapImage(new Uri("brake.jpg", UriKind.Relative))
                            };
                            break;
                        case Tile.TileTypes.EMPTY:
                            cubes[i][j].Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1f1f2e"));
                            break;
                    }
                    if (first_init) {
                        Grid.SetRow(cubes[i][j], i);
                        Grid.SetColumn(cubes[i][j], j);
                        game_field.Children.Add(cubes[i][j]);
                    }

                    //kek.Text += game.getTile(i, j).Type.ToString() + " ";
                }
                //kek.Text += "\n";
            }
            if (first_init)
                first_init = false;
            ReloadCurrent();
            isPressed = false;
            showNearestEmpty();
            if (game.Finish()) {
                MessageBoxResult result = MessageBox.Show("Вы выйграли!!! Хотите начать занаво?",
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes) {
                    RestartGame();
                }
                return;
            }
        }
        #endregion

        private void ReloadCurrent() {
            int temp_row = game.currentTile.Row;
            int temp_column = game.currentTile.Column;
            cubes[temp_row][temp_column].Stroke = new SolidColorBrush(Colors.Black);
            cubes[temp_row][temp_column].StrokeThickness = 6;
        }

        List<Tile> empty_list;

        private void showNearestEmpty() {
            empty_list = game.getNearestEmptyPosition();
            if (empty_list == null)
                return;
        }

        private Tile CheckNearestPosition(int row, int column) {
            if (empty_list == null)
                return null;
            foreach (Tile i in empty_list) {
                if (i.Row == row && i.Column == column)
                    return i;
            }
            return null;
        }

        private void setCurrent(int row, int column) {
            game.currentTile = game.tiles_array[row][column];
        }

        private void UpHandler() {
            if (game.currentTile.Row == 0)
                return;
            if (game.getTile(game.currentTile.Row - 1, game.currentTile.Column).Type == Tile.TileTypes.BLOCK)
                return;
            Move(game.currentTile.Row - 1, game.currentTile.Column);
        }

        private void Move(int row, int column) {
            if (isPressed) {
                Tile temp_tile = CheckNearestPosition(row, column);
                if (empty_list != null && temp_tile != null) {
                    temp_tile.Type = game.currentTile.Type;
                    game.currentTile.Type = Tile.TileTypes.EMPTY;

                }
                isPressed = false;
            }
            setCurrent(row, column);
            refreshGameField();
        }

        private void DownHandler() {

            if (game.currentTile.Row == 4)
                return;
            if (game.getTile(game.currentTile.Row + 1, game.currentTile.Column).Type == Tile.TileTypes.BLOCK)
                return;
            Move(game.currentTile.Row + 1, game.currentTile.Column);
        }

        private void LeftHandler() {
            if (game.currentTile.Column == 0)
                return;
            if (game.getTile(game.currentTile.Row, game.currentTile.Column - 1).Type == Tile.TileTypes.BLOCK)
                return;
            Move(game.currentTile.Row, game.currentTile.Column - 1);
        }

        private void RightHandler() {
            if (game.currentTile.Column == 4)
                return;
            if (game.getTile(game.currentTile.Row, game.currentTile.Column + 1).Type == Tile.TileTypes.BLOCK)
                return;
            Move(game.currentTile.Row, game.currentTile.Column + 1);
        }

        private void SpaceHandler() {
            if (game.currentTile.Type != Tile.TileTypes.EMPTY) {
                if (isPressed) {
                    isPressed = false;
                    refreshGameField();
                }
                else {
                    isPressed = true;
                    int row = game.currentTile.Row;
                    int column = game.currentTile.Column;
                    cubes[row][column].Stroke = new SolidColorBrush(Colors.Pink);
                }
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.W:
                    UpHandler();
                    break;
                case Key.S:
                    DownHandler();
                    break;
                case Key.A:
                    LeftHandler();
                    break;
                case Key.D:
                    RightHandler();
                    break;
                case Key.Space:
                    SpaceHandler();
                    break;
            }

        }

        private void RestartGame() {
            for (int i = 0; i < cubes.Length; i++) {
                for (int j = 0; j < cubes[i].Length; j++) {
                    game_field.Children.Remove(cubes[i][j]);
                }
            }

            game = new Game();
            cubes = new Rectangle[5][];
            first_init = true;
            refreshGameField();
        }

        private void Restart(object sender, RoutedEventArgs e) {
            RestartGame();
        }
    }
}