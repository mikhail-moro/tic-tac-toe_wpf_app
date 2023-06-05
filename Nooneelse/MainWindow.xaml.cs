using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
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

namespace Nooneelse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int player = 1;
        int[,] pole = new int[3, 3];
        int[] poleconv = new int[9];
        int count = 0;
        bool win = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void started_Click_1(object sender, RoutedEventArgs e)
        { 
            player = 1;
            mainn.Visibility = Visibility.Visible;
            hole.Background = Start.Background;
            Start.Visibility = Visibility.Collapsed;
            win = false;
            first.Content = "";
            second.Content = "";
            third.Content = "";
            fourth.Content = "";
            fiveth.Content = "";
            sixth.Content = "";
            seventh.Content = "";
            eighth.Content = "";
            nineth.Content = "";
            for (int i = 0; i <= 2; i++)
            {
                for (int s = 0; s <= 2; s++)
                {
                    pole[i, s] = 0;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string name = System.Convert.ToString(button.Name);
            Console.Write(name);
            if (System.Convert.ToString(button.Content) == "")
            {
                button.Content = "X";
                button.FontWeight = FontWeights.Bold;
                button.FontSize = 30;

                switch (System.Convert.ToString(button.Name))
                {

                    case "first":
                        {
                            pole[0, 0] = 1;
                            break;
                        }
                    case "second":
                        {
                            pole[0, 1] = 1;
                            break;
                        }
                    case "third":
                        {
                            pole[0, 2] = 1;
                            break;
                        }
                    case "fourth":
                        {
                            pole[1, 0] = 1;
                            break;
                        }
                    case "fiveth":
                        {
                            pole[1, 1] = 1;
                            break;
                        }
                    case "sixth":
                        {
                            pole[1, 2] = 1;
                            break;
                        }
                    case "seventh":
                        {
                            pole[2, 0] = 1;
                            break;
                        }
                    case "eighth":
                        {
                            pole[2, 1] = 1;
                            break;
                        }
                    case "nineth":
                        {
                            pole[2, 2] = 1;
                            break;
                        }
                }

                AiTurn();
            }
            Slowery();
        }

        void AiTurn()
        {
            int[] tempArr = new int[9]
            {
                pole[0, 0], pole[0, 1], pole[0, 2],
                pole[1, 0], pole[1, 1], pole[1, 2],
                pole[2, 0], pole[2, 1], pole[2, 2]
            };

            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "python",
                Arguments = @"predict.py",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            });

            BinaryWriter writer = new BinaryWriter(process.StandardInput.BaseStream);
            Array.ForEach(tempArr, writer.Write);
            writer.Flush();

            BinaryReader reader = new BinaryReader(process.StandardOutput.BaseStream);
            int result = reader.ReadInt32();

            Console.WriteLine(result);

            pole[result/3, result%3] = 2;

            Button cell = (Button)mainn.Children[result];
            cell.Content = "O";
            cell.FontWeight = FontWeights.Bold;
            cell.FontSize = 30;
        }

        void Slowery()
        {
            int koey = 0;
            for (int i = 0; i <= 2; i++)
            {
                for (int s = 0; s < 2; s++)
                {
                    poleconv[koey] = pole[i, s];
                }
            }

            for (int i = 0; i <= 2; i++)
            {
                for (int s = 0; s <= 2; s++) 
                { 
                    switch (pole[i, s]) 
                    {
                        case 0:
                            {
                                count = count - 10;
                                break;
                            }
                        case 1:
                            {
                                count = count + 1;
                                break;
                            }
                        case 2:
                            {
                                count = count + 2;
                                break;
                            }
                    }
                    Console.Write(count);
                }
                if (count == 3 || count == 6)
                {
                    win = true;
                }
                Console.WriteLine();
                count = 0;
            }
            for (int i = 0; i <= 2; i++)
            {
                for (int s = 0; s <= 2; s++)
                {
                    switch (pole[s, i])
                    {
                        case 0:
                            {
                                count = count - 10;
                                break;
                            }
                        case 1:
                            {
                                count = count + 1;
                                break;
                            }
                        case 2:
                            {
                                count = count + 2;
                                break;
                            }
                    }
                    Console.Write(count);
                }
                if (count == 3 || count == 6)
                {
                    win = true;
                }
                Console.WriteLine();
                count = 0;
            }
            if ((pole[0,0] == pole[1, 1] && pole[2,2] == pole[0,0] || pole[2, 0] == pole[1,1] && pole[1,1] == pole[0, 2]) && pole[1,1] != 0)
            {
                win = true;
            }
            if (win == true)
            {
                switch (player)
                {
                    case 1:
                        {
                            Start.Background = Brushes.Blue;
                            break;
                        }
                    case 2:
                        {
                            Start.Background = Brushes.Red;
                            break;
                        }
                }
                Showtime();
            }
            else
            {
                int key = 0;
                for (int i = 0; i <= 2; i++)
                {
                    for (int s = 0; s <= 2; s++)
                    {
                        if (pole[i, s] == 0)
                        {
                            key = 1;
                        }
                    }
                }
                if (key == 0)
                {
                    Start.Background = Brushes.Purple;
                    Showtime();
                }
            }
        }
        void Showtime()
        {
            Start.Visibility = Visibility.Visible;
            mainn.Visibility = Visibility.Collapsed;
        }
    }
}
