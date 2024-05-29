using System;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using static CodeMobileChallenge.Models.ProductModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CodeMobileChallenge.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                //FilterProducts();
                StartSearchTimer();
            }
        }
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
                //UpdateTotalPrice();
            }
        }
        private string totalPriceMessage;
        public string TotalPriceMessage
        {
            get => totalPriceMessage;
            set
            {
                totalPriceMessage = value;
                OnPropertyChanged(nameof(TotalPriceMessage));
            }
        }

        private ICollectionView filteredProducts;
        public ICollectionView FilteredProducts
        {
            get => filteredProducts;
            set
            {
                filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }
        private bool isTotalPriceVisible;
        public bool IsTotalPriceVisible
        {
            get => isTotalPriceVisible;
            set
            {
                isTotalPriceVisible = value;
                OnPropertyChanged(nameof(IsTotalPriceVisible));
            }
        }

        private string selectedMenuItem;
        public string SelectedMenuItem
        {
            get => selectedMenuItem;
            set
            {
                selectedMenuItem = value;
                OnPropertyChanged(nameof(SelectedMenuItem));
                // เพิ่มโค้ดสำหรับปรับปรุง UI หรือดำเนินการอื่นๆ เมื่อเลือกเมนู
                ExecuteComboBoxSelectionChanged(selectedMenuItem);
            }
        }
        private decimal totalPrice;
        public decimal TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        public List<string> MenuItems { get; private set; }
        public ICommand ComboBoxSelectionChangedCommand { get; private set; }
        public ICommand ToggleTotalPriceVisibilityCommand { get; private set; }
        public ICommand DetailCommand { get; private set; }
        private System.Timers.Timer searchTimer; // เพิ่ม Timer นี้

        public HomeViewModel()
        {
            IsTotalPriceVisible = false;
            Products = new ObservableCollection<Product>();
            FetchProducts();
            FilteredProducts = CollectionViewSource.GetDefaultView(Products);
            MenuItems = new List<string>
            {
                "ทั้งหมด",
                "ราคามากว่า 1000",
                "แสดงราคารวมต่อชิ้น",
                "เรียงเรตติ้ง",
                "แสดงราคารวมทั้งหมด"
            };
            SelectedMenuItem = MenuItems.FirstOrDefault(); // กำหนดค่าตั้งต้นเป็นเมนูแรกในรายการ
            ComboBoxSelectionChangedCommand = new RelayCommand<string>(ExecuteComboBoxSelectionChanged);
            DetailCommand = new RelayCommand<Product>(ShowDetail);
            ToggleTotalPriceVisibilityCommand = new RelayCommand(ToggleTotalPriceVisibility);

            // สร้าง Timer สำหรับ Debounce Time 1 วินาที
            searchTimer = new System.Timers.Timer();
            searchTimer.Interval = 1000; // 1 วินาที
            searchTimer.AutoReset = false; // ไม่ต้องเริ่มต้นนับเวลาอัตโนมัติ
            searchTimer.Elapsed += SearchTimer_Elapsed;
        }
        private void ToggleTotalPriceVisibility()
        {
            IsTotalPriceVisible = !IsTotalPriceVisible;
        }

        private void StartSearchTimer()
        {
            searchTimer.Stop(); // หยุดนับเวลาเก่า (ถ้ามี)
            searchTimer.Start(); // เริ่มนับเวลาใหม่
        }
        private void FetchProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("CMReq", "request");

                try
                {
                    HttpResponseMessage httpResponse = client.GetAsync("https://dummyjson.com/products").Result;

                    // เพิ่ม header key "CMERes" และตั้งค่าเป็น "response" ใน Response
                    httpResponse.Headers.Add("CMERes", "response");

                    httpResponse.EnsureSuccessStatusCode(); // ตรวจสอบสถานะการ Response

                    string response = httpResponse.Content.ReadAsStringAsync().Result;
                    var productResponse = JsonConvert.DeserializeObject<ProductResponse>(response);
                    foreach (var product in productResponse.products)
                    {
                        product.totalPrice = product.price * product.stock;
                        Products.Add(product);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteComboBoxSelectionChanged(string selectedItem)
        {
            FilterProducts(selectedItem);
        }
        private void SearchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // ค้นหาสินค้าเมื่อ Timer เริ่มต้นนับเวลา
            Application.Current.Dispatcher.Invoke(() => FilterProducts(selectedMenuItem));
        }
        private void FilterProducts(string type)
        {
            TotalPriceMessage = string.Empty;
            FilteredProducts.SortDescriptions.Clear();
            switch (type)
            {
                case "ราคามากว่า 1000":
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        FilteredProducts.Filter = (obj) =>
                        {
                            if (obj is Product product)
                            {
                                return product.price > 1000 && product.discountPercentage > 0;
                            }
                            return false;
                        };
                    }
                    else
                    {
                        FilteredProducts.Filter = (obj) =>
                        {
                            if (obj is Product product)
                            {
                                return product.title.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 && product.price > 1000 && product.discountPercentage > 0;
                            }
                            return false;
                        };
                    }
                    break;
                case "แสดงราคารวมต่อชิ้น":
                    CalculateTotalPricePerItem(FilteredProducts);
                    break;
                case "เรียงเรตติ้ง":
                    FilteredProducts.SortDescriptions.Add(new SortDescription(nameof(Product.rating), ListSortDirection.Descending));
                    FilteredProducts.SortDescriptions.Add(new SortDescription(nameof(Product.price), ListSortDirection.Ascending));
                    break;
                case "แสดงราคารวมทั้งหมด":
                    CalculateTotalPrice(FilteredProducts);
                    break;
                default:
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        FilteredProducts.Filter = null;
                    }
                    else
                    {
                        FilteredProducts.Filter = (obj) =>
                        {
                            if (obj is Product product)
                            {
                                return product.title.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
                            }
                            return false;
                        };
                    }
                    break;
            }
            FilteredProducts.Refresh();
        }
        private void CalculateTotalPrice(ICollectionView collectionView)
        {
            decimal totalPrice = 0;
            if (collectionView != null)
            {
                foreach (var item in collectionView)
                {
                    if (item is Product product)
                    {
                        totalPrice += (decimal)product.price;
                    }
                }
            }
            TotalPriceMessage = $"ราคารวมทั้งหมด: {string.Format("{0:N}", totalPrice)}";
        }

        private void CalculateTotalPricePerItem(ICollectionView collectionView)
        {
            ToggleTotalPriceVisibility();
            //if (collectionView != null)
            //{
            //    foreach (var item in collectionView)
            //    {
            //        if (item is Product product)
            //        {
            //            // คำนวณ totalPrice และกำหนดค่าให้กับ TotalPrice ของแต่ละสินค้า
            //            product.totalPrice = product.price * product.stock;
            //        }
            //    }
            //    ToggleTotalPriceVisibility();
            //}
        }

        private void ShowDetail(Product product)
        {
            MessageBox.Show($"Showing details for {product.id}");
        }
    }
}
