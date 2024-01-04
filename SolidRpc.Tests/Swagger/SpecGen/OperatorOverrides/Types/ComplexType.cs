using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.OperatorOverrides.Types
{
    /// <summary>
    /// Interface
    /// </summary>
    public interface IComplexType
    {
        /// <summary>
        /// 
        /// </summary>
        int X { get; set; }
    }

    /// <summary>
    /// ComplexType1
    /// </summary>
    public class ComplexType : IComplexType
    {
        public int X { get => 1; set { } }
        int IComplexType.X { get => 2; set { } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(!(obj is ComplexType other))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ComplexType operator -(ComplexType a, ComplexType b) => new ComplexType();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ComplexType operator +(ComplexType a, ComplexType b) => new ComplexType();
    }
}
