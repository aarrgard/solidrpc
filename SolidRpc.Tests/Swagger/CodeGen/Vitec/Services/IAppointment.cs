using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Appointment.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IAppointment {
        /// <summary>
        /// Skapa kalenderh�ndelse
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="id"></param>
        /// <param name="appointment"></param>
        /// <param name="cancellationToken"></param>
        Task<string> AppointmentPost(
            string customerId,
            string id,
            Models.Appointment appointment,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}