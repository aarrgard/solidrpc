using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.OperatorOverrides.Types
{
    /// <summary>
    /// ComplexType1
    /// </summary>
    public class ComplexType
    {
        public override bool Equals(object obj)
        {
            if(!(obj is ComplexType other))
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return 1;
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public static ComplexType operator -(ComplexType a, ComplexType b) => new ComplexType();
        public static ComplexType operator +(ComplexType a, ComplexType b) => new ComplexType();
    }
}
