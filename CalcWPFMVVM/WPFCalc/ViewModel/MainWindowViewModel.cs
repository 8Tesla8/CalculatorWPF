using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFCalc.Model;

namespace WPFCalc.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Calc calc = new Calc(); 

        private RelayCommand<string> digitBtnPress;
        private RelayCommand<string> operetionBtnPress;
        private RelayCommand<string> memoryBtnPress;
        private RelayCommand<string> deleteBtnPress;
        private RelayCommand<string> oneOperationBtnPress;

        // display
        private string bigDisplay = "0";
        private string smallDisplay;
        private string memoryDisplay = "   ";

        // bool
        private bool oneOperationWas = false; // становится true если была нажата кнопка одной операции
        private bool equalWas = false;    // становится true если была нажата кнопка = - + * /
        private bool digitWas = false; //становится true если была нажата кнопка цифры

        private double firstDigit;
        private char mathSign;


        public string MemoryDisplay
        {
            get
            {
                return memoryDisplay;
            }
            set
            {
                memoryDisplay = value;
                RaisePropertyChanged("MemoryDisplay");
            }
        }
        public string BigDisplay
        {
            get
            {
                return bigDisplay;
            }
            set
            {
                bigDisplay = value;
                RaisePropertyChanged("BigDisplay");
            }
        }
        public string SmallDisplay
        {
            get
            {
                return smallDisplay;
            }
            set
            {
                smallDisplay = value;
                RaisePropertyChanged("SmallDisplay");
            }
        }

        public ICommand OneOperationBtnPressCommand
        {
            get
            {
                if (oneOperationBtnPress == null)
                {
                    oneOperationBtnPress = new RelayCommand<string>(
                        OneOperationBtnPress, CanOneOperationBtnPress);
                }
                return oneOperationBtnPress;
            }
        }
        public void OneOperationBtnPress(string btn)
        {
            oneOperationWas = true;
            switch (btn)
            {
                case "Sqrt":
                    firstDigit = Math.Sqrt(Convert.ToDouble(BigDisplay));
                    SmallDisplay = string.Format(@"{0} Sqrt({1})", smallDisplay, BigDisplay);
                    break;

                case "%":
                    firstDigit = Convert.ToDouble(BigDisplay) / 100;
                    SmallDisplay = string.Format(@"{0}%", BigDisplay);                   
                    break;

                case "1/x":
                    firstDigit = 1 / Convert.ToDouble(BigDisplay);
                    SmallDisplay = string.Format(@"reciproc({0})", BigDisplay);
                    break;

            }
            BigDisplay = Convert.ToString(firstDigit);
            oneOperationWas = true;
        }
        public bool CanOneOperationBtnPress(string btn)
        {
            return true;
        }

        public ICommand DigitBtnPressCommand
        {
            get
            {
                if (digitBtnPress == null)
                {
                    digitBtnPress = new RelayCommand<string>(
                        DigitBtnPress, CanDigitBtnPress);
                }
                return digitBtnPress;
            }
        }
        public void DigitBtnPress(string btn)
        {

            if (btn == "+ -")
            {
                if (BigDisplay.Contains("-"))
                {
                    BigDisplay = BigDisplay.Remove(BigDisplay.IndexOf("-"), 1);
                }
                else
                {
                    BigDisplay = '-' + BigDisplay;
                }
                return;
            }

            //если была нажата кнопка кнопка одной операции, после была нажата кнопка цифры, все дисплей сбрасываются
            if (oneOperationWas)
            {
                oneOperationWas = false;
                SmallDisplay = null;
                BigDisplay = "0";
            }

            if (btn == ",")
            {
                if (BigDisplay.Contains(","))
                {
                    return;
                }
                else
                {
                    BigDisplay += btn;
                    digitWas = true;
                    return;
                }
            }

            if (btn == "0" && BigDisplay == "0")
            {
                return;
            }

            if (BigDisplay == "0" || equalWas) // ввод первого символа или посленажатия кнопки =
            {               
                BigDisplay = btn;
                digitWas = true;
                return;
            }

            BigDisplay += btn;
            digitWas = true;

        }
        public bool CanDigitBtnPress(string btn)
        {
            return true;
        }

        public ICommand DeleteBtnPressCommand
        {
            get
            {
                if (deleteBtnPress == null)
                {
                    deleteBtnPress = new RelayCommand<string>(
                        DeleteBtnPress, CanDeleteBtnPress);
                }
                return deleteBtnPress;
            }
        }
        private void DeleteBtnPress(string btn)
        {
            switch(btn)
            {
                case "Del":
                    BigDisplay = (BigDisplay.Length > 1 ?
                        BigDisplay.Substring(0, BigDisplay.Length - 1) :
                        BigDisplay = "0");
                    break;

                case "CE":
                    BigDisplay = "0";

                    equalWas = false;
                    oneOperationWas = false;
                    digitWas = false;
                    break;

                case "C":
                    BigDisplay = "0";
                    SmallDisplay = null;

                    equalWas = false;
                    oneOperationWas = false;
                    digitWas = false;
                    break;
            }

        }
        private bool CanDeleteBtnPress(string btn)
        {
            return true;
        }
        public ICommand OperetionBtnPressCommand
        {
            get
            {
                if (operetionBtnPress == null)
                {
                    operetionBtnPress = new RelayCommand<string>(
                        OperetionBtnPress, CanOperationBtnPress);
                }
                return operetionBtnPress;
            }
        }
        public void OperetionBtnPress(string btn)
        {

            if(btn == "=")
            {
                if (oneOperationWas)// если была нажата кнопка одной операции и кнопка =
                {
                    SmallDisplay = null;
                    equalWas = true;
                    return;
                }
                else if(!string.IsNullOrEmpty(smallDisplay))
                {
                    firstDigit = calc.Action(firstDigit, Convert.ToDouble(BigDisplay), mathSign);
                    BigDisplay = Convert.ToString(firstDigit);

                    mathSign = ' ';
                    SmallDisplay = null;

                    equalWas = true;
                    oneOperationWas = false;
                    digitWas = false;
                }
            }

            //if (oneOperationWas && btn == "=")// если была нажата кнопка одной операции и кнопка =
            //{
            //    SmallDisplay = null;
            //    equalWas = true;
            //    return;
            //}

            //if (btn == "=" && !string.IsNullOrEmpty(smallDisplay))
            //{
            //    firstDigit = calc.Action(firstDigit, Convert.ToDouble(BigDisplay), mathSign);
            //    BigDisplay = Convert.ToString(firstDigit);

            //    mathSign = ' ';
            //    SmallDisplay = null;

            //    equalWas = true;
            //    oneOperationWas = false;
            //    digitWas = false;
            //}
            else
            {

                if (string.IsNullOrEmpty(smallDisplay)) // заполяняет маленький экран если он пуст
                {
                    mathSign = btn[0];
                    firstDigit = Convert.ToDouble(BigDisplay);

                    SmallDisplay = string.Format(@"{0} {1}", BigDisplay, mathSign);
                    BigDisplay = "0";

                    digitWas = false;
                    oneOperationWas = false;
                }
                else
                {
                    // дает возможность изменять знак операции
                    // условие срабатывает если не была нажата кнопка цифры  
                    if (!digitWas)
                    {
                        mathSign = btn[0];
                        SmallDisplay = SmallDisplay.Substring(0, SmallDisplay.Length - 1) + mathSign;

                        digitWas = false;
                        oneOperationWas = false;
                        return;
                    }

                    digitWas = false;
                    oneOperationWas = false;

                    firstDigit = calc.Action(firstDigit, Convert.ToDouble(BigDisplay), mathSign);

                    mathSign = btn[0];

                    SmallDisplay = string.Format(@"{0} {1} {2}", SmallDisplay, BigDisplay, mathSign);
                    BigDisplay = Convert.ToString(firstDigit);
                }
            }

        }
        public bool CanOperationBtnPress(string btn)
        {
            return true;
        }

        public ICommand MemoryBtnPressCommand
        {
            get
            {
                if (memoryBtnPress == null)
                {
                    memoryBtnPress = new RelayCommand<string>(
                        MemoryBtnPress, CanMemoryBtnPress);
                }
                return memoryBtnPress;
            }
        }
        public void MemoryBtnPress(string btn)
        {
            switch(btn)
            {
                case "M+":
                    calc.MemoryAction(Convert.ToDouble(BigDisplay), '+');
                    MemoryDisplay = "M";
                    break;

                case "M-":
                    calc.MemoryAction(Convert.ToDouble(BigDisplay), '-');
                    MemoryDisplay = "M";
                    break;

                case "MS": // запомнить число
                    calc.memory = Convert.ToDouble(BigDisplay);
                    MemoryDisplay = "M";
                    break;

                case "MC": // сбросить число в памяти
                    calc.memory = 0;
                    MemoryDisplay = "   ";
                    break;

                case "MR": // вывести чесло из памяти на дисплей  
                    BigDisplay = Convert.ToString(calc.memory);
                    break;
            }
        }
        public bool CanMemoryBtnPress(string btn)
        {
            return true;
        }


    }
}
