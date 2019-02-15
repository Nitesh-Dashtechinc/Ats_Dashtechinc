$(document).ready(function () {
    var counter = 2;
    var counter1 = 2;
    var counter2 = 2;

    document.getElementById("PersonalInfo_NoOfChildren").value = 0;
    $('.divrefernce').hide();

    $('.only-number').keypress(function (event) {
        debugger;
        var keycode = event.which;
        if (!(event.shiftKey == false && (keycode == 46 || keycode == 8 || keycode == 37 || keycode == 39 || (keycode >= 48 && keycode <= 57)))) {
            event.preventDefault();
        }
    });
    //$('#PersonalInfo_EmailId').on('keypress', function () {
    //    var re = /([A-Z0-9a-z_-][^@])+?@[^$#<>?]+?\.[\w]{2,4}/.test(this.value);
    //    if (!re) {
    //        $('#errorEmail').show();
    //    } else {
    //        $('#errorEmail').hide();
    //    }
    //});


    //All DatePicker
    //ref:https://bootstrap-datepicker.readthedocs.io/en/latest/
    $('#PersonalInfo_DateOfBirth').datepicker({
        format: 'mm/dd/yyyy',
        endDate: new Date(),
        //todayHighlight: true,
        clearBtn: true,
        autoclose: true
    }).on('change', function (e) {
        var birthDate = $(this).val()
        var age = calculate_age(new Date(birthDate));
        if (age !== null) {
            document.getElementById("Age").innerHTML = age;
        }
    });

    $('#PersonalInfo_EarliestJoinDate').datepicker({
        format: 'mm/dd/yyyy',
        startDate: new Date(),
        clearBtn: true,
        autoclose: true
    });

    $('#WorkFrom').datepicker({
        format: 'dd/mm/yyyy',
        datesDisabled: new Date(),
        endDate: new Date(),
        clearBtn: true,
        autoclose: true
    }).on('change', function () {
        $("#WorkFrom").valid();
    });


    $('#WorkTo').datepicker({
        format: 'dd/mm/yyyy',
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
        debugger;
        switch (tableId) {
            case "previousEmploymentDetails":
                if ($('#frmpredetail').valid()) {
                    cols += '<td><label name="EmploymentId" id="EmploymentId" value=' + counter + '>' + counter + '</label><input type="hidden" class="form-control" id="CandidateId" name="CandidateId' + counter + '"/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" id="CompanyName" value=' + $('#CompanyName').val() + '  name="CompanyName' + counter + '" required/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" id="City" value=' + $('#City').val() + ' name="City' + counter + '" required/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" id="Designation" value=' + $('#Designation').val() + '  name="Designation' + counter + '" required/></td>';
                    cols += '<td><div style="display:inline-flex"><input type="text" value=' + $('#WorkFrom').val() + ' class="form-control datepicker minwidth" id="WorkFrom" name="WorkFrom' + counter + '"readonly required/><span class="input-group-text"><i class="fas fa-calendar-week"></i></span></div></td>';
                    cols += '<td><div style="display:inline-flex"><input type="text" value=' + $('#WorkTo').val() + ' class="form-control datepicker minwidth" id="WorkTo" name="WorkTo' + counter + '" readonly required/><span class="input-group-text"><i class="fas fa-calendar-week"></i></span></div></td>';
                    cols += '<td><input type="text"  class="form-control only-number minwidth" value=' + $('#CtcMonth').val() + '  id="CtcMonth" name="CtcMonth' + counter + '" required/></td>';
                    cols += '<td><button type="button" class="btn btn-md btn-danger fa fa-trash ibtn-Del"></button></td>';
                    newRow.append(cols);
                    $("table.table-pre-employeement").append(newRow);
                    $('.datepicker').datepicker({
                        format: 'dd/mm/yyyy'
                    });
                    var preEmpDetail = {
                        CompanyName: $('#CompanyName').val(),
                        City: $('#City').val(),
                        Designation: $('#Designation').val(),
                        WorkFrom: $('#WorkFrom').val(),
                        WorkTo: $('#WorkTo').val(),
                        CtcMonth: $('#CtcMonth').val(),
                    }
                    //var preEmpDetail = [];
                    //    preEmpDetail[0] = $('#CompanyName').val(),
                    //    preEmpDetail[1] = $('#City').val(),
                    //    preEmpDetail[2] = $('#Designation').val(),
                    //    preEmpDetail[3]=  $('#WorkFrom').val(),
                    //    preEmpDetail[4]=  $('#WorkTo').val(),
                    //    preEmpDetail[5]=  $('#CtcMonth').val(),
                    //localStorage.setItem("previousEmpDetail", JSON.stringify(preEmpDetail));
                    counter++;
                    $(this).closest('tr').find('#CompanyName').val('');
                    $(this).closest('tr').find('#City').val('');
                    $(this).closest('tr').find('#Designation').val('');
                    $(this).closest('tr').find('#WorkFrom').val('');
                    $(this).closest('tr').find('#WorkTo').val('');
                    $(this).closest('tr').find('#CtcMonth').val('');
                }

                break;
            case "reference":
                if ($('#frmReference').valid()) {
                    cols += '<td><label name="ReferenceId" id="ReferenceId" value=' + counter1 + '>' + counter1 + '</label><input type="hidden" class="form-control" id="CandidateId" name="CandidateId' + counter1 + '"/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" value=' + $('#PersonName').val() + ' id="PersonName" name="PersonName' + counter1 + '" required/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" value=' + $(this).closest('tr').find('#Designation').val() + '  id="Designation" name="Designation' + counter1 + '" required/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" value=' + $(this).closest('tr').find('#CompanyName').val() + ' id="CompanyName"  name="CompanyName' + counter1 + '" required/></td>';
                    cols += '<td><input type="text" class="form-control only-number minwidth" value=' + $('#ContactNo').val() + '  maxlength="10"  id="ContactNo" name="ContactNo' + counter1 + '" required/></td>';
                    cols += '<td><button type="button" class="btn btn-md btn-danger fa fa-trash ibtn-Del"></button></td>';
                    newRow.append(cols);
                    //addSerialNumber(counter);
                    $("table.table-refernce").append(newRow);
                    counter1++;
                    var preEmpDetail = {
                        PersonName: $('#PersonName').val(),
                        Designation: $('#PersonName').val(),
                        CompanyName: $('#PersonName').val(),
                        PersonName: $('#PersonName').val(),
                        PersonName: $('#PersonName').val(),
                        PersonName: $('#PersonName').val(),
                    }

                    $(this).closest('tr').find('#PersonName').val('');
                    $(this).closest('tr').find('#Designation').val('');
                    $(this).closest('tr').find('#CompanyName').val('');
                    $(this).closest('tr').find('#ContactNo').val('');
                }

                break;
            case "educationalBackground":
                if ($('#frmEducationalBackground').valid()) {
                    cols += '<td><label name="EducationalId" id="EducationalId" value=' + counter2 + '>' + counter2 + '</label><input type="hidden" class="form-control" id="CandidateId" name="CandidateId' + counter2 + '"/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" value=' + $('#BoardUniversityName').val() + '  id="BoardUniversityName" name="BoardUniversityName' + counter2 + '" required/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" value=' + $('#CourseDegreeName').val() + ' id="CourseDegreeName" name="CourseDegreeName' + counter2 + '" required/></td>';
                    cols += '<td><input type="text" class="form-control only-number minwidth" value=' + $('#PassingYear').val() + '  id="PassingYear" name="PassingYear' + counter2 + '" required/></td>';
                    cols += '<td><input type="text" class="form-control minwidth" value=' + $('#GradePercentage').val() + ' id="GradePercentage" name="GradePercentage' + counter2 + '" required/></td>';

                    cols += '<td><button type="button" class="btn btn-md btn-danger fa fa-trash ibtn-Del"></button></td>';
                    newRow.append(cols);
                    $("table.table-educational-background").append(newRow);
                    counter2++;
                    $(this).closest('tr').find('#BoardUniversityName').val('');
                    $(this).closest('tr').find('#CourseDegreeName').val('');
                    $(this).closest('tr').find('#PassingYear').val('');
                    $(this).closest('tr').find('#GradePercentage').val('');
                }

                break;
            default:
        }
    });    $("table.table-pre-employeement").on("click", ".ibtn-Del", function (event) {        $(this).closest("tr").remove();        counter -= 1;    });    $("table.table-refernce").on("click", ".ibtn-Del", function (event) {        $(this).closest("tr").remove();        counter -= 1;    });    $("table.table-educational-background").on("click", ".ibtn-Del", function (event) {        $(this).closest("tr").remove();        counter -= 1;    });

    $('input[type=radio][id=PersonalInfo_IsReference]').change(function () {
        if (this.value == 'Yes') {
            $('.divrefernce').show();
        }
    });

    $('input[type=radio][id=PersonalInfo_IsReference1]').change(function () {
        if (this.value == 'No') {
            $('.divrefernce').hide();
        }
    });


    $("#PersonalInfo_StatePresent").change(function () {        $("#PersonalInfo_CityPresent").empty();        $("#PersonalInfo_CityPresent").append('<option value="">--Select City--</option>');        var sId = $("select#PersonalInfo_StatePresent").val();        if (sId !== "") {            $.ajax({                type: 'GET',                url: '/Home/GetCity',                dataType: 'json',                data: { id: $("select#PersonalInfo_StatePresent").val() },                success: function (result) {                    //$('select option[value="1"]').attr("selected", true);                    $.each(result, function (i, city) {                        $("#PersonalInfo_CityPresent").append('<option value="' + city.CityId + '">' +                            city.CityName + '</option>');                    });                },                error: function (ex) {                    alert('Failed to retrieve states city.' + ex);                }            });        }    });    $("#PersonalInfo_StatePast").change(function () {        $("#CityListPast").empty();        $("#CityListPast").append('<option value="">--Select City--</option>');        var sId = $("select#PersonalInfo_StatePast").val();        if (sId !== "") {            $.ajax({                type: 'GET',                url: '/Home/GetCity',                dataType: 'json',                data: { id: $("select#PersonalInfo_StatePast").val() },                success: function (result) {                    //$('select option[value="1"]').attr("selected", true);                    $.each(result, function (i, city) {                        $("#CityListPast").append('<option value="' + city.CityId + '">' +                            city.CityName + '</option>');                    });                },                error: function (ex) {                    alert('Failed to retrieve states city.' + ex);                }            });        }        else { }    });

    //GetDesignation	
    $("#PersonalInfo_AppliedForDepartment").change(function () {
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
                },
                error: function (ex) {
                    alert('Failed to retrieve designationsList.' + ex);
                }
            });
        }

    });


    function calculate_age(dob) {
        var diff_ms = Date.now() - dob.getTime();
        var age_dt = new Date(diff_ms);

        return Math.abs(age_dt.getUTCFullYear() - 1970);
    }

});
var addSerialNumber = function () {
    $('table tr').each(function (index) {
        $(this).find('td:nth-child(1)').html(index + 1);
    });
};


function calculateRow(row) {
    var price = +row.find('input[name^="price"]').val();

}

function calculateGrandTotal() {
    var grandTotal = 0;
    $("table.order-list").find('input[name^="price"]').each(function () {
        grandTotal += +$(this).val();
    });
    $("#grandtotal").text(grandTotal.toFixed(2));
}

function SaveAll() {
    debugger;
    if ($('#frmdetail').valid()) {
        var valuesEng = [];        $("input[name='English']:checked").each(function () {            valuesEng.push($(this).val());        });        var valuesHindi = [];        $("input[name='Hindi']:checked").each(function () {            valuesHindi.push($(this).val());        });        //AddPersonalInformationItems.Hindi = valuesHindi.join(",");        var valuesGuj = [];        $("input[name='Gujarati']:checked").each(function () {            valuesGuj.push($(this).val());        });        //    AddPersonalInformationItems.Gujarati = valuesGuj.join(",");

        var objj = {
            PersonalInfo: {},
            PreviousEmploymentDetail: [],
            Reference: [],
            EducationBackground: []

        };

        objj.PersonalInfo =
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
                OtherCertification: $("#PersonalInfo_OtherCertification").val(),
                English: valuesEng.join(","),
                Hindi: valuesHindi.join(","),
                Gujarati: valuesGuj.join(",")
            };

        $("#previousEmploymentDetails TBODY TR").each(function () {
            var row = $(this);
            objj.PreviousEmploymentDetail.push({
                EmploymentId: row.find("#EmploymentId").text(),
                CandidateId: row.find("#CandidateId").val(),
                CompanyName: row.find("#CompanyName").val(),
                City: row.find("#City").val(),
                Designation: row.find("#Designation").val(),
                WorkFrom: row.find("#WorkFrom").val(),
                WorkTo: row.find("#WorkTo").val(),
                CtcMonth: row.find("#CtcMonth").val()
            });
        });

        $("#reference TBODY TR").each(function () {
            var row = $(this);
            objj.Reference.push({
                CandidateId: row.find("#CandidateId").val(),
                ReferenceId: row.find("#ReferenceId").text(),
                PersonName: row.find("#PersonName").val(),
                CompanyName: row.find("#CompanyName").val(),
                Designation: row.find("#Designation").val(),
                ContactNo: row.find("#ContactNo").val(),
            });
        });

        $("#educationalBackground TBODY TR").each(function () {
            var row = $(this);
            objj.EducationBackground.push({
                EducationalId: row.find("#EducationalId").text(),
                CandidateId: row.find("#CandidateId").val(),
                BoardUniversityName: row.find("#BoardUniversityName").val(),
                CourseDegreeName: row.find("#CourseDegreeName").val(),
                PassingYear: row.find("#PassingYear").val(),
                GradePercentage: row.find("#GradePercentage").val(),
            });
        });
        $.ajax({
            type: "POST",
            url: "/Home/SavePreInterView",
            data: JSON.stringify(objj),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (responce) {

                toastr[responce.ContentType](responce.Data);
                $("#frmdetail").each(function () { this.reset() });
                $("#frmpredetail").each(function () { this.reset() });
                $("#frmReference").each(function () { this.reset() });
                $("#frmEducationalBackground").each(function () { this.reset() });

                //alert("Your Records Saved");
            }
        });

    }
    else {
        $('#frmdetail').find("input.input-validation-error")[0].focus();
    }
}
