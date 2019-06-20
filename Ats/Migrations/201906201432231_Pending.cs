namespace Ats.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pending : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        StateId = c.Int(nullable: false),
                        CityName = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.CityId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesignationId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        DesignationName = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.DesignationId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedBackId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        InterviewDate = c.DateTime(nullable: false),
                        CandidateStatus = c.Boolean(nullable: false),
                        OtherComments = c.String(),
                        IsFeddbackAdded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FeedBackId);
            
            CreateTable(
                "dbo.InterEducBackgrounds",
                c => new
                    {
                        EducationalId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        BoardUniversityName = c.String(maxLength: 50, unicode: false),
                        CourseDegreeName = c.String(maxLength: 50, unicode: false),
                        PassingYear = c.String(maxLength: 50, unicode: false),
                        GradePercentage = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.EducationalId);
            
            CreateTable(
                "dbo.InterLanguages",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        LanguageType = c.String(),
                        Read = c.String(),
                        Speak = c.String(),
                        Write = c.String(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            CreateTable(
                "dbo.InterPersonalInfoes",
                c => new
                    {
                        CandidateId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 50, unicode: false),
                        MobileNo1 = c.String(nullable: false, maxLength: 15, unicode: false),
                        MobileNo2 = c.String(maxLength: 15, unicode: false),
                        DateOfBirth = c.String(nullable: false, maxLength: 15, unicode: false),
                        Age = c.Int(nullable: false),
                        Gender = c.String(nullable: false, maxLength: 6, unicode: false),
                        MaritalStaus = c.String(nullable: false, maxLength: 10, unicode: false),
                        NoOfChildren = c.Int(),
                        AddressPresent = c.String(nullable: false, maxLength: 250, unicode: false),
                        StatePresent = c.String(nullable: false, maxLength: 20, unicode: false),
                        CityPresent = c.String(nullable: false, maxLength: 20, unicode: false),
                        PincodePresent = c.String(nullable: false, maxLength: 10, unicode: false),
                        AddressPast = c.String(nullable: false, maxLength: 250, unicode: false),
                        StatePast = c.String(nullable: false, maxLength: 20, unicode: false),
                        CityPast = c.String(nullable: false, maxLength: 20, unicode: false),
                        PinCodePast = c.String(nullable: false, maxLength: 10, unicode: false),
                        AppliedForDepartment = c.String(nullable: false),
                        AppliedForDesignation = c.String(nullable: false),
                        TotalExperienceInYear = c.String(nullable: false, maxLength: 10, unicode: false),
                        EarliestJoinDate = c.String(nullable: false, maxLength: 15, unicode: false),
                        SalaryExpectation = c.String(nullable: false, maxLength: 20, unicode: false),
                        Vehicle = c.String(nullable: false, maxLength: 10, unicode: false),
                        JobSource = c.String(nullable: false, maxLength: 20, unicode: false),
                        NightShift = c.Boolean(nullable: false),
                        IsReference = c.Boolean(nullable: false),
                        ReferenceName = c.String(maxLength: 40, unicode: false),
                        ReferenceMobileNo = c.String(maxLength: 20, unicode: false),
                        ReferenceDesignation = c.String(maxLength: 50, unicode: false),
                        CreatedDate = c.DateTime(nullable: false),
                        EmailId = c.String(nullable: false, maxLength: 50, unicode: false),
                        OtherCertification = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.CandidateId);
            
            CreateTable(
                "dbo.InterPreEmpDetails",
                c => new
                    {
                        EmploymentId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        CompanyName = c.String(maxLength: 50, unicode: false),
                        City = c.String(maxLength: 50, unicode: false),
                        Designation = c.String(maxLength: 50, unicode: false),
                        WorkFrom = c.String(maxLength: 50, unicode: false),
                        WorkTo = c.String(maxLength: 50, unicode: false),
                        CtcMonth = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.EmploymentId);
            
            CreateTable(
                "dbo.InterReferences",
                c => new
                    {
                        ReferenceId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        PersonName = c.String(maxLength: 50, unicode: false),
                        CompanyName = c.String(maxLength: 50, unicode: false),
                        Designation = c.String(maxLength: 50, unicode: false),
                        ContactNo = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ReferenceId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        StateName = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.StateId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.States");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.InterReferences");
            DropTable("dbo.InterPreEmpDetails");
            DropTable("dbo.InterPersonalInfoes");
            DropTable("dbo.InterLanguages");
            DropTable("dbo.InterEducBackgrounds");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Designations");
            DropTable("dbo.Departments");
            DropTable("dbo.Cities");
        }
    }
}
