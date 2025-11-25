using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization;

[TestFixture]
public class Tests
{
    private Point _center;
    private Size _size;
    private IPointGenerator _pointGenerator;
    
    [SetUp]
    public void Setup()
    {
        _center = new Point(0, 0);
        _size = new Size(10, 10);
        var density = 1;
        _pointGenerator = new ArchimedeanSpiralPointGenerator(_center, density);
    }
    
    [Test]
    public void CircularCloudLayout_ShouldCorrectlyPlaceRectangle_OnCenterPoint()
    {
        var layouter = new CircularCloudLayouter(_center, _pointGenerator);
        var expected = new Rectangle(_center - _size / 2, _size);

        var result = layouter.PutNextRectangle(_size);
        
        expected.Should().BeEquivalentTo(result);
    }

    [Test]
    public void CircularCloudLayout_ShouldCorrectlyPlace_SecondRectangle()
    {
        var layouter = new CircularCloudLayouter(_center, _pointGenerator);
        var expected = new Rectangle(_center + _size, _size);
        
        var firstRect = layouter.PutNextRectangle(_size);
        var result = layouter.PutNextRectangle(_size);
        
        firstRect.IntersectsWith(result).Should().BeTrue();
        expected.Should().BeEquivalentTo(result);
    }
    
    [Test]
    public void CircularCloudLayout_ShouldNotPlace_TwoRectangles_OnSamePosition()
    {
        var layouter = new CircularCloudLayouter(_center, _pointGenerator);
        
        var firstRect = layouter.PutNextRectangle(_size);
        var secondRect = layouter.PutNextRectangle(_size);
        
        secondRect.IntersectsWith(firstRect).Should().BeFalse();
    }

    [Test]
    public void ArchimedeanSpiralGenerator_ShouldStartFromCenter()
    {
        var firstPoint = _pointGenerator.GetNextPoint().First();
        firstPoint.Should().BeEquivalentTo(_center);
    }

    [Test]
    public void ArchimedeanSpiralGenerator_ShouldGeneratePoints_ByArchimedeanSpiral()
    {
        // Для наглядности пропустил первые 10 значений, потому что там разгон спирали идёт очень медленно, и X с Y равны 0 при округлении
        var points = _pointGenerator.GetNextPoint().Skip(10).Take(10).ToList();
        
        float GetDistance(Point p) => (float)Math.Sqrt(Math.Pow(p.X - _center.X, 2) + Math.Pow(p.Y - _center.Y, 2));
        
        for (var i = 1; i < points.Count; ++i)
            GetDistance(points[i]).Should().BeGreaterThan(GetDistance(points[i - 1]));
    }
}