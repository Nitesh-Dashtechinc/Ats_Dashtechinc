var counter = 2;
var counter1 = 2;
var counter2 = 2;
var counter3 = 2;
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    //document.getElementById("PersonalInfo_NoOfChildren").value = 0;
    $('.divrefernce').hide();

    $('.only-number').bind('keyup paste', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $('.number-dot').bind('keyup paste', function () {
        this.value = this.value.replace(/[^0-9.]/g, '');
    });

    //All DatePicker
    //ref:https://bootstrap-datepicker.readthedocs.io/en/latest/
    //Sel Default date today
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true
    }).on('change', function () {
    });

    dt = new Date()
    dt.setFullYear(dt.getFullYear() - 18)

    $('#PersonalInfo_DateOfBirth').datepicker('setDate', dt);
    $('#PersonalInfo_DateOfBirth').datepicker('setEndDate', dt);
    $('#PersonalInfo_EarliestJoinDate').datepicker('setDate', 'today');
    $('#PersonalInfo_EarliestJoinDate').datepicker('setStartDate', 'today');
    $('#WorkFrom').datepicker('setDate', 'today');
    $('#WorkFrom').datepicker('setEndDate', 'today');
    $('#WorkTo').datepicker('setDate', 'today');
    $('#WorkTo').datepicker('setEndDate', 'today');

    $('#PersonalInfo_DateOfBirth').datepicker({
        format: 'dd/mm/yyyy',
        endDate: dt,
        autoclose: true,
        clearBtn: true,
    }).on('change', function (e) {
        var birthDate = $(this).val().toString();
        var date = moment(birthDate, "DD/MM/YYYY");
        var age = setAge(moment(date));
        var age1 = Math.floor(age);
        if (age1 !== null) {
            document.getElementById("Age").innerHTML = age1;
        }
    }).inputmask('99/99/9999');



    $('#PersonalInfo_EarliestJoinDate').datepicker({
        //format: 'mm/dd/yyyy',
        //startDate: new Date(),
        clearBtn: true,
        autoclose: true
    }).inputmask('99/99/9999');

    $('#WorkFrom').datepicker({
        format: 'dd/mm/yyyy',
        datesDisabled: new Date(),
        startDate: new Date(),
        clearBtn: true,
        autoclose: true
    }).on('change', function () {
        $("#WorkFrom").valid();
    });

    $('#WorkTo').datepicker({
        format: 'dd/mm/yyyy',
        endDate: new Date(),
        datesDisabled: new Date(),
        clearBtn: true,
        autoclose: true
    }).on('change', function () {
        $("#WorkTo").valid();
    });


    $(".add-row").on("click", function () {
        var tableId = $(this).parents('table').attr('id');
        var newRow = $("<tr>");
        var cols = "";
        switch (tableId) {
            case "previousEmploymentDetails":
                var te1 = $('#frmpredetail').valid();
                if ($('#frmpredetail').valid()) {
                    if (counter == 2)
                        $('#previousEmploymentDetails tfoot').empty();
                    $('#previousEmploymentDetails tbody').append('<tr><td></td><td>' + $('#CompanyName').val() + '</td><td>' + $('#City').val() + '</td><td>' + $('#Designation').val() + '</td><td>' + $('#WorkFrom').val() + '</td><td>' + $('#WorkTo').val() + '</td><td>' + $('#CtcMonth').val() + '</td><td><button type="button" class="btn btn-md btn-danger fa fa-trash ibtn-Del"></button></td></tr>');
                    counter++;
                    $('#frmpredetail')[0].reset();
                    $('#WorkFrom').datepicker('setDate', 'today');
                    $('#WorkFrom').datepicker('setEndDate', 'today');
                    $('#WorkTo').datepicker('setDate', 'today');
                    $('#WorkTo').datepicker('setEndDate', 'today');
                    $("form#frmpredetail :input").each(function () {
                        $(this).removeClass('is-valid').addClass('is-invalid');
                    });
                }
                break;
            case "reference":
                var reg = $('#frmReference').valid();
                if ($('#frmReference').valid()) {
                    if (counter1 == 2)
                        $('#reference tfoot').empty();
                    $('#reference tbody').append('<tr><td></td><td>' + $('#PersonName').val() + '</td><td>' + $(this).closest('tr').find('#Designation').val() + '</td><td>' + $(this).closest('tr').find('#CompanyName').val() + '</td><td>' + $('#ContactNo').val() + '</td><td><button type="button" class="btn btn-md btn-danger fa fa-trash ibtn-Del"></button></td></tr>');
                    counter1++;
                    $('#frmReference')[0].reset();
                    $("form#frmReference :input").each(function () {
                        $(this).removeClass('is-valid').addClass('is-invalid');
                    });
                    var eduErr = document.getElementById("errRefmsg");
                    eduErr.style.display = "none"
                }
                break;
            case "educationalBackground":
                var edu = $('#frmEducationalBackground').valid();
                if ($('#frmEducationalBackground').valid()) {
                    if (counter2 == 2)
                        $('#educationalBackground tfoot').empty();
                    $('#educationalBackground tbody').append('<tr><td></td><td>' + $('#BoardUniversityName').val() + '</td><td>' + $('#CourseDegreeName').val() + '</td><td>' + $('#PassingYear').val() + '</td><td>' + $('#GradePercentage').val() + '</td><td><button type="button" class="btn btn-md btn-danger fa fa-trash ibtn-Del"></button></td></tr>');
                    counter2++;
                    $('#frmEducationalBackground')[0].reset();
                    $("form#frmEducationalBackground :input").each(function () {
                        $(this).removeClass('is-valid').addClass('is-invalid');
                    });
                    var eduErr = document.getElementById("errEdumsg");
                    eduErr.style.display = "none"
                }
                break;
            case "language":
                if ($('#Language option:selected').text() != '--Select Language--' && canadd()) {
                    var read = "No", speak = "No", write = "No";
                    if ($('#Read').is(":checked")) {
                        read = "Yes"
                    }
                    if ($('#Speak').is(":checked")) {
                        speak = 'Yes';
                    }
                    if ($('#Write').is(":checked")) {
                        write = 'Yes';
                    }
                    cols += '<td>' + $('#Language option:selected').text() + '</td>';
                    cols += '<td>' + read + '</td>';
                    cols += '<td>' + speak + '</td>';
                    cols += '<td>' + write + '</td>';
                    cols += '<td><button type="button" class="btn btn-md btn-danger fa fa-trash ibtn-Del"></button></td>';
                    $('#Read').prop('checked', true);
                    $('#Speak').prop('checked', true);
                    $('#Write').prop('checked', true);
                }
                else {
                    toastr.warning($('#Language option:selected').text() + " all ready exist");
                    cols += "";
                }
                newRow.append(cols);
                $("table.table-language").append(newRow);
                counter3++;
                break;
            default:
        }
    });


    $("table.table-pre-employeement").on("click", ".ibtn-Del", function (event) {
        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to recover this imaginary file!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $(this).closest("tr").remove();
                    counter -= 1;
                    swal("Done! Your record has been deleted!", {
                        icon: "success",
                    });
                }
            });

    });
    $("table.table-refernce").on("click", ".ibtn-Del", function (event) {
        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to recover this imaginary file!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $(this).closest("tr").remove();
                    counter1 -= 1;
                    swal("Done! Your record has been deleted!", {
                        icon: "success",
                    });
                    if (counter1 == 2) {
                        var eduErr = document.getElementById("errRefmsg");
                        eduErr.style.display = "block"
                    }
                }
            });

    });
    $("table.table-educational-background").on("click", ".ibtn-Del", function (event) {
        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to recover this imaginary file!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $(this).closest("tr").remove();
                    counter2 -= 1;
                    swal("Done! Your record has been deleted!", {
                        icon: "success",
                    });
                    if (counter2 == 2) {
                        var eduErr = document.getElementById("errEdumsg");
                        eduErr.style.display = "block"
                    }
                }
            });

    });
    $("table.table-language").on("click", ".ibtn-Del", function (event) {
        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to recover this imaginary file!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $(this).closest("tr").remove();
                    counter -= 1;
                    swal("Done! Your record has been deleted!", {
                        icon: "success",
                    });
                }
            });

    });
    $('input[type=radio][id=PersonalInfo_IsReference]').change(function () {
        if (this.value == 'True') {
            $('.divrefernce').show();
        }
    });

    $('input[type=radio][id=PersonalInfo_IsReference1]').change(function () {
        if (this.value == 'False') {
            $('.divrefernce').hide();
            $('#PersonalInfo_ReferenceName').val('');
            $('#PersonalInfo_ReferenceDesignation').val('');
            $('#PersonalInfo_ReferenceMobileNo').val('');
        }
    });

    $('#asAboveCheck').change(function () {
        var Address = $('#PersonalInfo_AddressPresent').val();
        var pincode = $('#PersonalInfo_PincodePresent').val();
        var state = $("select#PersonalInfo_StatePresent").val();
        var selectedStateText = $("#PersonalInfo_StatePresent option:selected").html();
        var city = $('select#PersonalInfo_CityPresent').val();
        var selectedCityText = $("#PersonalInfo_CityPresent option:selected").html();
        if (this.checked) {
            $('#PersonalInfo_AddressPast').val(Address);
            $('#PersonalInfo_PinCodePast').val(pincode);
            $('#PersonalInfo_StatePast').empty();
            $("#PersonalInfo_StatePast").append('<option value="' + state + '">' +
                selectedStateText + '</option>');

            $("#CityListPast").empty();
            $("#CityListPast").append('<option value="' + city + '">' +
                selectedCityText + '</option>');
        }
        else {
            $('#PersonalInfo_AddressPast').val('');
            $('#PersonalInfo_PinCodePast').val('');
            $('#PersonalInfo_StatePast').empty();
            $("#PersonalInfo_StatePast").append('<option value="">--Select State--</option>');
            $("#CityListPast").empty();
            $("#CityListPast").append('<option value="">--Select City--</option>');
        }
        $('#asAboveCheck').val(this.checked);
    });
    
    $("#PersonalInfo_StatePresent").change(function () {
        $('#pre-loader').show();
        $("#PersonalInfo_CityPresent").empty();
        $("#PersonalInfo_CityPresent").append('<option value="">--Select City--</option>');
        var sId = $("select#PersonalInfo_StatePresent").val();
        if (sId !== "") {
            $.ajax({
                type: 'GET',
                url: '/Home/GetCity',
                dataType: 'json',
                data: { id: $("select#PersonalInfo_StatePresent").val() },
                success: function (result) {
                    //$('select option[value="1"]').attr("selected", true);
                    $.each(result, function (i, city) {
                        $("#PersonalInfo_CityPresent").append('<option value="' + city.CityId + '">' +
                            city.CityName + '</option>');
                    });
                    $('#pre-loader').hide();
                },
                error: function (ex) {
                    alert('Failed to retrieve states city.' + ex);
                    $('#pre-loader').hide();
                }
            });
        }

    });

    $("#PersonalInfo_StatePast").change(function () {
        $('#pre-loader').show();
        $("#CityListPast").empty();
        $("#CityListPast").append('<option value="">--Select City--</option>');
        var sId = $("select#PersonalInfo_StatePast").val();
        if (sId !== "") {
            $.ajax({
                type: 'GET',
                url: '/Home/GetCity',
                dataType: 'json',
                data: { id: $("select#PersonalInfo_StatePast").val() },
                success: function (result) {
                    $.each(result, function (i, city) {
                        $("#CityListPast").append('<option value="' + city.CityId + '">' +
                            city.CityName + '</option>');
                    });
                    $('#pre-loader').hide();
                },
                error: function (error) {
                    //toastr.error(responce.Data);
                    alert('Failed to retrieve states city.' + ex);
                    $('#pre-loader').hide();
                }
            });
        }
    });

    //GetDesignation	
    $("#PersonalInfo_AppliedForDepartment").change(function () {
        $('#pre-loader').show();
        $("#PersonalInfo_AppliedForDesignation").empty();
        $("#PersonalInfo_AppliedForDesignation").append('<option value="">--Select Designation--</option>');
        if ($("select#PersonalInfo_AppliedForDepartment").val() !== "") {
            $.ajax({
                type: 'GET',
                url: '/Home/GetDesignation',
                dataType: 'json',
                data: { id: $("select#PersonalInfo_AppliedForDepartment").val() },
                success: function (designations) {
                    $.each(designations, function (i, designation) {
                        $("#PersonalInfo_AppliedForDesignation").append('<option value="' + designation.DesignationId + '">' +
                            designation.DesignationName + '</option>');
                    });
                    $('#pre-loader').hide();
                },
                error: function (ex) {
                    alert('Failed to retrieve designationsList.' + ex);
                    $('#pre-loader').hide();
                }
            });
        }

    });

    function setAge(d) {
        return moment().diff(d, 'years', true);
    }

});
var addSerialNumber = function () {
    $('table tr').each(function (index) {
        $(this).find('td:nth-child(1)').html(index + 1);
    });
};

