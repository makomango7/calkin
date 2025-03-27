/*
Restrictions:
    Storing only one number at a time...
    Cannot use Nullable type (it is an old calculator)
*/


using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace calkin;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CalculatorDataContext _calcContext;
    private Calculator _calc;
    public MainWindow()
    {
        InitializeComponent();
        _calcContext = new CalculatorDataContext();
        _calc = new Calculator(_calcContext);
        labelExpression.DataContext = _calcContext;
        labelMonitor.DataContext = _calcContext;
    } 

    private void ButtonPressed(object sender, RoutedEventArgs e)
    {
        Button source = (Button)e.Source;
        string tag = (string)source.Tag;

        switch(tag)
        {
            case "num_0":
                _calc.EvalNumber(0);
                break; 
            case "num_1":
                _calc.EvalNumber(1);
                break; 
            case "num_2":
                _calc.EvalNumber(2);
                break; 
            case "num_3":
                _calc.EvalNumber(3);
                break; 
            case "num_4":
                _calc.EvalNumber(4);
                break; 
            case "num_5":
                _calc.EvalNumber(5);
                break; 
            case "num_6":
                _calc.EvalNumber(6);
                break; 
            case "num_7":
                _calc.EvalNumber(7);
                break; 
            case "num_8":
                _calc.EvalNumber(8);
                break; 
            case "num_9":
                _calc.EvalNumber(9);
                break; 
            case "op_+":
                _calc.SwitchOperator(CALCULATION_OPERATOR.ADD);
                break; 
            case "op_-":
                _calc.SwitchOperator(CALCULATION_OPERATOR.REMOVE);
                break; 
            case "op_*":
                _calc.SwitchOperator(CALCULATION_OPERATOR.MULTIPLY);
                break; 
            case "op_/":
                _calc.SwitchOperator(CALCULATION_OPERATOR.DIVIDE);
                break; 
            case "op_=":
                _calc.PerformCalculation();
                break;
            case "op_C":
                _calc.Clear();
                break; 
        }
    }


}

public class CalculatorDataContext : INotifyPropertyChanged
{
    private string _monitorStr;
    private string _exprStr;
    
    public string MonitorString 
    { 
        get => _monitorStr; 
        set
        {
            _monitorStr = value;
            OnPropertyChanged("MonitorString");
        }
    }
    public string ExpressionString 
    { 
        get => _exprStr; 
        set
        {
            _exprStr = value;
            OnPropertyChanged("ExpressionString");
        }
    } 
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}

public class Calculator
{
    private float _mem0;
    private float _mem1;
    private MEMINDEX _memPtr;
    private bool _freshInput;
    private CALCULATION_OPERATOR _switchedOperator;

    private CalculatorDataContext _contextDependency;

    internal Calculator(CalculatorDataContext contextDependency)
    {
        _contextDependency = contextDependency;
        Clear();
        LogInit();
    }

    public void EvalNumber(float number)
    {
        float operatedNum = GetFromMemory();
        if(_freshInput)
        {
            operatedNum = number;
            _freshInput = false;
        }
        else
        {
            operatedNum *= 10;
            operatedNum += number;
        }
        WriteToMemory(operatedNum);
        if(_memPtr == MEMINDEX.MEM1)
        {
            UpdateExprStringEmpty();
        }

        UpdateMonitorString(operatedNum.ToString());
        Log("INPUT");
    }

    public void SwitchOperator(CALCULATION_OPERATOR op)
    {
        _switchedOperator = op;
        SetPointerTo(MEMINDEX.MEM0);
        _freshInput = true;
        UpdateExprString(_mem1.ToString(), String.Empty, false);
        Log("SWITCH");
    }


    public void PerformCalculation()
    {

        SetPointerTo(MEMINDEX.MEM1);
        float tmp = _mem1;
        switch(_switchedOperator)
        {
            case CALCULATION_OPERATOR.ADD:
                _mem1 += _mem0;
                break;
            case CALCULATION_OPERATOR.REMOVE:
                _mem1 -= _mem0;
                break;
            case CALCULATION_OPERATOR.MULTIPLY:
                _mem1 *= _mem0;
                break;
            case CALCULATION_OPERATOR.DIVIDE:
                _mem1 /= _mem0;
                break;
            case CALCULATION_OPERATOR.EMPTY:
                _freshInput = true;
                break;
            default:
                break;
        }
        _freshInput = true;
        UpdateMonitorString(_mem1.ToString());
        UpdateExprString(tmp.ToString(), _mem0.ToString(), true);
        Log("CALC");

    }

    public void Clear()
    {
        _mem0 = 0;
        _mem1 = 0;
        _memPtr = MEMINDEX.MEM1;
        _freshInput = true;
        _switchedOperator = CALCULATION_OPERATOR.EMPTY;
        UpdateExprStringEmpty();
        UpdateMonitorStringEmpty();
    }
    
    private void UpdateExprStringEmpty()
    {
        _contextDependency.ExpressionString = String.Empty;
    }
    private void UpdateMonitorStringEmpty()
    {
        _contextDependency.MonitorString = "0";
    }
    private void UpdateExprString(string op1, string op0, bool isFinal)
    {
        string calculatorExpr = isFinal ? "=" : " ";
        if(_switchedOperator == CALCULATION_OPERATOR.EMPTY)
        {
            op0 = "";
        }
        _contextDependency.ExpressionString = op1 + GetOpString() + op0.ToString() + calculatorExpr;
    }
    private void UpdateMonitorString(string value)
    {
        _contextDependency.MonitorString = value;
    }

    private string GetOpString()
    {
       switch(_switchedOperator)
       {
           case CALCULATION_OPERATOR.ADD:
               return "+";
               break;
           case CALCULATION_OPERATOR.REMOVE:
               return "-";
               break;
           case CALCULATION_OPERATOR.MULTIPLY:
               return "*";
               break;
           case CALCULATION_OPERATOR.DIVIDE:
               return "/";
               break;
           default:
               return "";
               break;
       }
    }

    private void WriteToMemory(float number)
    {
        switch(_memPtr)
        {
            case MEMINDEX.MEM0:
                _mem0 = number;
                break;
            case MEMINDEX.MEM1:
                _mem1 = number;
                break;
        }
    }

    private float GetFromMemory()
    {
        switch(_memPtr)
        {
            case MEMINDEX.MEM0:
                return _mem0;
            case MEMINDEX.MEM1:
                return _mem1;
            default:
                return 0;
        }
    }

    private void SetPointerTo(MEMINDEX mem)
    {
       _memPtr = mem; 
    }

    private void LogInit()
    {
        Console.WriteLine("HEADER\tOP\tMEM0\tMEM1\tPTR\tEXPR\tMONITOR");
    }

    private void Log(string header)
    {
        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", 
                    header, _switchedOperator, _mem0, _mem1, _memPtr, 
                    _contextDependency.ExpressionString, 
                    _contextDependency.MonitorString);
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

public enum MEMINDEX
{
    MEM0 = 0,
    MEM1 = 1
}
