using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Den.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Avalonia.Platform.Storage;
using Avalonia.Media.Imaging;
using System.Threading.Tasks;
using Avalonia.Platform;

namespace Den.Views;

public partial class AdminEditProductsWindow : Window
{
    private string? tempimgPth;
    private string? tempFileName;
    public AdminEditProductsWindow()
    {
        InitializeComponent();
        this.Title.Text = "Добавление товара";
        LoadAllBox();
        this.Img.Source = new Bitmap(AssetLoader.Open(new Uri("avares://Den/Assets/picture.png")));

    }
    public AdminEditProductsWindow(string articule)
    {
        InitializeComponent();  
        Articule.IsEnabled = false;
         
        LoadAllBox();    
        LoadData(articule); 
    }
    public void LoadData(string articule)
    {
        using(var db = new DemoContext())
        {
            var products = db.Products.Include(x => x.Suppliers).Include(x => x.Gender).Include(x => x.Manufactures).FirstOrDefault(x => x.Articule == articule);
            this.Articule.Text = products.Articule;
            System.Console.WriteLine(products.ProductsTypeId);
            var type_tovar = db.ProductsTypes.FirstOrDefault(x=> x.Id == products.ProductsTypeId);


            System.Console.WriteLine(type_tovar.Name);
            this.Type.SelectedItem = type_tovar.Name;
            this.Unit.Text = products.Unit;
            this.Price.Value = products.Price;
            this.Suppliers.SelectedItem = products.Suppliers.Name;
            this.Manufactures.SelectedItem = products.Manufactures.Name;
            this.Gender.SelectedItem = this.Gender.Items
            .OfType<ComboBoxItem>()
            .FirstOrDefault(x => Convert.ToInt32(x.Tag) == products.GenderId);
            this.Discount.Value = products.Discount;
            this.Qty.Value = products.Qty;
            if (!string.IsNullOrEmpty(products.ImgPth) && products.ImgPth != "NULL")
            {
           
                string hardFolderPath = @"C:\Users\HP\DEMO\День1\Den\Assets\"; 
                string fullImagePath = Path.Combine(hardFolderPath, products.ImgPth);

                if (File.Exists(fullImagePath))
                {
                    this.Img.Source = new Bitmap(fullImagePath);
                }
            
                else 
                {
                    try 
                    {
                        this.Img.Source = new Bitmap(AssetLoader.Open(new Uri("avares://Den/Assets/" + products.ImgPth)));
                    }
                    catch 
                    {
                       
                        this.Img.Source = new Bitmap(AssetLoader.Open(new Uri("avares://Den/Assets/picture.png")));
                    }
                }
            }
            else
            {
                this.Img.Source = new Bitmap(AssetLoader.Open(new Uri("avares://Den/Assets/picture.png")));
            }
            this.Description.Text = products.Description;

        }
    }
    public void LoadAllBox()
    {
        using(var db = new DemoContext())
        {
            var type = db.ProductsTypes.ToList();
            foreach(var t in type)
            {
                this.Type.Items.Add(t.Name);
            }
            var suppliers = db.Suppliers.ToList();
            foreach(var t in suppliers)
            {
                this.Suppliers.Items.Add(t.Name);
            }
            var manufactures = db.Manufactures.ToList();
            foreach(var t in manufactures)
            {
                this.Manufactures.Items.Add(t.Name);
            }
            var genders = db.Genders.ToList();
            foreach(var g in genders)
            {
                this.Gender.Items.Add(new ComboBoxItem() 
                { 
                    Content = g.Name, 
                    Tag = g.Id 
                });
            }
        }
    }
    public void Button_Click1(object s, RoutedEventArgs e)
    {
        this.Close();
        
    }

