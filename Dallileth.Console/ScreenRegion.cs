using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dallileth.Sandbox
{
    public class ScreenObject : IDisposable
    {
        ScreenRegion _region;
        public ScreenObject(char c, XY? xy, ScreenRegion region)
        {
            _region = region;
            _c = c;
            XY = xy;
        }
        public void Dispose()
        {
            _xy = null;
        }
        XY? _xy; //todo: change to XY, and add IsInvisible flag
        char _c;
        public XY? XY
        {
            get => _xy;

            set
            {
                if (_xy != null)
                    _region.Write(_xy.Value.X, _xy.Value.Y, null);
                _xy = value;
                if (_xy != null)
                {
                    _region.Write(_xy.Value.X, _xy.Value.Y, _c);
                }
            }
        }

    }
    public struct XY
    {
        public int X { get; init; }
        public int Y { get; init; }
        public XY(int x, int y) { X = x; Y = y; }

        public static XY operator +(XY c1, XY c2)
        {
            return new XY(c1.X + c2.X, c1.Y + c2.Y);
        }
        public static XY operator +(XY c1, Direction direction)
        {
            return c1 + direction switch
            {
                Direction.Up => new XY(0, -1),
                Direction.Right => new XY(1, 0),
                Direction.Down => new XY(0, 1),
                Direction.Left => new XY(-1, 0),
                _ => throw new NotImplementedException($"{direction} not implemented")
            };
        }
    }

    public enum Direction { Up, Down, Left, Right };
    public class ScreenRegion
    {
        char _default_text = ' ';
        public ScreenRegion(int x, int y, int w, int h, char default_text = '.')
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            _default_text = default_text;

        }

        public void Write(int x, int y, char? c)
        {
            if (0 <= x && x < W && 0 <= y && y < H)
            {
                Console.SetCursorPosition(x + X, y + Y);
                Console.Write(c ?? _default_text);

                Console.SetCursorPosition(x+X,y+Y);
                System.Threading.Thread.Sleep(1);
            }
        }

        public void Clear(char? c = null)
        {
            string empty = new string(c ?? _default_text, W);
            Console.SetCursorPosition(0, Y);
            for (int i = 0; i < H; i++)
            {
                Console.WriteLine(empty);
            }
        }
        public int X { get; init; }
        public int Y { get; init; }
        public int W { get; init; }
        public int H { get; init; }
    }

}