$('#PersonalInfo_ReferenceName').blur(function () {
    if ($("input[name='PersonalInfo.IsReference']:checked").val() == 'True') {
        if ($('#PersonalInfo_ReferenceName').val() == "") {
            document.getElementById("errRefName").innerHTML = "Please Enter Reference Name";
        }
        else { document.getElementById("errRefName").innerHTML = ""; }
    }
});

$('#PersonalInfo_ReferenceDesignation').blur(function () {
    if ($("input[name='PersonalInfo.IsReference']:checked").val() == 'True') {
        if ($('#PersonalInfo_ReferenceDesignation').val() == "") {
            document.getElementById("errRefDesignation").innerHTML = "Please Enter Reference Designation";
        }
        else { document.getElementById("errRefDesignation").innerHTML = ""; }
    }
});

$('#PersonalInfo_ReferenceMobileNo').blur(function () {
    if ($("input[name='PersonalInfo.IsReference']:checked").val() == 'True') {
        if ($('#PersonalInfo_ReferenceName').val() == "") {
            document.getElementById("errRefMobileNo").innerHTML = "Please Enter Reference Mobile No";
        }
        else { document.getElementById("errRefMobileNo").innerHTML = ""; }
    }
});

$('button.validateAll').click(function () {
    $('form').each(function () {
        $(this).valid();
    });
});

