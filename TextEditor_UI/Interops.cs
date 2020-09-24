using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurTextEditor
{
    public static class CursorInterop
    {
        public static IJSRuntime JsRuntime = null;

        public static ValueTask<bool> Alert(string msg)
        {
            return JsRuntime.InvokeAsync<bool>(
                "CursorPositionFunctions.Alert", msg);
        }

        public static ValueTask<string> GetCaretCoordinates(string elementId)
        {
            return JsRuntime.InvokeAsync<string>(
                "CursorPositionFunctions.getCaretCoordinates", elementId);
        }

        public static ValueTask<double> GetElementActualTop(string elementId)
        {
            return JsRuntime.InvokeAsync<double>(
                "CursorPositionFunctions.GetElementActualTop", elementId);
        }

        public static ValueTask<double> GetElementActualLeft(string elementId)
        {
            return JsRuntime.InvokeAsync<double>(
                "CursorPositionFunctions.GetElementActualLeft", elementId);
        }

        public static ValueTask<int> GetCharCursorPosition(string elementId)
        {
            return JsRuntime.InvokeAsync<int>("CursorPositionFunctions.GetCharCursorPosition", elementId);
        }
    }
}