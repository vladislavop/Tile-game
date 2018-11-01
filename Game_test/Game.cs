using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_test {

    class Game {

        public Tile[][] tiles_array;
        public Tile currentTile;

        public Game() {
            tiles_array = new Tile[5][];
            InitBlocks();
            currentTile = tiles_array[0][0];
        }

        private void InitBlocks() {
            for (int i = 0; i < 5; i++) {
                tiles_array[i] = new Tile[5];
                for (int j = 0; j < 5; j++)
                    tiles_array[i][j] = new Tile(Tile.TileTypes.EMPTY, i, j);
            }
            for (int i = 0; i < 5; i += 2) {
                tiles_array[i][1].Type = tiles_array[i][3].Type = Tile.TileTypes.BLOCK;
            }
            RandomColorInit();
        }

        private void RandomColorInit() {
            int count_yellow, count_red, count_green;
            count_yellow = count_red = count_green = 5;
            Random random = new Random();
            int column, row;
            while (count_yellow != 0 || count_green != 0 || count_red != 0) {
                row = random.Next(0, 5);
                column = random.Next(0, 5);
                if (tiles_array[row][column].Type == Tile.TileTypes.EMPTY) {
                    if (count_yellow != 0) {
                        tiles_array[row][column].Type = Tile.TileTypes.YELLOW;
                        count_yellow--;
                    }
                    else if (count_green != 0) {
                        tiles_array[row][column].Type = Tile.TileTypes.GREEN;
                        count_green--;
                    }
                    else if (count_red != 0) {
                        tiles_array[row][column].Type = Tile.TileTypes.RED;
                        count_red--;
                    }
                }
            }
        }

        public Tile getTile(int row, int column) {
            return tiles_array[row][column];
        }

        public List<Tile> getNearestEmptyPosition() {
            int row = currentTile.Row;
            int column = currentTile.Column;
            List<Tile> empty_list = new List<Tile>();
            try {
                if (tiles_array[row + 1][column].Type == Tile.TileTypes.EMPTY) {
                    empty_list.Add(tiles_array[row + 1][column]);
                }
            }
            catch (Exception e) { }
            try {
                if (tiles_array[row - 1][column].Type == Tile.TileTypes.EMPTY) {
                    empty_list.Add(tiles_array[row - 1][column]);
                }
            }
            catch (Exception e) { }
            try {
                if (tiles_array[row][column + 1].Type == Tile.TileTypes.EMPTY) {
                    empty_list.Add(tiles_array[row][column + 1]);
                }
            }
            catch (Exception e) { }
            try {
                if (tiles_array[row][column - 1].Type == Tile.TileTypes.EMPTY) {
                    empty_list.Add(tiles_array[row][column - 1]);
                }
            }
            catch (Exception e) { }
            if (!empty_list.Any())
                return null;
            return empty_list;
        }

        public bool Finish() {
            for(int i = 0; i < 5; i += 2) {
                for(int j = 0; j < 4; j++) {
                    if (tiles_array[j][i].Type != tiles_array[j+1][i].Type)
                        return false;
                }   
            }
            return true;
        }
    }
}
