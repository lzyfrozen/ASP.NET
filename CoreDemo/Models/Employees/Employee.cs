using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class Employee
    {
        public Employee() { }

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>        
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
