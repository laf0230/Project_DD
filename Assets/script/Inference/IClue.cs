
namespace Inference
{
    public interface IClue
    {
        string Id { get; set; }
        string Name { get; set; }
        string[] Tags { get; set; }
        ClueDescription[] Description { get; set; }
        int DescriptionLength { get; }
    }
}
