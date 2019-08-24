using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.Business.DependencyResolvers.Ninject;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WinFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _productService = InstanceFactory.GetInstance<IProductService>();
            _categoryService = InstanceFactory.GetInstance<ICategoryService>();
        }

        private IProductService _productService;
        private ICategoryService _categoryService;

        private void LoadProducts()
        {
            bunifuDataGridView1.DataSource = _productService.GetAll();
        }

        private void LoadCategories()
        {
            comboBox1.DataSource = _categoryService.GetAll();
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryId";

            comboBox2.DataSource = _categoryService.GetAll();
            comboBox2.DisplayMember = "CategoryName";
            comboBox2.ValueMember = "CategoryId";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bunifuDataGridView1.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(comboBox1.SelectedValue));
            }
            catch
            {

            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                bunifuDataGridView1.DataSource = _productService.GetProductsByName(textBox1.Text);
            }
            else
            {
                LoadProducts();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Add(new Product
                {
                    CategoryId = Convert.ToInt32(comboBox2.SelectedValue),
                    ProductName = textBox2.Text,
                    UnitPrice = Convert.ToDecimal(textBox3.Text),
                    UnitsInStock = Convert.ToInt16(textBox4.Text),
                    QuantityPerUnit = textBox5.Text
                });
                MessageBox.Show("Ürün Kaydı Başarıyla Yapılmıştır.", "Ürün Kaydı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Update(new Product
                {
                    ProductId = Convert.ToInt32(bunifuDataGridView1.CurrentRow.Cells[0].Value),
                    CategoryId = Convert.ToInt32(comboBox2.SelectedValue),
                    ProductName = textBox2.Text,
                    UnitPrice = Convert.ToDecimal(textBox3.Text),
                    UnitsInStock = Convert.ToInt16(textBox4.Text),
                    QuantityPerUnit = textBox5.Text
                });
                MessageBox.Show("Ürün Güncelleme Başarıyla Yapılmıştır.", "Ürün Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void BunifuDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = bunifuDataGridView1.CurrentRow;
            textBox2.Text = row.Cells[1].Value.ToString();
            comboBox2.SelectedValue = row.Cells[2].Value;
            textBox3.Text = row.Cells[3].Value.ToString();
            textBox5.Text = row.Cells[4].Value.ToString();
            textBox4.Text = row.Cells[5].Value.ToString();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridView1.CurrentRow!=null)
            {
                try
                {
                    _productService.Delete(new Product
                    {
                        ProductId = Convert.ToInt32(bunifuDataGridView1.CurrentRow.Cells[0].Value),
                    });
                    MessageBox.Show("Ürün Silme İşlemi Başarıyla Yapılmıştır.", "Ürün Silme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message,"Ürün Silme",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    //MessageBox.Show(exception.InnerException.InnerException.Message);
                }
            }
        }
    }
}