    public void Edit_Tovar()
    {
        if(!string.IsNullOrWhiteSpace(this.Articule.Text) && !string.IsNullOrWhiteSpace(this.Unit.Text) && !string.IsNullOrWhiteSpace(this.Description.Text) &&
        Type.SelectedItem is string type && Suppliers.SelectedItem is string suppliers && Manufactures.SelectedItem is string manufactures &&
        Gender.SelectedItem is ComboBoxItem selectedGender &&
        Price.Value.HasValue && Qty.Value.HasValue && Discount.Value.HasValue)
        {
            using(var db = new DemoContext())
            {
                var existingProduct = db.Products.FirstOrDefault(x => x.Articule == Articule.Text);
                var products_ = db.Products.ToList();
                try
                {

                    System.Console.WriteLine("A tyt");
                    var type_ = db.ProductsTypes.FirstOrDefault(x=> x.Name == type);
                    var suppliers_ = db.Suppliers.FirstOrDefault(x => x.Name == suppliers);
                    var manufactures_ = db.Manufactures.FirstOrDefault(x => x.Name == manufactures);
                    

                   System.Console.WriteLine("A tyt2");
                    int genderId = Convert.ToInt32(selectedGender.Tag);

                    existingProduct.ProductsTypeId = type_?.Id;
                    existingProduct.Unit = Unit.Text;
                    existingProduct.Price = Convert.ToDecimal(Price.Value.Value);
                    existingProduct.SuppliersId = suppliers_?.Id;
                    existingProduct.ManufacturesId = manufactures_?.Id;
                    existingProduct.GenderId = genderId;
                    existingProduct.Discount = Convert.ToInt32(Discount.Value.Value);
                    existingProduct.Qty = Convert.ToInt32(Qty.Value.Value);
                    existingProduct.Description = Description.Text;

            
                    if (tempFileName != null)
                    {
                        existingProduct.ImgPth = tempFileName;
                    }

                    System.Console.WriteLine("A tyt3");

                    db.SaveChanges();

                    string destFolder = @"C:\Users\HP\DEMO\День1\Den\Assets\"; 
        
                    if (!string.IsNullOrEmpty(tempimgPth) && !string.IsNullOrEmpty(tempFileName))
                    {
                        string fullDestPath = Path.Combine(destFolder, tempFileName);
                        File.Copy(tempimgPth, fullDestPath, true); 
                    }

                    var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Успешное изменение товара", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
                    window.ShowDialog(this);

                    this.Close();
                }
                catch
                {
                    var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Ошибка, проверьте написание полей!!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
                    window.ShowDialog(this);
                }

                
            }
        }

        else
        {
            var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Заполните все поля!!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
            window.ShowDialog(this);
        }

    }

    public void Add_Tovar()
    {
        if(!string.IsNullOrWhiteSpace(this.Articule.Text) && !string.IsNullOrWhiteSpace(this.Unit.Text) && !string.IsNullOrWhiteSpace(this.Description.Text) &&
        Type.SelectedItem is string type && Suppliers.SelectedItem is string suppliers && Manufactures.SelectedItem is string manufactures &&
        Gender.SelectedItem is ComboBoxItem selectedGender &&
        Price.Value.HasValue && Qty.Value.HasValue && Discount.Value.HasValue)
        {
            using(var db = new DemoContext())
            {
                var products_ = db.Products.ToList();
                try
                {

                    System.Console.WriteLine("A tyt");
                    var type_ = db.ProductsTypes.FirstOrDefault(x=> x.Name == type);
                    var suppliers_ = db.Suppliers.FirstOrDefault(x => x.Name == suppliers);
                    var manufactures_ = db.Manufactures.FirstOrDefault(x => x.Name == manufactures);
                    

                   System.Console.WriteLine("A tyt2");

                    if(tempFileName == null)
                    {
                        tempFileName = "picture.png";
                    }

                   
                    int genderId = Convert.ToInt32(selectedGender.Tag);

                    db.Products.Add(new Product()
                    {
                        Articule = Articule.Text,
                        ProductsTypeId = type_?.Id,
                        Unit = Unit.Text,
                        Price = Convert.ToDecimal(Price.Value.Value),
                        SuppliersId = suppliers_?.Id,
                        ManufacturesId = manufactures_?.Id,
                        GenderId = genderId,
                        Discount = Convert.ToInt32(Discount.Value.Value),
                        Qty = Convert.ToInt32(Qty.Value.Value),
                        Description = Description.Text,
                        ImgPth = tempFileName

                    });

                    System.Console.WriteLine("A tyt3");

                    db.SaveChanges();

                    string destFolder = @"C:\Users\HP\DEMO\День1\Den\Assets\"; 
        
                    if (!string.IsNullOrEmpty(tempimgPth) && !string.IsNullOrEmpty(tempFileName))
                    {
                        string fullDestPath = Path.Combine(destFolder, tempFileName);
                        File.Copy(tempimgPth, fullDestPath, true); 

                    }

                    var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Успешное добавление товара", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
                    window.ShowDialog(this);

                    this.Close();
                }
                catch
                {
                    var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Ошибка, проверьте написание полей!!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
                    window.ShowDialog(this);
                }

                
            }
        }

        else
        {
            var window = new Window(){Width = 350, Height= 200 ,Content = new TextBlock(){ Text = "Заполните все поля!!", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center}};
            window.ShowDialog(this);
        }

    }

    public void Button_Click_Save(object s, RoutedEventArgs e)
    {
        if(this.Title.Text == "Добавление товара")
        {
            Add_Tovar();
        }
        else
        {
            Edit_Tovar();
        }

        
    }

    public async void Button_Click_Add(object s, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if(topLevel == null) 
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
        }
        );

        if(files.Count >= 1)
        {
            var file = files[0];

            tempimgPth = file.Path.LocalPath;
            tempFileName = file.Name;

            System.Console.WriteLine(tempFileName);

            await using(var steam = await file.OpenReadAsync())
            {
                Img.Source = new Bitmap(steam);
            }
        }

    }


}