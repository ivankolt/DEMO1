using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Den.Models;
using Microsoft.EntityFrameworkCore;


namespace Den.Views;

public partial class ManagerProductsWindow : Window
{
    public int ID = 0;
    public ManagerProductsWindow(int id)
    {
        InitializeComponent();
        LoadFIO(id);
        ID = id;
        LoadSuppliers();
        LoadData(null,null,null);
    }
    public void LoadFIO(int id)
    {
        try
        {
        using(var db = new DemoContext())
        {
            var persons = db.Users.Include(x => x.Persons).FirstOrDefault(x => x.Id == id);

            string FIO1 = persons.Persons?.FirstName + " " + persons.Persons?.LastName + " " + persons.Persons?.MiddleName + " ";

            this.FIO.Text = FIO1;
        }
        }
        catch
        {
            System.Console.WriteLine("ошибка");
        }
    }
    public void LoadSuppliers()
    {
        using(var db = new DemoContext())
        {
            var type = db.Suppliers.ToList();
            foreach(var t in type)
            {
                this.Suppliers.Items.Add(t.Name);
            }
        }

        this.Suppliers.Items.Add("Все поставщики");
    }

    public void TextChanged(object s, TextChangedEventArgs   e)
    {

       string text = this.text_.Text ?? "";

        string suppliers = this.Suppliers.SelectedItem as string ?? "";
        if (suppliers == "Все поставщики")
            suppliers = "";
       
        string sort = GetSort();
        if (sort == "По умолчанию")
            sort = "";
        

        LoadData(text, suppliers, sort);
    }

    public void Suppliers_SelectionChanged(object s, SelectionChangedEventArgs e)
    {
      

        string text = this.text_.Text ?? "";

        string suppliers = this.Suppliers.SelectedItem as string ?? "";
        if (suppliers == "Все поставщики")
            suppliers = "";
       
        string sort = GetSort();
        if (sort == "По умолчанию")
            sort = "";
        

        LoadData(text, suppliers, sort);
    }
    private string GetSort()
    {
        if (Sort.SelectedItem is ComboBoxItem item)
            return item.Content?.ToString() ?? "";
        return "";
    }

    public void Sort_SelectionChanged(object s, SelectionChangedEventArgs e)
    {

        string text = this.text_.Text ?? "";

        string suppliers = this.Suppliers.SelectedItem as string ?? "";
        if (suppliers == "Все поставщики")
            suppliers = "";
       
        string sort = GetSort();
        if (sort == "По умолчанию")
            sort = "";
        

        LoadData(text, suppliers, sort);
    }

    public void LoadData(string text, string suppliers, string sort)
    {   
        this.Products.Children.Clear();
  
        using(var db = new DemoContext())
        {
            var query = db.Products.Include(x=> x.Suppliers).Include(x=> x.Manufactures).Include(x=> x.Gender).AsQueryable();

            if(!string.IsNullOrWhiteSpace(text))
            {
          
                query = query.Where(x => x.Articule.Contains(text) || x.Unit.Contains(text) || x.Suppliers.Name.Contains(text)
                || x.Manufactures.Name.Contains(text) || x.Gender.Name.Contains(text) || x.Description.Contains(text)
                 );
            }

            if(!string.IsNullOrWhiteSpace(suppliers))
            {
                 query = query.Where(x => x.Suppliers.Name == suppliers);
            }

            if(!string.IsNullOrWhiteSpace(sort))
            {
                if (sort == "По возрастанию")
                    query = query.OrderBy(x => x.Qty);
                else if (sort == "По убыванию")
                    query = query.OrderByDescending(x => x.Qty);
            }

            foreach(var i in query.ToList())
            {
                
                
                var type = db.ProductsTypes.FirstOrDefault(x => x.Id == i.ProductsTypeId);
                Border border = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brush.Parse("#000000"),
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                    
                };

                if(i.Discount > 15)
                {
                    border.Background = Brush.Parse("#2E8B57");
                }

                Grid grid = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitions("Auto, Auto, Auto"),
                    VerticalAlignment = VerticalAlignment.Center

                }; 
                
                
                var img = new Image()
                {
                    Width = 190,
                    Height = 190,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                try
                {
                    string img_ = i.ImgPth;
                    string new_img = img_.Trim();

                    System.Console.WriteLine(new_img);
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; 

                    string fullImagePath = Path.Combine(baseDirectory, "Assets", i.ImgPth);

                    if (File.Exists(fullImagePath))
                    {
                        img.Source = new Avalonia.Media.Imaging.Bitmap(fullImagePath);
                    }
                    else
                    {

                        if(new_img != "" && new_img != null && new_img != "NULL")
                        {
                            img.Source = new Bitmap(AssetLoader.Open(new System.Uri("avares://Den/Assets/" + i.ImgPth)));
                        }
                        
                        else
                        {
                            System.Console.WriteLine("Я тут");
                            img.Source = new Bitmap(AssetLoader.Open(new System.Uri("avares://Den/Assets/picture.png")));
                        }
                    }
                }
                catch
                {
                    
                }

                Grid.SetColumn(img,0);
                grid.Children.Add(img);

                var main = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brush.Parse("#000000"),
                    Margin = new Thickness(2),
                    Width = 300
                    
                };

                var mainStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };

