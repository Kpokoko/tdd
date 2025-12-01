using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudVisualizationApp;

public static class Program
{
    public static void Main()
    {
        // Раскладка фиксированных прямоугольников
        var center = new Point(400, 400);
        var layouter = new CircularCloudLayouter(center);
        var rectSize = new Size(10, 20);
        for (var i = 0; i < 100; ++i)
            layouter.PutNextRectangle(rectSize);
        CloudVisualizer.DrawRectangles(layouter.GetLayout(), center, "static-cloud.bmp");
        
        // Раскладка прямоугольников случайного размера
        var layouter2 = new CircularCloudLayouter(center);
        var random = new Random();
        for (var i = 0; i < 100; ++i)
            layouter2.PutNextRectangle(new Size(random.Next(10, 30), random.Next(10, 30)));
        CloudVisualizer.DrawRectangles(layouter2.GetLayout(), center, "random-cloud.bmp");
    }
}