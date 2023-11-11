using System.Text;

namespace BoschZip
{
    /// <summary>
    /// This class will be supporting the result of a process 
    /// and should provide any type of info regarding what happened
    /// during the process
    /// </summary>
    public class ResultObj
    {
        public ResultObj()
        {
            ResultType = ResultTypeEnum.Success;
            Message = new();
        }
        public ResultObj(ResultTypeEnum type) : this()
        {
            ResultType = type;
        }
        public ResultObj(ResultTypeEnum type, string message) : this(type)
        {
            Message.AppendLine(message);
        }
        public ResultTypeEnum ResultType { get; set; } = ResultTypeEnum.Success;
        public bool IsSuccess { get => ResultType == ResultTypeEnum.Success; }
        public bool IsFailure { get => ResultType == ResultTypeEnum.Failure; }
        public bool IsWarning { get => ResultType == ResultTypeEnum.Warning; }
        public StringBuilder Message { get; set; } = new();
    }

    /// <summary>
    /// Type of result in a process
    /// </summary>
    public enum ResultTypeEnum
    {
        Success = 0,
        Failure = 1,
        Warning = 2
    }


}
