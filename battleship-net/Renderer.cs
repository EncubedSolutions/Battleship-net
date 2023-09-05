using System.Runtime;

namespace battleship_net {
    public class Renderer {

        private const int colHeader = 3;
        private readonly int _width;
        private readonly int _height;
        private readonly Coordinate _playerBoardOffset;
        private readonly Coordinate _targetBoardOffset;
        private readonly int _promptY;

        public Renderer(int width, int height, Coordinate boardOffset, Coordinate targetBoardOffset)
        {
            _width =width;
            _height = height;
            _playerBoardOffset = boardOffset;
            _targetBoardOffset = targetBoardOffset;
            _promptY = height + 5;

            Console.CursorVisible = false;
        }
        public void RefreshPlayerBoard(Player player){
            RenderPlayerBoard();

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

        public void RenderPlayerBoard()
        {
          RenderBoard(_playerBoardOffset);
        }

        public void RenderTargetBoard()
        {
           RenderBoard(_targetBoardOffset);
        }

        private void RenderBoard(Coordinate offset)
        {
            Console.CursorVisible = false;
            for (int i=0; i< _height; i++)
            {
             Console.SetCursorPosition(offset.X, offset.Y);
                RenderCellRow(i);
            }
            RenderHeaderRow();
        }
        private void RenderHeaderRow(){
            var cells = Enumerable.Range('a', _width).Select(c => (char)c);
            var row = string.Join(' ', cells);
            var yOffset = _playerBoardOffset.Y + _height;
            Console.SetCursorPosition(_playerBoardOffset.X + colHeader, yOffset);
            Console.Write($"{row}");
        }

        private void RenderCellRow(int rowNumber){
            var cells = Enumerable.Repeat('_', _width);
            var row = string.Join('|', cells);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + rowNumber);
            Console.Write($"{rowNumber} |{row}|");
        }

        public void RenderShip(Ship ship, Position position, bool isValid = true) 
        {
            var cellX = _playerBoardOffset.X + colHeader + (position.Col *2);
            var cellY = _playerBoardOffset.Y + position.Row;

            Console.BackgroundColor = isValid ? ConsoleColor.DarkGreen : ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.White;

            for(int i= 0; i < ship.Size; i++) {

                if (position.Orientation == Orientation.Vertical) {
                    Console.SetCursorPosition(cellX, cellY+i);
                }
                else{ 
                    Console.SetCursorPosition(cellX + (i * 2), cellY);
                }
                Console.Write(ship.Name[0]);
            }

            Console.ResetColor();
        }

        private void RenderTargetCell(Coordinate coordinate, bool isHit) {
            //TODO: This doesn't work as expected
            Console.SetCursorPosition(_targetBoardOffset.X + (coordinate.X * 2)+ colHeader, _targetBoardOffset.Y + coordinate.Y);

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