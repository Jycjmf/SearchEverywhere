namespace SearchEverywhere.Model;

public class ChangeMainUiVisibilityModel
{
    public ChangeMainUiVisibilityModel(MainUiElement element)
    {
        Element = element;
    }

    public MainUiElement Element { get; set; }
}