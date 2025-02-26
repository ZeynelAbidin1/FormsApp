using System.Diagnostics;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.IO;
using System.Reflection;

namespace FormsApp.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
            
        }
        [HttpGet]
        public IActionResult Index(string searchString, string category)
        {
            var products = Repository.Products;
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.searchString =searchString;
                products = products.Where(p => p.Name.ToLower().Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(category) && category!="0"){
                products = products.Where(p=> p.CategoryId == int.Parse(category)).ToList();
            }

           // ViewBag.Categories= new SelectList(Repository.Categories,"CategoryId","Name",category);
            var model = new ProductViewModel{
                Products = products,
                Categories = Repository.Categories,
                selectedCategory = category
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product model,IFormFile imageFile){



            var extension = "";
            if (imageFile != null)
            {
                var allowedExtension = new[] { ".jpg", ".jpeg", ".png" };
                extension = Path.GetExtension(imageFile.FileName);//abc.jpg
                if (!allowedExtension.Contains(extension))
                {
                    ModelState.AddModelError("", "Ge�erli bir resim se�in. .jpg .jpeg .png");
                }
            }

            if (ModelState.IsValid){
                if (imageFile != null)
                {
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.Image = randomFileName;
                    model.ProductId = Repository.Products.Count + 1;
                    Repository.CreateProduct(model);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
            return View(model);
            
        }
        public IActionResult Edit(int? id)
        {
            if (id == null) {
                return NotFound();
            }
            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity == null) {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) {
                if (imageFile != null)
                {
                    var allowedExtension = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(imageFile.FileName);//abc.jpg
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.Image = randomFileName;
                }
                Repository.EditProduct(model);
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
            return View(model);

        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
            if (entity == null)
            {
                return NotFound();
            }
            return View("DeleteConfirm",entity);
        }
        [HttpPost]
        public IActionResult Delete(int id, int ProductId)
        {
            if (id != ProductId)
            {
                return NotFound();
            }
            var entity = Repository.Products.FirstOrDefault(p => p.ProductId == ProductId);
            if (entity == null)
            {
                return NotFound();
            }
            Repository.DeleteProduct(entity);
            return RedirectToAction("Index");
        }
        public IActionResult EditProducts(List<Product> Products){
            foreach(var product in Products){
                Repository.EditIsActive(product);
            }
            return RedirectToAction("Index");
        }
    }
    
}
