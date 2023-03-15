using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BinaryCalculator.Calculator
{
    public class CalculatorViewModel : ObservableObject
    {
        public IRelayCommand<string> EnterNumberCommand { get; set; }
        public IRelayCommand<string> SelectOperationCommand { get; set; }
        public IRelayCommand CalculateResultCommand { get; set; }
        public IRelayCommand ClearLastCommand { get; set; }
        public IRelayCommand ClearAllCommand { get; set; }

        public string Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged();
                NotifyCanExecuteChanged();
            }
        }
        private string _display;

        private CalculatorModel _calculator;

        private const string DefaultValue = "0";

        public CalculatorViewModel()
        {
            _calculator = new CalculatorModel();
            _display = DefaultValue;

            EnterNumberCommand = new RelayCommand<string>(EnterNumber, CanEnterNumber);
            SelectOperationCommand = new RelayCommand<string>(SelectOperation, CanSelectOperation);
            CalculateResultCommand = new RelayCommand(CalculateResult, CanCalculateResult);
            ClearLastCommand = new RelayCommand(ClearLast, CanClearLast);
            ClearAllCommand = new RelayCommand(ClearAll, CanClearAll);
        }

        private void EnterNumber(string? value)
        {
            if (Display == DefaultValue || _calculator.Status == OperationStatus.WaitsForSecondOperand)
            {
                Display = string.Empty;
            }
            Display += value;

            if (_calculator.Status == OperationStatus.WaitsForSecondOperand)
            {
                _calculator.Status = OperationStatus.ReadyForCalculation;
            }
        }

        private void SelectOperation(string? mathOperator)
        {
            if (mathOperator == null)
            {
                _calculator.Status = OperationStatus.Error;
                return;
            }

            _calculator.Operation = mathOperator;
            if (_calculator.Status == OperationStatus.ReadyForCalculation)
            {
                _calculator.SecondOperand = Display;
                Display = _calculator.Calculate();
            }
            else
            {
                _calculator.FirstOperand = Display;
            }

            if (_calculator.Status != OperationStatus.Error)
            {
                _calculator.Status = OperationStatus.WaitsForSecondOperand;
            }

            NotifyCanExecuteChanged();
        }

        private void ClearLast()
        {
            if (_calculator.Status == OperationStatus.ReadyForCalculation)
            {
                Display = DefaultValue;
            }
        }

        private void ClearAll()
        {
            Display = DefaultValue;
            _calculator = new CalculatorModel();
            NotifyCanExecuteChanged();
        }

        private void CalculateResult()
        {
            if (_calculator.Status is not OperationStatus.Calculated)
            {
                _calculator.SecondOperand = Display;
            }

            _calculator.Status = OperationStatus.ReadyForCalculation;
            Display = _calculator.Calculate();
        }

        private bool CanEnterNumber(string? value)
        {
            return _calculator.Status is not (OperationStatus.Error or OperationStatus.Calculated);
        }

        private bool CanSelectOperation(string? value)
        {
            return _calculator.Status is not OperationStatus.Error;
        }

        private bool CanClearLast()
        {
            return _calculator.Status is not (OperationStatus.Error or OperationStatus.WaitsForFirstOperand or OperationStatus.Calculated);
        }

        private bool CanClearAll()
        {
            return true;
        }

        private bool CanCalculateResult()
        {
            return _calculator.Status is not (OperationStatus.Error or OperationStatus.WaitsForFirstOperand);
        }

        private void NotifyCanExecuteChanged()
        {
            EnterNumberCommand.NotifyCanExecuteChanged();
            SelectOperationCommand.NotifyCanExecuteChanged();
            ClearLastCommand.NotifyCanExecuteChanged();
            CalculateResultCommand.NotifyCanExecuteChanged();
        }
    }
}
