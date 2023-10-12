using System.ComponentModel.DataAnnotations;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class UserInputDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        [DateRange("1900-01-01")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        public long ClassId { get; set; }

        [Required(ErrorMessage = "Trường {0} không được trống!")]
        public string Avatar { get; set; }
    }
}