                mainStackPanel.Children.Add(new TextBlock(){Text = type.Name + " | " + i.Articule, FontWeight = FontWeight.Bold, FontSize = 20});
                mainStackPanel.Children.Add(new TextBlock(){Text = "Описание товара: " + i.Description, FontSize = 15, TextWrapping = TextWrapping.Wrap});
                mainStackPanel.Children.Add(new TextBlock(){Text = "Производитель: " + i.Manufactures?.Name, FontSize = 15});
                mainStackPanel.Children.Add(new TextBlock(){Text = "Поставщик: " + i.Suppliers?.Name, FontSize = 15});

                if(i.Discount > 0)
                {
                    var stack = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };
                    stack.Children.Add(new TextBlock(){Text = "Цена: ", FontSize = 15});
                    
                    stack.Children.Add(new TextBlock(){Text = Convert.ToString(i.Price) + "    ", FontSize = 15, TextDecorations = TextDecorations.Strikethrough, Foreground = Brush.Parse("#ff0000")});

                    decimal? price = i.Price;
                    decimal? new_price = price - (price * Convert.ToDecimal((i.Discount/100.00)));

                    stack.Children.Add(new TextBlock(){Text = Convert.ToString(new_price), FontSize = 15,});

                    mainStackPanel.Children.Add(stack);
                }
                else
                {
                    mainStackPanel.Children.Add(new TextBlock(){Text = "Цена: " + i.Price, FontSize = 15});
                }
                mainStackPanel.Children.Add(new TextBlock(){Text = "Еденица измерения: " + i.Unit, FontSize = 15});

                if(i.Qty == 0)
                {
                    mainStackPanel.Children.Add(new TextBlock(){Text = "Количество на складе: " + i.Qty, FontSize = 15, Background = Brush.Parse("#00a2ff")});
                }
                else
                {
                    mainStackPanel.Children.Add(new TextBlock(){Text = "Количество на складе: " + i.Qty, FontSize = 15});
                }

                main.Child = mainStackPanel;

                Grid.SetColumn(main,1);
                grid.Children.Add(main);

                var main_discount = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brush.Parse("#000000"),
                    Margin = new Thickness(2),
                    Width = 180
                };

                main_discount.Child = new TextBlock(){Text = "Дейсвующая скидка: " + i.Discount, FontSize = 13, TextWrapping = TextWrapping.Wrap, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center};

                Grid.SetColumn(main_discount,2);
                grid.Children.Add(main_discount);


                border.Child = grid;
                Products.Children.Add(border);

            }

        }
    }


    public void Button_Click1(object s, RoutedEventArgs e)
    {
        ManagerWindow mainWindow = new ManagerWindow(this.ID);
        mainWindow.Show();
        this.Close();
    }
}