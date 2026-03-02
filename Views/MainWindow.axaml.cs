using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Den.Models;
using Microsoft.EntityFrameworkCore;

namespace Den.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public void Button_Click(object s, RoutedEventArgs e)
    {
        GusetWindow gusetWindow = new GusetWindow();
        gusetWindow.Show();
        this.Close();
    }
    public void Button_Click_1(object s, RoutedEventArgs e)
    {
        string login = this.Login.Text;
        string password = this.Password.Text;


        using(var db = new DemoContext())
        {
            if(login != null && password != null)
            {
                var persons = db.Users.Include(x => x.Role).FirstOrDefault(x => x.Email == login);

                if(persons == null)
                {
                    System.Console.WriteLine("нулл");
                }

                else if(persons.Password == password)
                {
                    if(persons.Role?.Name == "Авторизированный клиент")
                    {
                        System.Console.WriteLine(persons.Id);
                        CustomersWindows customersWindows = new CustomersWindows(persons.Id);
                        customersWindows.Show();
                        this.Close();
                    }
                    else if(persons.Role?.Name == "Менеджер")
                    {
                        System.Console.WriteLine(persons.Id);
                        ManagerWindow customersWindows = new ManagerWindow(persons.Id);
                        customersWindows.Show();
                        this.Close();
                    }
                    else if(persons.Role?.Name == "Администратор")
                    {
                        System.Console.WriteLine(persons.Id);
                        var customersWindows = new AdminWindow(persons.Id);
                        customersWindows.Show();
                        this.Close();
                    }
                    else
                    {
                        System.Console.WriteLine("ничего");
                    }
                }
            }
            else
            {   
                var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Ошибка входа. Проверьте логин или пароль", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
                window.ShowDialog(this);
            }

        }
    }
}   