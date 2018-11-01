using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_test {

    class Tile {
        public enum TileTypes {
            YELLOW = 0,
            RED = 1,
            GREEN = 2,
            BLOCK = 3,
            EMPTY = 4
        }

        private int column, row;

        public int Column {
            set {
                column = value;
            }
            get {
                return column;
            }
        }

        public int Row {
            set {
                row = value;
            }
            get {
                return row;
            }
        }

        private TileTypes type;
        public Tile(TileTypes type, int row, int column) {
            this.row = row;
            this.column = column;
            this.type = type;
        }

        public TileTypes Type {
            get {
                return type;
            }
            set {
                type = value;
            }
        }


    }
}
