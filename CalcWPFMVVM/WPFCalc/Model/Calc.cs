using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCalc.Model
{
    public class Calc
    {
        public Calc()
        {
            memory = 0;
        }
        public double memory { set; get; }

        public void MemoryAction(double number, char mathSign)
        {
            switch(mathSign)
            {
                case '+':
                    memory += number;
                    break;

                case '-':
                    memory -= number;
                    break;
            }

            //if (mathSign == '+')
            //{
            //    memory += number;
            //}
            //else if (mathSign == '-')
            //{
            //    memory -= number;
            //}
        }

        public double Action (double firstDigit, double secondDigit, char mathSign)
        {
            double rezult = 0;


            switch(mathSign)
            {
                case '+':
                    rezult = firstDigit + secondDigit;
                    break;

                case '-':
                    rezult = firstDigit - secondDigit;
                    break;

                case '*':
                    rezult = firstDigit * secondDigit;
                    break;

                case '/':
                    rezult = firstDigit / secondDigit;
                    break;

            }

            //if (mathSign == '+')
            //{
            //    rezult = firstDigit + secondDigit;
            //}
            //else if (mathSign == '-')
            //{
            //    rezult = firstDigit - secondDigit;
            //}
            //else if (mathSign == '*')
            //{
            //    rezult = firstDigit * secondDigit;
            //}
            //else if (mathSign == '/') //проверка деления на ноль
            //{
            //    rezult = firstDigit / secondDigit;
            //}

            return rezult;
        }
    }
}
