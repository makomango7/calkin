/*
Restrictions:
    Storing only one number at a time...
    Cannot use Nullable type (it is an old calculator)
*/


using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace calkin;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private float _storedNumber;
    private float _input = 0;
    private CALCULATION_OPERATOR _switchedOperator; 

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonPressed(object sender, RoutedEventArgs e)
    {
        Button source = (Button)e.Source;
        string tag = (string)source.Tag;

        switch(tag)
        {
            case "num_0":
                EvalNumber(0);
                break; 
            case "num_1":
                EvalNumber(1);
                break; 
            case "num_2":
                EvalNumber(2);
                break; 
            case "num_3":
                EvalNumber(3);
                break; 
            case "num_4":
                EvalNumber(4);
                break; 
            case "num_5":
                EvalNumber(5);
                break; 
            case "num_6":
                EvalNumber(6);
                break; 
            case "num_7":
                EvalNumber(7);
                break; 
            case "num_8":
                EvalNumber(8);
                break; 
            case "num_9":
                EvalNumber(9);
                break; 
            case "op_+":
                SwitchOperator(CALCULATION_OPERATOR.ADD);
                break; 
            case "op_-":
                SwitchOperator(CALCULATION_OPERATOR.REMOVE);
                break; 
            case "op_*":
                SwitchOperator(CALCULATION_OPERATOR.MULTIPLY);
                break; 
            case "op_%":
                SwitchOperator(CALCULATION_OPERATOR.DIVIDE);
                break; 
            case "op_=":
                PerformCalculation();
                break; 
        }
    }

    private void EvalNumber(int number)
    {
        _input *= 10;
        _input += number;
        System.Console.WriteLine("Input is: " + _input);
    }

    private void SwitchOperator(CALCULATION_OPERATOR op)
    {
        _switchedOperator = op;
        _storedNumber = _input;
        _input = 0;
        Console.WriteLine("SWITCHING in: {0}, op: {1}, mem: {2}", _input, _switchedOperator, _storedNumber);
    }

    private void PerformCalculation()
    {
        Console.WriteLine("CALCULATE in: {0}, op: {1}, mem: {2}", _input, _switchedOperator, _storedNumber);
        switch(_switchedOperator)
        {
            case CALCULATION_OPERATOR.ADD:
                _storedNumber += _input;
                break;
            case CALCULATION_OPERATOR.REMOVE:
                _storedNumber -= _input;
                break;
            case CALCULATION_OPERATOR.MULTIPLY:
                _storedNumber *= _input;
                break;
            case CALCULATION_OPERATOR.DIVIDE:
                _storedNumber /= _input;
                break;
            default:
                break;
        }
        // _input = 0;
        Console.WriteLine("Stored number {0}", _storedNumber);
    }
}

public enum  CALCULATION_OPERATOR
{
    ADD,
    REMOVE,
    MULTIPLY,
    DIVIDE,
    EMPTY 
}