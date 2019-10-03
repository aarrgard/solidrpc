using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
using System.IO;
using SolidRpc.Tests.Swagger.CodeGen.Local.Types;
namespace SolidRpc.Tests.Swagger.CodeGen.Local.Services {
    /// <summary>
    /// 
    /// </summary>
    public interface IMvcProxyTest {
        /// <summary>
        /// Sends a boolean back and forth between client and server
        /// </summary>
        /// <param name="b">The boolean to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<bool> ProxyBooleanInQuery(
            bool b = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a short back and forth between client and server
        /// </summary>
        /// <param name="s">The short to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyShortInQuery(
            int s = default(int),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a byte back and forth between client and server
        /// </summary>
        /// <param name="b">The byte to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyByteInQuery(
            int b = default(int),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyIntInQuery(
            int i = default(int),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyIntInForm(
            int i = default(int),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyIntInHeader(
            int i = default(int),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="i">The interger to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<int> ProxyIntInRoute(
            int i,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <param name="iarr">The interger to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<IEnumerable<int>>> ProxyIntArrArrInQuery(
            IEnumerable<IEnumerable<int>> iarr = default(IEnumerable<IEnumerable<int>>),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends an long back and forth between client and server
        /// </summary>
        /// <param name="l">The long to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<long> ProxyLongInQuery(
            long l = default(long),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a float back and forth between client and server
        /// </summary>
        /// <param name="f">The float to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<float> ProxyFloatInQuery(
            float f = default(float),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a double back and forth between client and server
        /// </summary>
        /// <param name="d">The double to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<double> ProxyDoubleInQuery(
            double d = default(double),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a string back and forth between client and server
        /// </summary>
        /// <param name="s">The string to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<string> ProxyStringInQuery(
            string s = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a guid back and forth between client and server
        /// </summary>
        /// <param name="g">The guid to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<Guid> ProxyGuidInQuery(
            Guid g = default(Guid),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a datetimeoffset back and forth between client and server
        /// </summary>
        /// <param name="d">The datetime to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<DateTimeOffset> ProxyDateTimeOffsetInQuery(
            DateTimeOffset d = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <param name="d">The datetime to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<DateTimeOffset> ProxyDateTimeInQuery(
            DateTimeOffset d = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <param name="dArr">The datetime to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<DateTimeOffset>> ProxyDateTimeArrayInQuery(
            IEnumerable<DateTimeOffset> dArr = default(IEnumerable<DateTimeOffset>),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <param name="dArr">The datetime to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<DateTimeOffset>> ProxyDateTimeArrayInForm(
            IEnumerable<DateTimeOffset> dArr = default(IEnumerable<DateTimeOffset>),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a stream back and forth between client and server
        /// </summary>
        /// <param name="ff">The stream to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<Stream> ProxyStream(
            Stream ff = default(Stream),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Sends a complex object back and forth between client and server
        /// </summary>
        /// <param name="co1">The complex object to proxy</param>
        /// <param name="cancellationToken"></param>
        Task<ComplexObject1> ProxyComplexObject1InBody(
            ComplexObject1 co1 = default(ComplexObject1),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}