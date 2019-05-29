using System;
using SolidRpc.Swagger.Model;
using SolidRpc.Swagger.Model.V2;

namespace SolidRpc.Swagger.Generator.V2
{
    public class SwaggerSpecGeneratorV2 : SwaggerSpecGenerator
    {
        public SwaggerSpecGeneratorV2(SwaggerSpecSettings settings) : base(settings)
        {
        }

        protected override ISwaggerSpec CreateSwaggerSpecFromTypesInRepo()
        {
            var swaggerObject = new SwaggerObject
            {
                Swagger = "2.0",
                Info = new InfoObject()
                {
                    Title = Settings.Title,
                    Version = Settings.Version,
                    License = CreateLicense(),
                    Contact = CreateContact()
                },
                Host = "localhost",
                BasePath = "/",

            };
            return swaggerObject;
        }

        private ContactObject CreateContact()
        {
            var contact = new ContactObject()
            {
                Email = Settings.ContactEmail,
                Name = Settings.ContactName,
                Url = Settings.ContactUrl
            };
            if(string.IsNullOrEmpty(contact.Email) &&
                string.IsNullOrEmpty(contact.Name) &&
                string.IsNullOrEmpty(contact.Url))
            {
                return null;
            }
            return contact;
        }

        private LicenseObject CreateLicense()
        {
            if (string.IsNullOrEmpty(Settings.LicenseName))
            {
                return null;
            }
            return new LicenseObject()
            {
                Name = Settings.LicenseName,
                Url = Settings.LicenseUrl
            };
        }
    }
}