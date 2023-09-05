using System.Runtime;

namespace battleship_net {
    public class Renderer {
        private const int _cellWidth =2;
        private readonly int _colHeader;
        private readonly int _width;
        private readonly int _height;
        private readonly Coordinate _playerBoardOffset;
        private readonly Coordinate _targetBoardOffset;
        private readonly int _promptY;

        public Renderer(int width, int height, Coordinate boardOffset)
        {
            _width =width;
            _height = height;
            _colHeader = _height > 10 ? 4 : 3;
            _playerBoardOffset = boardOffset;

            var playerBoardWidth = (_width * _cellWidth) + 1;
            var playerBoardRight = playerBoardWidth + _colHeader + _playerBoardOffset.X;
            var boardMargin = 3;
            _targetBoardOffset = _playerBoardOffset with {X = playerBoardRight + boardMargin};
            _promptY = height + 5;

            Console.CursorVisible = false;
        }
        public void RefreshPlayerBoard(Player player){
            RenderPlayerBoard(player);

            foreach(var ship in player.Ships){
                RenderShip(ship.Item1, ship.Item2);
            }
        }

        public void RefreshTargetBoard(Player player){
            RenderTargetBoard();

            foreach(var target in player.Targets){
                RenderTargetCell(target.Item1, target.Item2);
            }
        }

        public void RenderPlayerBoard(Player player)
        {
          RenderBoard(_playerBoardOffset, player.Name);
        }

        public void RenderTargetBoard()
        {
           RenderBoard(_targetBoardOffset, "Target");
        }

        private void RenderBoard(Coordinate offset, string title)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(offset.X + _colHeader, offset.Y);
            
            var pad = ((_width * _cellWidth) - title.Length) /2;
            var pl = title.PadLeft(pad + title.Length);
            var pr = pl.PadRight(_width * 2);
            Console.Write(pr);
            for (int i=0; i< _height; i++)
            {
                RenderCellRow(offset, i);
            }
            RenderHeaderRow(offset);
        }
        private void RenderHeaderRow(Coordinate offset){
            var cells = Enumerable.Range('a', _width).Select(c => (char)c);
            var row = string.Join(' ', cells);
            //More magic number
            var yOffset = offset.Y + _height + 1;
            Console.SetCursorPosition(offset.X + _colHeader, yOffset);
            Console.Write($"{row}");
        }

        private void RenderCellRow(Coordinate offset, int rowNumber){
            var cells = Enumerable.Repeat('_', _width);
            var row = string.Join('|', cells);
            //Todo: eliminate Magic number 1
            Console.SetCursorPosition(offset.X, offset.Y + 1 + rowNumber);
            Console.Write($"{rowNumber.ToString(_height > 10 ? "d2" : "d1")} |{row}|");
        }

        public void RenderShip(Ship ship, Position position, bool isValid = true) 
        {
            var cellX = _playerBoardOffset.X + _colHeader + (position.Col *_cellWidth);
            //Magic number 1
            var cellY = _playerBoardOffset.Y + position.Row + 1;

            Console.BackgroundColor = isValid ? ConsoleColor.DarkGreen : ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;

            for(int i= 0; i < ship.Size; i++) {

                if (position.Orientation == Orientation.Vertical) {
                    Console.SetCursorPosition(cellX, cellY+i);
                }
                else{ 
                    Console.SetCursorPosition(cellX + (i * _cellWidth), cellY);
                }
                Console.Write(ship.Name[0]);
            }

            Console.ResetColor();
        }

        private void RenderTargetCell(Coordinate coordinate, bool isHit) {
            Console.SetCursorPosition(_targetBoardOffset.X + (coordinate.X * _cellWidth) + _colHeader, _targetBoardOffset.Y + coordinate.Y);

            Console.BackgroundColor = isHit ? ConsoleColor.Red : ConsoleColor.Cyan;
            Console.Write('_');
            Console.ResetColor();
        }

        public void RenderPrompt(string message)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, _promptY);
            Console.Write(message.PadRight(80));
            Console.SetCursorPosition(message.Length + 1, _promptY);
        }
    }
}