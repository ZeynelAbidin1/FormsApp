using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
    public class Product 
    {
        [Display(Name= "Ürün Id")]
        
        public int ProductId {  get; set; }
        [Required(ErrorMessage = "Ürün Adını Giriniz.")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage ="Fiyat Bilgisini Giriniz.")]
        [Display(Name = "Fiyat")]
        public decimal? Price { get; set; }
        //[Required(ErrorMessage ="Ürün Görsel Adını Yazınız.Örn:1.jpg")]
        [Display(Name = "Ürün Görseli")]
        public string? Image{ get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage ="Ürünün Bulunduğu Kategoriyi Seçiniz.")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
    }
}
