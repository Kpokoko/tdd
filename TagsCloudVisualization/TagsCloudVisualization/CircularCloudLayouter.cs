using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> _prevRectangles;
    private readonly IPointGenerator _pointGenerator;
    private const int Density = 1;
    private Point _center;
    private bool isEnumerating;
    public CircularCloudLayouter(Point center)
    {
        _pointGenerator = new ArchimedeanSpiralPointGenerator(center, Density);
        _prevRectangles = new List<Rectangle>();
        this._center = center;
    }

    public IEnumerable<Rectangle> GetLayout()
    {
        isEnumerating = true;
        foreach (var rectangle in _prevRectangles)
            yield return rectangle;
        isEnumerating = false;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (isEnumerating)
            throw new InvalidOperationException("Can't put next rectangle while enumeration is running");
        foreach (var nextRectangleCenter in _pointGenerator.GetNextPoint())
        {
            var nextRectanglePos = nextRectangleCenter - rectangleSize / 2;
            var nextRect = new Rectangle(Point.Round(nextRectanglePos), rectangleSize);
            if (_prevRectangles.Any(x => x.IntersectsWith(nextRect)))
                continue;
            var pressedNextRect = new Rectangle(PressRectangleToCenter(nextRect), rectangleSize);
            _prevRectangles.Add(pressedNextRect);
            return pressedNextRect;
        }
        throw new Exception("No more space available");
    }

    public Point PressRectangleToCenter(Rectangle rectangle)
    {
        var rectPos = rectangle.Location;
        while (true)
        {
            var rectCenter = new Point(
                rectPos.X + rectangle.Width / 2,
                rectPos.Y + rectangle.Height / 2);
            var direction = _center.Subtract(rectCenter);
            if (direction == Point.Empty)
                return rectPos;
            var nextPos = rectPos.Add(direction);
            var nextRect = new Rectangle(nextPos, rectangle.Size);
            if (_prevRectangles.Any(x => x.IntersectsWith(nextRect)))
                return rectPos;
            rectPos = nextPos;
        }
    }
}