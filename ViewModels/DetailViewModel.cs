using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CodeMobileChallenge.Models.ProductModel;
using System.Windows;

namespace CodeMobileChallenge.ViewModels
{
    internal class DetailViewModel : ViewModelBase
    {
        private Product product;
        public Product Product
        {
            get => product;
            set
            {
                product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public DetailViewModel(int id)
        {
            FetchProductDetails(id);
        }

        private void FetchProductDetails(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("CMReq", "request");
                try
                {
                    HttpResponseMessage httpResponse = client.GetAsync($"https://dummyjson.com/products/{id}").Result;
                    httpResponse.Headers.Add("CMERes", "response");
                    httpResponse.EnsureSuccessStatusCode();
                    string responseBody = httpResponse.Content.ReadAsStringAsync().Result;
                    Product = JsonConvert.DeserializeObject<Product>(responseBody);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching product details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
