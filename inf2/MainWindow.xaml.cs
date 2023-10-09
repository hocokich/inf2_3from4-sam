using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace inf2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string num1;
        public string num2;
        public MainWindow()
        {
            InitializeComponent();
        }
        static int[] BinaryAdd(int[] n1, int[] n2)//сложение
        {
            int[] add = new int[8];//результат нашей операции
            int ntrans = 0;//перенос в следующий разряд
            for (int i = 7; i >= 0; i--)
            {
                add[i] = n1[i] + n2[i] + ntrans;
                if (add[i] == 0) { add[i] = 0; ntrans = 0; }//0 + 0 = 0
                if (add[i] == 1) { add[i] = 1; ntrans = 0; }//1 + 0 = 1
                if (add[i] == 2) { add[i] = 0; ntrans = 1; }//1 + 1 = 2. А должно быть 1 и 0, где 1 = ntrans, а  0 = текущий элемент массива
                if (add[i] == 3) { add[i] = 1; ntrans = 1; }//1 + 1 + 1 = 3. Складываем 2 элемента массива 1 + 1 и 1 трансфер.
            }                                               //У нас получается 1 + 1 = 1 0 ==> 1 идет в новый трансфер, 0 складывается с предыдущем трансфером и записывается 1 в элемент массива.
            return add;
        }
        static int[] BinarySub(int[] n1, int[] n2)//вычитание
        {

            //по сути вычитание это сложение положительного и отрицательного числа.
            int[] t = { 0, 0, 0, 0, 0, 0, 0, 0 };

            t = AddOne(Invers((int[])n2.Clone()));

            n1 = BinaryAdd(n1, t);

            return n1; // инверсируем второе число и вычисляем

            // бывший код - BinaryAdd(n1, AddOne(Invers(n2)))

        }
        static int[] Invers(int[] n) //функция инвертирования значений
        {
            for (int i = 7; i >= 0; i--)
            {
                if (n[i] == 0) n[i] = 1;
                else n[i] = 0;
            }
            return n;
        }
        static int[] AddOne(int[] n) //функция добавления единицы к числу
        {
            for (int i = 7; i >= 0; i--)
            {
                if (n[i] == 1) n[i] = 0;// -5^10 != 1111 101[0]^2 != [1]
                else                    // ||
                {                       // \/
                    n[i] = 1;           // 1111 101[1] = 1
                    break; //прерываем цикл отправляем на золото
                }
            }
            return n;
        }
        static int[] BinaryMul(int[] n1, int[] n2)//умножение
        {
            int[] mul = new int[8];
            int[] multik = { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int j = 7; j >= 0; j--)
            {
                for (int i = 7; i >= 0; i--)//1 из 8 массивов
                {
                    mul[i] = n2[j] * n1[i];
                }
                multik = BinaryAdd(mul, multik);//складываем последовательно текущий и пердыдущий массив
                n1 = Sdvig(n1, 1);//и каждый раз сдвигаем влево на 1 разряд
            }
            return multik;
        }
        static int[] BinaryDiv(int[] a, int[] b)//деление(не работает :-))
        {
            int[] d = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] temp_a = (int[])a.Clone();
            int[] r = new int[8];
            //r[7] = a[0];
            //r - b
            //r << 1
            //r[7] = a[1]

            for (int i = 0; i <= 7; i++)
            {
                Sdvig(r, 1);
                r[7] = a[i];
                temp_a = (int[])r.Clone();
                BinarySub(temp_a, b);
                if (temp_a[0] == 0)
                {
                    r = (int[])temp_a.Clone();
                    d[i] = 1;
                }
                else d[i] = 0;
            }
            return d;
        }
        static int[] Perevorot(int[] n) // запись массива в нормальном порядке
        {
            int[] Perevorot = new int[8];
            Perevorot[0] = n[7];
            Perevorot[1] = n[6];
            Perevorot[2] = n[5];
            Perevorot[3] = n[4];
            Perevorot[4] = n[3];
            Perevorot[5] = n[2];
            Perevorot[6] = n[1];
            Perevorot[7] = n[0];
            return Perevorot;
        }
        static int[] Sdvig(int[] n, int poz)
        {
            int[] sdmas = new int[8];
            if (poz == 1)//если 1 то влево
            {
                sdmas[7] = 0;
                for (int i = 7; i >= 1; i--)
                {
                    sdmas[i - 1] = n[i];
                }
                return sdmas;
            }
            if (poz == 2)//если 2 то вправо
            {
                sdmas[0] = 0;
                for (int i = 0; i < 7; i++)
                {
                    sdmas[i + 1] = n[i];
                }
            }
            return sdmas;
        }
        static int BinToInt(int[] n) //преобразование массива целых чисел в целое число
        {
            int num = BinToDec(n);

            if (num > 127) // для отрицательного числа
            {
                num = BinToDec(Invers(n)); // n - 1
                num++;                         // n - 1 + 1  =  n
                num *= -1;                     // n * -1 = -n
            }
            return num;
        }
        static int BinToDec(int[] n)// алгоритм преобразования числа из 2 в 10
        {
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                sum += n[i] * (int)(Math.Pow(2, 8 - i - 1));
            }
            return sum;
        }
        static int[] StrToBin(string n)//преобразовываем строку в массив
        {
            int[] bin = new int[8];
            for (int i = 0; i < 8; i++)
            {
                bin[i] = int.Parse(Char.ToString(n[i]));
            }
            return bin;
        }
        static string BinToStr(int[] n) //преобразуем массив в строку
        {
            string str = String.Join("", n); //https://learn.microsoft.com/ru-ru/dotnet/api/system.string.join?view=net-7.0
            return str;
        }
        private void plus_Click(object sender, RoutedEventArgs e)//кнопка сложения
        {
            num1 = A.Text;
            num2 = B.Text;
            result.Text = BinToStr(BinaryAdd(StrToBin(num1), StrToBin(num2)));
        }
        private void minus_Click(object sender, RoutedEventArgs e)//кнопка вычитания
        {
            num1 = A.Text;
            num2 = B.Text;
            result.Text = BinToStr(BinarySub(StrToBin(num1), StrToBin(num2)));
        }
        private void multi_Click(object sender, RoutedEventArgs e)//конпка умножения
        {
            num1 = A.Text;
            num2 = B.Text;
            result.Text = BinToStr(BinaryMul(StrToBin(num1), StrToBin(num2)));
        }
        private void division_Click(object sender, RoutedEventArgs e)//кнопка деления
        {
            num1 = A.Text;
            num2 = B.Text;
            result.Text = BinToStr(BinaryDiv(StrToBin(num1), StrToBin(num2)));
        }
        private void A_TextChanged(object sender, TextChangedEventArgs e)//тексбокс для 1 числа
        {

        }
        private void B_TextChanged(object sender, TextChangedEventArgs e)//текстбокс для 2 числа
        {

        }
        private void result_TextChanged(object sender, TextChangedEventArgs e)//ответ в текстбоксе
        {

        }
    }