function SaveAll() {
    if ($('#frmdetail').valid() && counter2 != 2 && counter1 != 2) {
        $('#pre-loader').show();
        var objj = {
            PersonalInfo: {},
            PreviousEmploymentDetail: [],
            Reference: [],
            EducationBackground: [],
            Languages: []
        };

        objj.PersonalInfo = {
            FirstName: $("#PersonalInfo_FirstName").val(),
            LastName: $("#PersonalInfo_LastName").val(),
            MobileNo1: $("#PersonalInfo_MobileNo1").val(),
            MobileNo2: $("#PersonalInfo_MobileNo2").val(),
            EmailId: $("#PersonalInfo_EmailId").val(),
            DateOfBirth: $("#PersonalInfo_DateOfBirth").val(),
            Age: $("#Age").text(),
            Gender: $("input[name='PersonalInfo.Gender']:checked").val(),
            MaritalStaus: $("input[name='PersonalInfo.MaritalStaus']:checked").val(),
            NoOfChildren: $("#PersonalInfo_NoOfChildren").val(),
            AddressPresent: $("#PersonalInfo_AddressPresent").val(),
            StatePresent: $("#PersonalInfo_StatePresent option:selected").text(),
            CityPresent: $("#PersonalInfo_CityPresent option:selected").text(),
            PincodePresent: $("#PersonalInfo_PincodePresent").val(),
            AddressPast: $("#PersonalInfo_AddressPast").val(),
            StatePast: $("#PersonalInfo_StatePast option:selected").text(),
            CityPast: $("#CityListPast option:selected").text(),
            PinCodePast: $("#PersonalInfo_PinCodePast").val(),
            AppliedForDepartment: $("#PersonalInfo_AppliedForDepartment option:selected").text(),
            AppliedForDesignation: $("#PersonalInfo_AppliedForDesignation option:selected").text(),
            TotalExperienceInYear: $("#PersonalInfo_TotalExperienceInYear").val(),
            EarliestJoinDate: $("#PersonalInfo_EarliestJoinDate").val(),
            SalaryExpectation: $("#PersonalInfo_SalaryExpectation").val(),
            Vehicle: $("input[name='PersonalInfo.Vehicle']:checked").val(),
            JobSource: $("input[name='PersonalInfo.JobSource']:checked").val(),
            NightShift: $("input[name='PersonalInfo.NightShift']:checked").val(),
            IsReference: $("input[name='PersonalInfo.IsReference']:checked").val(),
            ReferenceName: $("#PersonalInfo_ReferenceName").val(),
            ReferenceMobileNo: $("#PersonalInfo_ReferenceMobileNo").val(),
            ReferenceDesignation: $("#PersonalInfo_ReferenceDesignation").val(),
            OtherCertification: $("#PersonalInfo_OtherCertification").val()
        };

        $("#previousEmploymentDetails tbody tr").each(function () {
            var row = $(this);
            objj.PreviousEmploymentDetail.push({
                CompanyName: row.find("td:eq(1)").text(),
                City: row.find("td:eq(2)").text(),
                Designation: row.find("td:eq(3)").text(),
                WorkFrom: row.find("td:eq(4)").text(),
                WorkTo: row.find("td:eq(5)").text(),
                CtcMonth: row.find("td:eq(6)").text()
            });
        });

        $("#reference tbody tr").each(function () {
            var row = $(this);
            objj.Reference.push({
                PersonName: row.find("td:eq(1)").text(),
                CompanyName: row.find("td:eq(2)").text(),
                Designation: row.find("td:eq(3)").text(),
                ContactNo: row.find("td:eq(4)").text(),
            });
        });
        $("#educationalBackground tbody tr").each(function () {
            var row = $(this);
            objj.EducationBackground.push({
                BoardUniversityName: row.find("td:eq(1)").text(),
                CourseDegreeName: row.find("td:eq(2)").text(),
                PassingYear: row.find("td:eq(3)").text(),
                GradePercentage: row.find("td:eq(4)").text(),
            });
        });

        var countLanguage = 0;

        $("#language tbody tr").each(function () {
            var row = $(this);
            if (row.find("td:eq(0)").text() != "") {
                objj.Languages.push({
                    LanguageType: row.find("td:eq(0)").text(),
                    Read: row.find("td:eq(1)").text(),
                    Speak: row.find("td:eq(2)").text(),
                    Write: row.find("td:eq(3)").text()
                });
            }

            countLanguage = countLanguage + 1;
        });

        if (countLanguage == 0) {
            toastr.error("At Least One Language Known Required");
            $('#pre-loader').hide();
            $('#Language').focus();
        }
        else {
            $.ajax({
                type: "POST",
                url: "/Home/SavePreInterView",
                data: JSON.stringify(objj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (responce) {
                    if (responce.ContentType == "success") {
                        $("#myModal").modal('show');
                        $("#frmdetail").each(function () { this.reset() });
                        $("#frmpredetail").each(function () { this.reset() });
                        $("#frmReference").each(function () { this.reset() });
                        $("#frmEducationalBackground").each(function () { this.reset() });
                        $("#PersonalInfo_OtherCertification").val('');
                        $('#pre-loader').hide();
                    } else {
                        toastr.warning(responce.Data);
                        $('#pre-loader').hide();
                    }
                },
                error: function () {
                    toastr.warning(responce.Data);
                    $('#pre-loader').hide();
                }
            });
        }
    }
    else {
        if (counter2 == 2) {
            var eduErr = document.getElementById("errEdumsg");
            eduErr.style.display = "block"
        }
        if (counter1 == 2) {
            var eduErr = document.getElementById("errRefmsg");
            eduErr.style.display = "block"
        }
        $('#frmdetail').find("input.input-validation-error")[0].focus();
    }
}





function SaveAllwalkin() {
    if ($('#frmdetail').valid() && counter2 != 2 && counter1 != 2) {
        $('#pre-loader').show();
        var objj = {
            PersonalInfo: {},
            PreviousEmploymentDetail: [],
            Reference: [],
            EducationBackground: [],
            Languages: []
        };

        objj.PersonalInfo = {
            FirstName: $("#PersonalInfo_FirstName").val(),
            LastName: $("#PersonalInfo_LastName").val(),
            MobileNo1: $("#PersonalInfo_MobileNo1").val(),
            MobileNo2: $("#PersonalInfo_MobileNo2").val(),
            EmailId: $("#PersonalInfo_EmailId").val(),
            DateOfBirth: $("#PersonalInfo_DateOfBirth").val(),
            Age: $("#Age").text(),
            Gender: $("input[name='PersonalInfo.Gender']:checked").val(),
            MaritalStaus: $("input[name='PersonalInfo.MaritalStaus']:checked").val(),
            NoOfChildren: $("#PersonalInfo_NoOfChildren").val(),
            AddressPresent: $("#PersonalInfo_AddressPresent").val(),
            StatePresent: $("#PersonalInfo_StatePresent option:selected").text(),
            CityPresent: $("#PersonalInfo_CityPresent option:selected").text(),
            PincodePresent: $("#PersonalInfo_PincodePresent").val(),
            AddressPast: $("#PersonalInfo_AddressPast").val(),
            StatePast: $("#PersonalInfo_StatePast option:selected").text(),
            CityPast: $("#CityListPast option:selected").text(),
            PinCodePast: $("#PersonalInfo_PinCodePast").val(),
            AppliedForDepartment: $("#PersonalInfo_AppliedForDepartment option:selected").text(),
            AppliedForDesignation: $("#PersonalInfo_AppliedForDesignation option:selected").text(),
            TotalExperienceInYear: $("#PersonalInfo_TotalExperienceInYear").val(),
            EarliestJoinDate: $("#PersonalInfo_EarliestJoinDate").val(),
            SalaryExpectation: $("#PersonalInfo_SalaryExpectation").val(),
            Vehicle: $("input[name='PersonalInfo.Vehicle']:checked").val(),
            JobSource: $("input[name='PersonalInfo.JobSource']:checked").val(),
            NightShift: $("input[name='PersonalInfo.NightShift']:checked").val(),
            IsReference: $("input[name='PersonalInfo.IsReference']:checked").val(),
            ReferenceName: $("#PersonalInfo_ReferenceName").val(),
            ReferenceMobileNo: $("#PersonalInfo_ReferenceMobileNo").val(),
            ReferenceDesignation: $("#PersonalInfo_ReferenceDesignation").val(),
            OtherCertification: $("#PersonalInfo_OtherCertification").val()
        };

        $("#previousEmploymentDetails tbody tr").each(function () {
            var row = $(this);
            objj.PreviousEmploymentDetail.push({
                CompanyName: row.find("td:eq(1)").text(),
                City: row.find("td:eq(2)").text(),
                Designation: row.find("td:eq(3)").text(),
                WorkFrom: row.find("td:eq(4)").text(),
                WorkTo: row.find("td:eq(5)").text(),
                CtcMonth: row.find("td:eq(6)").text()
            });
        });

        $("#reference tbody tr").each(function () {
            var row = $(this);
            objj.Reference.push({
                PersonName: row.find("td:eq(1)").text(),
                CompanyName: row.find("td:eq(2)").text(),
                Designation: row.find("td:eq(3)").text(),
                ContactNo: row.find("td:eq(4)").text(),
            });
        });
        $("#educationalBackground tbody tr").each(function () {
            var row = $(this);
            objj.EducationBackground.push({
                BoardUniversityName: row.find("td:eq(1)").text(),
                CourseDegreeName: row.find("td:eq(2)").text(),
                PassingYear: row.find("td:eq(3)").text(),
                GradePercentage: row.find("td:eq(4)").text(),
            });
        });

        var countLanguage = 0;

        $("#language tbody tr").each(function () {
            var row = $(this);
            if (row.find("td:eq(0)").text() != "") {
                objj.Languages.push({
                    LanguageType: row.find("td:eq(0)").text(),
                    Read: row.find("td:eq(1)").text(),
                    Speak: row.find("td:eq(2)").text(),
                    Write: row.find("td:eq(3)").text()
                });
            }

            countLanguage = countLanguage + 1;
        });

        if (countLanguage == 0) {
            toastr.error("At Least One Language Known Required");
            $('#pre-loader').hide();
            $('#Language').focus();
        }
        else {
            $.ajax({
                type: "POST",
                url: "/Home/SavePreInterViewforWalkin",
                data: JSON.stringify(objj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (responce) {
                    if (responce.ContentType == "success") {
                        $("#myModal").modal('show');
                        $("#frmdetail").each(function () { this.reset() });
                        $("#frmpredetail").each(function () { this.reset() });
                        $("#frmReference").each(function () { this.reset() });
                        $("#frmEducationalBackground").each(function () { this.reset() });
                        $("#PersonalInfo_OtherCertification").val('');
                        $('#pre-loader').hide();
                    } else {
                        toastr.warning(responce.Data);
                        $('#pre-loader').hide();
                    }
                },
                error: function () {
                    toastr.warning(responce.Data);
                    $('#pre-loader').hide();
                }
            });
        }
    }
    else {
        if (counter2 == 2) {
            var eduErr = document.getElementById("errEdumsg");
            eduErr.style.display = "block"
        }
        if (counter1 == 2) {
            var eduErr = document.getElementById("errRefmsg");
            eduErr.style.display = "block"
        }
        $('#frmdetail').find("input.input-validation-error")[0].focus();
    }
}
function canadd() {
    var is = true;
    $("#language tbody tr").each(function () {
        if ($('#Language option:selected').text() == $(this).find("td:eq(0)").text()) {
            is = false;
            return is;
        }
    });
    return is;
}