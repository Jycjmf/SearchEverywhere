namespace SearchEverywhere.Model;

public class PreviewUiElementModel
{
    public enum PreviewUiElement
    {
        Video,
        Image,
        Text,
        Config,
        Ppt,
        Word,
        Excel
    }

    public PreviewUiElementModel(PreviewUiElement elementName, bool isSmallWindow, string path)
    {
        ElementName = elementName;
        IsSmallWindow = isSmallWindow;
        Path = path;
    }

    public PreviewUiElement ElementName { get; set; }
    public bool IsSmallWindow { get; set; }
    public string Path { get; set; }
}