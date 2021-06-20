using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.OAuth2;
using SolidRpc.Abstractions.Types.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Describes a single API operation on a path.
    /// <a href="https://swagger.io/specification/v2/#operationObject">see</a>
    /// </summary>
    public class OperationObject : ModelBase, IOpenApiOperation
    {
        /// <summary>
        /// The empty list of parameters
        /// </summary>
        public static readonly IEnumerable<ParameterObject> EmptyParameterArray = new ParameterObject[0];

        /// <summary>
        /// Constructs a new intance
        /// </summary>
        /// <param name="parent"></param>
        public OperationObject(ModelBase parent) : base(parent) { }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// A short summary of what the operation does. For maximum readability in the swagger-ui, this field SHOULD be less than 120 characters.
        /// </summary>
        [DataMember(Name = "summary", EmitDefaultValue = false)]
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation behavior. GFM syntax can be used for rich text representation.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        [DataMember(Name = "externalDocs", EmitDefaultValue = false)]
        public ExternalDocumentationObject ExternalDocs { get; set; }

        /// <summary>
        /// Unique string used to identify the operation. The id MUST be unique among all operations described in the API. Tools and libraries MAY use the operationId to uniquely identify an operation, therefore, it is recommended to follow common programming naming conventions.
        /// </summary>
        [DataMember(Name = "operationId", EmitDefaultValue = false)]
        public string OperationId { get; set; }

        /// <summary>
        /// A list of MIME types the operation can consume. This overrides the consumes definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        [DataMember(Name = "consumes", EmitDefaultValue = false)]
        public IEnumerable<string> Consumes { get; set; }

        /// <summary>
        /// A list of MIME types the operation can produce. This overrides the produces definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        [DataMember(Name = "produces", EmitDefaultValue = false)]
        public IEnumerable<string> Produces { get; set; }

        /// <summary>
        /// A list of parameters that are applicable for this operation. If a parameter is already defined at the Path Item, the new definition will override it, but can never remove it. The list MUST NOT include duplicated parameters. A unique parameter is defined by a combination of a name and location. The list can use the Reference Object to link to parameters that are defined at the Swagger Object's parameters. There can be one "body" parameter at most.
        /// </summary>
        [DataMember(Name = "parameters", EmitDefaultValue = false)]
        public IEnumerable<ParameterObject> Parameters { get; set; }

        /// <summary>
        /// Required. The list of possible responses as they are returned from executing this operation.
        /// </summary>
        [DataMember(Name = "responses", EmitDefaultValue = false)]
        public ResponsesObject Responses { get; set; }

        /// <summary>
        /// The transfer protocol for the operation. Values MUST be from the list: "http", "https", "ws", "wss". The value overrides the Swagger Object schemes definition.
        /// </summary>
        [DataMember(Name = "schemes", EmitDefaultValue = false)]
        public IEnumerable<string> Schemes { get; set; }

        /// <summary>
        /// Declares this operation to be deprecated. Usage of the declared operation should be refrained. Default value is false.
        /// </summary>
        [DataMember(Name = "deprecated", EmitDefaultValue = false)]
        public bool Deprecated { get; set; }

        /// <summary>
        /// A declaration of which security schemes are applied for this operation. The list of values describes alternative security schemes that can be used (that is, there is a logical OR between the security requirements). This definition overrides any declared top-level security. To remove a top-level security declaration, an empty array can be used.
        /// </summary>
        [DataMember(Name = "security", EmitDefaultValue = false)]
        public IEnumerable<SecurityRequirementObject> Security { get; set; }

        /// <summary>
        /// Returns the responses object.
        /// </summary>
        /// <returns></returns>
        public ResponsesObject GetResponses()
        {
            if (Responses == null)
            {
                Responses = new ResponsesObject(this);
            }
            return Responses;
        }

        /// <summary>
        /// Returns the response object for supplied code.
        /// </summary>
        /// <param name="responseCode"></param>
        /// <returns></returns>
        public ResponseObject GetResponse(string responseCode)
        {
            var responses = GetResponses();
            if (responses[responseCode] == null)
            {
                responses[responseCode] = new ResponseObject(responses);
            }
            return responses[responseCode];
        }

        /// <summary>
        /// Adds a consumes content type
        /// </summary>
        /// <param name="contentType"></param>
        public void AddConsumes(string contentType)
        {
            Consumes = (new string[] { contentType }).Union(Consumes ?? new string[0]).Distinct().ToArray();
        }

        /// <summary>
        /// Adds a consumes content type
        /// </summary>
        /// <param name="contentType"></param>
        public void AddProduces(string contentType)
        {
            Produces = (new string[] { contentType }).Union(Produces ?? new string[0]).Distinct().ToArray();
        }

        /// <summary>
        /// The method to use to access this operation.
        /// </summary>
        public string GetMethod() {
            if (Parent is PathItemObject pathItem)
            {
                if (pathItem.Delete == this)
                {
                    return "DELETE";
                }
                else if (pathItem.Get == this)
                {
                    return "GET";
                }
                else if (pathItem.Head == this)
                {
                    return "HEAD";
                }
                else if (pathItem.Patch == this)
                {
                    return "PATCH";
                }
                else if (pathItem.Post == this)
                {
                    return "POST";
                }
                else if (pathItem.Put == this)
                {
                    return "PUT";
                }
                else if (pathItem.Options == this)
                {
                    return "OPTIONS";
                }
                else
                {
                    throw new System.Exception("Cannot find operation object.");
                }
            }
            throw new System.Exception("Cannot determine method.");
        }

        /// <summary>
        /// The path to this operation.
        /// </summary>
        public string GetPath()
        {
            return GetParent<PathItemObject>().Path;
        }

        /// <summary>
        /// Returns the absolute path.
        /// </summary>
        /// <returns></returns>
        public string GetAbsolutePath()
        {
            return $"{GetParent<SwaggerObject>().GetBasePath()}{GetPath().Substring(1)}";
        }

        /// <summary>
        /// Returns the address to this operation
        /// </summary>
        /// <returns></returns>
        public Uri GetAddress()
        {
            var path = GetPath();
            if(path.StartsWith("/"))
            {
                path = path.Substring(1);
            }
            return new Uri(GetParent<SwaggerObject>().BaseAddress, path);
        }

        /// <summary>
        /// Returns the consumes elemets - empty list if null
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetConsumes()
        {
            if (Consumes == null)
            {
                return GetParent<SwaggerObject>().Consumes ?? new string[0];
            }
            return Consumes;
        }

        /// <summary>
        /// returns the produces elements - empty list if null
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetProduces()
        {
            if (Produces == null)
            {
                return GetParent<SwaggerObject>().Produces ?? new string[0];
            }
            return Produces;
        }

        /// <summary>
        /// Returns the parameters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParameterObject> GetParameters()
        {
            return Parameters ?? EmptyParameterArray;
        }

        /// <summary>
        /// Returns the parameter with supplied name. Creates one if it does not exist
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public ParameterObject GetParameter(string parameterName)
        {
            var parameter = Parameters?.FirstOrDefault(o => o.Name == parameterName);
            if(parameter == null)
            {
                parameter = new ParameterObject(this) { Name = parameterName };
                Parameters = (Parameters ?? ParameterObject.EmptyList).Union(new[] { parameter }).ToList();
            }
            return parameter;
        }

        /// <summary>
        /// Adds the security key
        /// </summary>
        /// <param name="securityDefinitionName"></param>
        /// <param name="headerName"></param>
        public void AddApiKeyAuth(string securityDefinitionName, string headerName)
        {
            var sro = GetSecurityRequirement(securityDefinitionName);

            //
            // make sure that the security definition exists
            //
            var securityDefinitions = GetParent<SwaggerObject>().GetSecurityDefinitions();
            if(!securityDefinitions.TryGetValue(securityDefinitionName, out SecuritySchemeObject sso))
            {
                securityDefinitions[securityDefinitionName] = sso = new SecuritySchemeObject(securityDefinitions);
                sso.Type = "apiKey";
                sso.In = "header";
                sso.Name = headerName;
            }
        }

        private SecurityRequirementObject GetSecurityRequirement(string securityDefinitionName)
        {
            if (Security == null) Security = new SecurityRequirementObject[0];
            var sro = Security.FirstOrDefault(o => o.Keys.Contains(securityDefinitionName));
            if(sro == null)
            {
                sro = new SecurityRequirementObject(this);
                Security = Security.Union(new[] {
                    sro
                }).ToList();
                sro[securityDefinitionName] = new string[0];
            }
            return sro;
        }

        /// <summary>
        /// Adds the supplied authority scheme to this methos
        /// </summary>
        /// <param name="authDoc"></param>
        /// <param name="flow"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public void AddOAuth2Auth(OpenIDConnnectDiscovery authDoc, string flow, IEnumerable<string> scopes)
        {
            if(scopes == null || !scopes.Any())
            {
                scopes = new[] { "openid", "solidrpc" };
            }
            switch(flow.ToLower())
            {
                case "accesscode":
                case "authorizationcode":
                    AddOAuth2AccessCodeAuth(authDoc, scopes);
                    break;
                default:
                    throw new Exception();

            } 
        }

        private void AddOAuth2AccessCodeAuth(OpenIDConnnectDiscovery authDoc, IEnumerable<string> scopes)
        {
            //
            // make sure that the security definition exists
            //
            var flow = "accessCode";
            var securityDefinitions = GetParent<SwaggerObject>().GetSecurityDefinitions();
            string defName = null;
            SecuritySchemeObject securityScheme = null;
            foreach(var securityDefinition in securityDefinitions)
            {
                if (securityDefinition.Value.Type != "oauth2") continue;
                if (securityDefinition.Value.Flow != flow) continue;
                if (securityDefinition.Value.AuthorizationUrl != authDoc.AuthorizationEndpoint.ToString()) continue;
                if (securityDefinition.Value.TokenUrl != authDoc.TokenEndpoint.ToString()) continue;
                AddScopes(securityDefinition.Value, scopes);
                defName = securityDefinition.Key;
            }
            if(defName == null)
            {
                securityScheme = new SecuritySchemeObject(this)
                {
                    AuthorizationUrl = authDoc.AuthorizationEndpoint.ToString(),
                    TokenUrl = authDoc.TokenEndpoint.ToString(),
                    Type = "oauth2",
                    Flow = flow,
                };
                AddScopes(securityScheme, scopes);
                defName = GetDefinitionName(securityDefinitions.Keys, flow);
                securityDefinitions.Add(defName, securityScheme);
            }

            var sro = GetSecurityRequirement(defName);
            sro[defName] = scopes.ToList();
        }

        private void AddScopes(SecuritySchemeObject sco, IEnumerable<string> scopes)
        {
            if(sco.Scopes == null)
            {
                sco.Scopes = new ScopesObject(sco);
            }
            foreach (var scope in scopes)
            {
                if(!sco.Scopes.TryGetValue(scope, out string desc))
                {
                    sco.Scopes[scope] = scope;
                }
            }
        }

        private string GetDefinitionName(IEnumerable<string> keys, string name)
        {
            int count = 1;
            var newName = name;
            while (keys.Contains(name))
            {
                count++;
                newName = $"{name}{count}";
            }
            return newName;
        }

        /// <summary>
        /// Returns the operation id from 
        /// </summary>
        /// <returns></returns>
        public string GetOperationId()
        {
            if(!string.IsNullOrEmpty(OperationId))
            {
                return OperationId;
            }
            return GetPath().Split('/').Where(o => !o.StartsWith("{")).Last();
        }
    }
}