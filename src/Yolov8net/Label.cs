namespace Yolov8.Net
{
    public class Label
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public LabelKind Kind { get; init; } = LabelKind.Generic;
    }
}
