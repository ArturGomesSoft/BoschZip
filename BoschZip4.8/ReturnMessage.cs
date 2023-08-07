using System.Text;

namespace BoschZip
{
    public class ResultObj
    {
        public ResultObj()
        {
            ResultType = ResultTypeEnum.Success;
            Message = new StringBuilder();
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
        public StringBuilder Message { get; set; } = new StringBuilder();
    }

    public enum ResultTypeEnum
    {
        Success = 0,
        Failure = 1,
        Warning = 2
    }


}
