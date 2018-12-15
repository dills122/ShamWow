namespace ShamWow.Processor
{
    public interface IShamWow
    {
        object CleanData();
        ShamWowEngine Scrub();
        bool CheckManifest();
        DocumentManifest GetManifest();
    }
}
