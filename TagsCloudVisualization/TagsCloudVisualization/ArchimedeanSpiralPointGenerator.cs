using System.Drawing;

namespace TagsCloudVisualization;

public class ArchimedeanSpiralPointGenerator(Point start, float density) : IPointGenerator
{
    private Point _currentPoint = start;
    private float _step = 0f;

    public IEnumerable<Point> GetNextPoint()
    {
        while (true)
        {
            yield return _currentPoint;
            var newX = _currentPoint.X + density * _step * (float)Math.Cos(_step);
            var newY = _currentPoint.Y + density * _step * (float)Math.Sin(_step);
            _currentPoint = Point.Round(new PointF(newX, newY));
            _step += 0.1f;
        }
    }
}