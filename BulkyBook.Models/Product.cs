using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1,10000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price50 {  get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price100 { get; set;}

		[ValidateNever]
		public string ImageUrl { get; set; }

        #region ForeginKey relationship
        //we need foreign key relation - each book should be related to a category
        //EF Core handles this for us: It knows 'Category' and 'CategoryId' and relates 
        [Required]
        public int CategoryId { get; set; }
        //[ForeignKey("CategoryId")] : we don't need to explicitly call this - EF Core automatically maps two properties.
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        public int CoverTypeId { get; set; }
		[ValidateNever]
		public CoverType CoverType { get; set; }
        #endregion

    }
}
