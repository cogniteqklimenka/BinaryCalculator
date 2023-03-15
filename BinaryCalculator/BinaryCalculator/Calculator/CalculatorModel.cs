using BinaryCalculator.Helpers;

namespace BinaryCalculator.Calculator
{
    public class CalculatorModel
    {
        public string FirstOperand { get; set; } = DefaultValue;
        public string SecondOperand { get; set; } = DefaultValue;
        public string Operation { get; set; } = string.Empty;
        public OperationStatus Status { get; set; } = OperationStatus.WaitsForFirstOperand;

        private const string DefaultValue = "0";
        private const string ErrorMessage = "Error";
        
        public string Calculate()
        {
            if (Status == OperationStatus.WaitsForSecondOperand)
            {
                return DefaultValue;
            }

            var firstOperand = BinaryConvertHelper.ConvertBinaryToDecimal(FirstOperand);
            var secondOperand = BinaryConvertHelper.ConvertBinaryToDecimal(SecondOperand);

            var result = -1;
            result = Operation switch
            {
                "+" => firstOperand + secondOperand,
                "-" => firstOperand - secondOperand,
                _ => result
            };

            if (result < 0)
            {
                Status = OperationStatus.Error;
                return ErrorMessage;
            }
            
            var convertedResult = BinaryConvertHelper.ConvertDecimalToBinary(result);
            FirstOperand = convertedResult;

            Status = OperationStatus.Calculated;
            return convertedResult;
        }
    }
}
