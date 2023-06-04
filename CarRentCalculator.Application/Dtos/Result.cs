using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentCalculator.Application.Dtos
{
    public class Result<T>
    {
        public Result(ResultType resultType, string error)
        {
            ResultType = resultType;
            ErrorMessage = error;
        }

        public Result(T data)
        {
            Data = data;
        }

        public ResultType ResultType { get; set; } = ResultType.Success;
        public string ErrorMessage = string.Empty;
        public T? Data { get; set; } = default;
    }

    public enum ResultType
    {
        Success,
        Failure,
        NotFound,
    }
}
