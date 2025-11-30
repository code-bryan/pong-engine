namespace Engine.Core.Components;

public class AIComponent
{
    public float Difficulty { get; set; } = .8f;
    public float ProdictionErrorMargin { get; set; } = 30f;
}