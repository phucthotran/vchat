using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model;
using System.Windows;

namespace vChat.Module
{
    class Helper
    {
        public static void ShowMessage(MethodInvokeResult InvokeResult)
        {
            switch (InvokeResult.Status)
            {
                case MethodInvokeResult.RESULT.SUCCESS:
                case MethodInvokeResult.RESULT.FAIL:
                case MethodInvokeResult.RESULT.UNHANDLE_ERROR:
                    MessageBox.Show(InvokeResult.Message);
                    break;

                case MethodInvokeResult.RESULT.INPUT_ERROR:
                    String Errors = String.Join(",", InvokeResult.Errors.ToArray());
                    MessageBox.Show(String.Format("Please fix following errors: ", Errors));
                    break;
            }
        }
    }
}