/*
    int[] chastnoe = new int[8];
    int nomer_elementa_delimovo = 0;//5
            for (int i = 7; i >= 0; i--)
            {
                if (Perevorot(delimoe)[i] == 1)
                {
                    nomer_elementa_delimovo = i + 1;
                    break;
                }
            }
            int nomer_elementa_delitela = 0;//2
for (int i = 7; i >= 0; i--)
{
    if (Perevorot(delitel)[i] == 1)
    {
        nomer_elementa_delitela = i + 1;
        break;
    }
}

for (int i = nomer_elementa_delimovo - nomer_elementa_delitela; i >= 1; i--)//3 раза сдвигаем влево делитель
{
    delitel = Sdvig(delitel, 1); // привели крайнюю левую 1 делителя к делимому
}*/

    /*static string BinaryDivision(int[] n1, int[] n2)// деление
    {
        string chastnoe = ""; //результат
        int[] delimoe = n2; //делитель
        for (int i = 7; i >= 0; i--)
        {
            if (n1[i] == 1)
            {
                while (i + 1 > 0)
                {
                    if (delimoe[0] >= n2[0])
                    {
                        chastnoe += "1";
                        delimoe = BinarySub(delimoe, n2);
                    }
                    else
                    {
                        chastnoe += "0";
                    }
                    delimoe = SdvigLevo(delimoe);
                    i--;
                }
                break;
            }
        }
        return chastnoe;// возращаемое значение
    }*/

    /*static string BinaryDivision(string numerator, string denominator)// деление
    {
        int resultLength = numerator.Length;
        string result = ""; //результат
        string numeratorCopy = numerator; //числитель

        while (resultLength > 0)
        {
            if (numeratorCopy[0] >= denominator[0])
            {
                result += "1";
                numeratorCopy = BinarySub(numeratorCopy, denominator);
            }
            else
            {
                result += "0";
            }

            numeratorCopy = numeratorCopy.Substring(1) + "0";
            resultLength--;
        }
        return result;// возращаемое значение
    }*/

    /*if (BinToInt(BinarySub(delimoe, delitel)) > 0)
                {
                    SdvigLevo(delitel);
                    chastnoe[i] = 0;
                    break;
                }
                if (BinToInt(BinarySub(delimoe, delitel)) == 0)
                {
                    delimoe = BinarySub(delimoe, delitel);
                    chastnoe[i] = 1;
                }
                else
                {
                    SdvigPravo(delitel);
                    delimoe = BinarySub(delimoe, delitel);
                    chastnoe[i] = 1;
                    break;
                }*/

    /*static int[] DivideBinary(int[] dividend, int[] divisor)
    {
        if (divisor == "0")
        {
            throw new DivideByZeroException("Деление на ноль невозможно.");
        }

        string quotient = "";
        string remainder = dividend;

        for (int i = 0; i < dividend.Length; i++)
        {
            // Сдвигаем остаток на один бит влево и добавляем следующий бит из делимого числа
            remainder += dividend[i].ToString();

            // Если остаток больше или равен делителю, добавляем 1 в частное и вычитаем делитель
            if (IsGreaterOrEqual(remainder, divisor))
            {
                quotient += "1";
                remainder = SubtractBinary(remainder, divisor);
            }
            else
            {
                quotient += "0";
            }
        }

        return (quotient, remainder);
    }

    static bool IsGreaterOrEqual(string binaryA, string binaryB)
    {
        int lengthA = binaryA.Length;
        int lengthB = binaryB.Length;

        if (lengthA > lengthB)
        {
            return true;
        }
        else if (lengthA < lengthB)
        {
            return false;
        }
        else
        {
            return binaryA.CompareTo(binaryB) >= 0;
        }
    }*/
}