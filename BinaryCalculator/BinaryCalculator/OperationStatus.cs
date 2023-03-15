namespace BinaryCalculator
{
    public enum OperationStatus
    {
        WaitsForFirstOperand,
        WaitsForSecondOperand,
        ReadyForCalculation,
        Calculated,
        Error
    }
}
