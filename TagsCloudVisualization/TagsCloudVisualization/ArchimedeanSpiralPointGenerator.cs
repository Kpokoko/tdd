using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralPointGenerator(Point start, float density) : IPointGenerator
{
    private Point _start = start;
    private float _step = 0f;

    public IEnumerable<Point> GetNextPoint()
    {
        yield return _start;
        while (true)
        {
            var newX = _start.X + density * _step * (float)Math.Cos(_step);
            var newY = _start.Y + density * _step * (float)Math.Sin(_step);
            yield return Point.Round(new PointF(newX, newY));
            _step += 0.01f;
        }
    }
}