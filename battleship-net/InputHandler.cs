using System.Text.RegularExpressions;

namespace battleship_net {
        public enum InputCommand {
            NoOp,
            MoveLeft,
            MoveRight,
            MoveUp,
            MoveDown,
            SelectRow,
            SelectCol,
            Rotate,
            Accept
        }

    public class InputHandler {

    public (InputCommand command, int? data) GetInput(){
        var command = InputCommand.NoOp;
        int? data = null;

        while (command == InputCommand.NoOp) {
            var input = Console.ReadKey();
            (command, data) = HandleInput(input.Key);
        }

        return (command, data);
    }

    private (InputCommand command, int? data) HandleInput(ConsoleKey key) =>
        key switch {
            ConsoleKey.Tab => (InputCommand.Rotate, null),
            ConsoleKey.Enter => (InputCommand.Accept, null),
            ConsoleKey.UpArrow => (InputCommand.MoveUp, null),
            ConsoleKey.DownArrow => (InputCommand.MoveDown, null),
            ConsoleKey.LeftArrow => (InputCommand.MoveLeft, null),
            ConsoleKey.RightArrow => (InputCommand.MoveRight, null),
            >= ConsoleKey.D0 and <= ConsoleKey.D9 => (InputCommand.SelectRow, key - ConsoleKey.A),
            >= ConsoleKey.A and <= ConsoleKey.Z => (InputCommand.SelectCol, key - ConsoleKey.D0),
            _ => (InputCommand.NoOp, null)
        };
    }
}