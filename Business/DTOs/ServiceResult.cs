using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";

        public static ServiceResult SuccessResult(string msg)
        => new() { Success = true, Message = msg };

        public static ServiceResult FailureResult(string msg)
            => new() { Success = false, Message = msg };
    }
}
