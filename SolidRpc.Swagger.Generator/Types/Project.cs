﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Types
{
    /// <summary>
    /// Represents a project
    /// </summary>
    public class Project
    {
        /// <summary>
        /// All the project files
        /// </summary>
        public IEnumerable<ProjectFile> ProjectFiles { get; set; }
    }
}
