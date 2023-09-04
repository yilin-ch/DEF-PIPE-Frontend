namespace DataCloud.PipelineDesigner.Repositories
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string DatabaseName { get; set; }
        public string TemplateCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string ShapeCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string TypeCollectionName { get; set; }
        public string PublicRepoCollectionName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string DatabaseName { get; set; }
        string TemplateCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string ShapeCollectionName { get; set; }
        string CategoryCollectionName { get; set; }
        string TypeCollectionName { get; set; }
        string PublicRepoCollectionName { get; set; }
    }
}
