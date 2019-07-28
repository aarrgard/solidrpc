namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a swagger tag
    /// </summary>
    public class SwaggerTag
    {
        /// <summary>
        /// The name of the tag
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The tag description.
        /// </summary>
        public SwaggerDescription Description { get; set; }
    }
}