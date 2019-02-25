using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Models
{
    public class InterPersonalInfo
    {
        [Key]
        public int CandidateId { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile NO1")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo1 { get; set; }


        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        public string MobileNo2 { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Enter Date Of Birth")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Enter Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please Enter Gender")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(6)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Select MaritalStaus")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string MaritalStaus { get; set; }
        
        public int NoOfChildren { get; set; }

        [Required(ErrorMessage = "Please Enter Present Address")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        public string AddressPresent { get; set; }

        [Required(ErrorMessage = "Please Select Present State")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string StatePresent { get; set; }

        [Required(ErrorMessage = "Please Select Present City")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string CityPresent { get; set; }

        [Required(ErrorMessage = "Please Enter Present Pincode")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string PincodePresent { get; set; }

        [Required(ErrorMessage = "Please Enter Past Address")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        public string AddressPast { get; set; }

        [Required(ErrorMessage = "Please Select Past State")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string StatePast { get; set; }

        [Required(ErrorMessage = "Please Select Past City")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string CityPast { get; set; }

        [Required(ErrorMessage = "Please Enter Past Pincode")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string PinCodePast { get; set; }

        [Required(ErrorMessage = "Please Enter Applied For Department")]
        public string AppliedForDepartment { get; set; }

        [Required(ErrorMessage = "Please Enter Applied For Designation")]
        public string AppliedForDesignation { get; set; }

        //[Column(TypeName = "decimal(4,2)")]
        [Required(ErrorMessage = "Please Enter Total Experience In Year")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string TotalExperienceInYear { get; set; }

        //[Column(TypeName = "date")]
        [Required(ErrorMessage = "Please Enter Joining Date")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(15)]
        public string EarliestJoinDate { get; set; }

        //public DateTime? EarliestJoinDate { get; set; }
        [Required(ErrorMessage = "Please Enter Salary Expectation")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string SalaryExpectation { get; set; }

        [Required(ErrorMessage = "Please Select Vahicle")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string Vehicle { get; set; }

        [Required(ErrorMessage = "Please Check JobSource")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string JobSource { get; set; }

        [Required(ErrorMessage = "Please Select Night Shift")]
        public bool NightShift { get; set; }

        [Required(ErrorMessage = "Please Select Reference")]
        public bool IsReference { get; set; }


        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string ReferenceName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string ReferenceMobileNo { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ReferenceDesignation { get; set; }


        //[Column(TypeName = "VARCHAR")]
        //[StringLength(50)]
        //public string English { get; set; }
        //[Column(TypeName = "VARCHAR")]
        //[StringLength(50)]
        //public string Hindi { get; set; }
        //[Column(TypeName = "VARCHAR")]
        //[StringLength(50)]
        //public string Gujarati { get; set; }

        //Enter Language English
        public bool IsEnglishRead { get; set; }
        public bool IsEnglishSpeak { get; set; }
        public bool IsEnglishWrite { get; set; }
        //Enter Language Hindi
        public bool IsHindiRead { get; set; }
        public bool IsHindiSpeak { get; set; }
        public bool IsHindiWrite { get; set; }
        //Enter Language Gujarati
        public bool IsGujaratiRead { get; set; }
        public bool IsGujaratiSpeak { get; set; }
        public bool IsGujaratiWrite { get; set; }


        [Column(TypeName = "DATETIME")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Please Enter EmailId")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string EmailId { get; set; }

        [Column(TypeName = "VARCHAR")]
        //[StringLength(250)]
        public string OtherCertification { get; set; }
    }
}