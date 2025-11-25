using System.Drawing;
using TagsCloudVisualization;

class CircularCloudLayouter
{
    private readonly List<Rectangle> _prevRectangles;
    private readonly IPointGenerator _pointGenerator;
    private Point _center;
    public CircularCloudLayouter(Point center, IPointGenerator generator)
    {
        _pointGenerator = generator;
        _prevRectangles = new List<Rectangle>();
        this._center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        foreach (var nextRectangleCenter in _pointGenerator.GetNextPoint())
        {
            var nextRectPos = nextRectangleCenter - rectangleSize / 2;
            var nextRect = new Rectangle(nextRectPos,  rectangleSize);
            if (_prevRectangles.Any(x => x.IntersectsWith(nextRect)))
                continue;
            _prevRectangles.Add(nextRect);
            return nextRect;
        }
        throw new Exception("No more space available");
    }
}