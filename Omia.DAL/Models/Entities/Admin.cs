using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Omia.DAL.Models.Base;
using Omia.DAL.Models.Enums;

namespace Omia.DAL.Models.Entities
{
    [Display(Name = "المسؤول")]
    public class Admin : BaseUserEntity
    {
        [Display(Name = "الصلاحيات")]
        public AdminPermissions Permissions { get; set; } = AdminPermissions.None;
    }
}
