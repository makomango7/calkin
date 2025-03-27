﻿/*
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
    private float _mem0 = 0;
    private float _mem1 = 0;
    private MEMINDEX _memPtr = MEMINDEX.MEM1; // 0 is mem0, 1 is mem1
    private bool _inputClosedSwitch;
    private CALCULATION_OPERATOR _switchedOperator = CALCULATION_OPERATOR.EMPTY;
    private string _monitorStr;
    private string _exprStr;

    public MainWindow()
    {
        InitializeComponent();
        LogInit();
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

    private void EvalNumber(float number)
    {
        float operatedNum = GetFromMemory();
        if(_inputClosedSwitch)
        {
            operatedNum = number;
            _inputClosedSwitch = false;
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

    private void SwitchOperator(CALCULATION_OPERATOR op)
    {
        _switchedOperator = op;
        SetPointerTo(MEMINDEX.MEM0);
        _inputClosedSwitch = true;
        UpdateExprString(_mem1.ToString(), String.Empty, false);
        Log("SWITCH");
    }


    private void PerformCalculation()
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
                break;
            default:
                break;
        }
        _inputClosedSwitch = true;
        UpdateMonitorString(_mem1.ToString());
        UpdateExprString(tmp.ToString(), _mem0.ToString(), true);
        Log("CALC");

    }
    
    private void UpdateExprStringEmpty()
    {
        _exprStr = String.Empty;
    }
    private void UpdateExprString(string op1, string op0, bool isFinal)
    {
        string calculatorExpr = isFinal ? "=" : " ";
        _exprStr = op1 + GetOpString() + op0.ToString() + calculatorExpr;
    }
    private void UpdateMonitorString(string value)
    {
        _monitorStr = value;
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
                    header, _switchedOperator, _mem0, _mem1, _memPtr, _exprStr, _monitorStr);
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
