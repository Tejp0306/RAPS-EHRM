function disableScroll() {
    // Get the current page scroll position
    scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    scrollLeft = window.pageXOffset || document.documentElement.scrollLeft,

        // if any scroll is attempted, set this to the previous value
        window.onscroll = function () {
            window.scrollTo(scrollLeft, scrollTop);
        };
}


//function getPDF() {

//    var HTML_Width = $("#main1").width();
//    var HTML_Height = $("#main1").height();
//    var top_left_margin = 15;
//    var PDF_Width = HTML_Width + (top_left_margin * 2);
//    var PDF_Height = (PDF_Width * 1.5) + (top_left_margin * 2);
//    var canvas_image_width = HTML_Width;
//    var canvas_image_height = HTML_Height;

//    var totalPDFPages = Math.ceil(HTML_Height / PDF_Height) - 1;


//    html2canvas($("#main1")[0], { allowTaint: true }).then(function (canvas) {
//        canvas.getContext('2d');

//        console.log(canvas.height + "  " + canvas.width);


//        var imgData = canvas.toDataURL("image/png", 1.0);
//        var pdf = new jsPDF('p', 'pt', [PDF_Width, PDF_Height]);
//        pdf.addImage(imgData, 'jpg', top_left_margin, top_left_margin, canvas_image_width, canvas_image_height, '', 'FAST');


//        for (var i = 1; i <= totalPDFPages; i++) {
//            pdf.addPage(PDF_Width, PDF_Height);
//            pdf.addImage(imgData, 'jpg', top_left_margin, -(PDF_Height * i) + (top_left_margin * 4), canvas_image_width, canvas_image_height, '', 'FAST');
//        }

//        //pdf.save("HTML-Document.pdf");

//        var reqdata = btoa(pdf.output());

//        $.ajax({
//            url: "/dictionary/uploadconsent",
//            data: { data: reqdata, filename: $('#UniqueID').val() },
//            datatype: "json",
//            type: "post",
//            success: function (result) { self.close(); },
//            error: function (error) {
//                showerrormessage('unable to save on server!');
//            }
//        });

//    });
//};

let pdf = null;

async function generateCanvas(totaldivs, i, deferred) {

    let divname = totaldivs[i];

    var top_left_margin = 10;

    await html2canvas($("#" + divname)[0], {
        allowTaint: true,
        onrendered: function (canvas) {
        }
    }).then(function (canvas) {

        if (i == 0) {
            canvas.getContext('2d');
        }
        else {
            pdf.addPage();
        }
        let imgData = canvas.toDataURL('image/png');
        const imgProps = pdf.getImageProperties(imgData);
        const pdfWidth = pdf.internal.pageSize.getWidth() - (top_left_margin * 2);
        let pdfHeight = ((imgProps.height * pdfWidth) / imgProps.width);

        if (pdfHeight > pdf.internal.pageSize.getHeight()) {
            pdfHeight = (pdf.internal.pageSize.getHeight() - 5);
        }
        else {
            pdfHeight = pdfHeight - 5;
        }

        pdf.addImage(imgData, 'png', top_left_margin, top_left_margin, pdfWidth, pdfHeight, '', 'FAST');
        deferred.resolve();
    });

}

async function getPDF(totaldivs) {
    //debugger
    let ScreenId = $('#ScreenId').val();
    if (ScreenId == 2) {
        totaldivs = ['Page1', 'Page2', 'Page3', 'Page4', 'Page5'];
    }
    else if (ScreenId == 4) {
        totaldivs = ['Page7', 'Page8'];
    }
    else if (ScreenId == 1) {
        totaldivs = ['Page1', 'Page2', 'Page3', 'Page4', 'Page5', 'Page6', 'Page7'];
    }
    else {
        totaldivs = ['Page8', 'Page9'];
    }

    if (totaldivs.length > 0) {
        pdf = new jspdf.jsPDF('p', 'px', 'letter');

        var deferreds = [];

        for (let i = 0; i < totaldivs.length; i++) {
            var deferred = $.Deferred();
            deferreds.push(deferred.promise());
            await generateCanvas(totaldivs, i, deferred);
        }

        $.when.apply($, deferreds).then(function () { // executes after adding all images
            //pdf.save('test.pdf');
            var reqdata = btoa(pdf.output());

            $.ajax({
                url: "/dictionary/uploadconsent",
                data: { data: reqdata, filename: $('#UniqueID').val() },
                datatype: "json",
                type: "post",
                success: function (result) { self.close(); },
                error: function (error) {
                    showerrormessage('unable to save on server!');
                }
            });
        });
    }
};

async function getRemovalPDF() {
    pdf = new jspdf.jsPDF('p', 'px', 'letter');

    var deferreds = [];
    totaldivs = ['Page1'];
    for (let i = 0; i < totaldivs.length; i++) {
        var deferred = $.Deferred();
        deferreds.push(deferred.promise());
        await generateCanvas(totaldivs, i, deferred);
    }

    $.when.apply($, deferreds).then(function () { // executes after adding all images
        //pdf.save('test.pdf');
        var reqdata = btoa(pdf.output());

        $.ajax({
            url: "/dictionary/uploadremovalform",
            data: { data: reqdata, filename: $('#fileName').val() },
            datatype: "json",
            type: "post",
            success: function (result) { self.close(); },
            error: function (error) {
                showerrormessage('unable to save on server!');
            }
        });
    });
};

function browserprintAicure() {
    var versionDate = $("#SiteDetailsDTO_AicVersioDate").val();
    var irbNo = $("#SiteDetailsDTO_AicIRBNo").val();
    var printcontent = document.getElementById("divContendArea").innerHTML;
    document.body.innerHTML = printcontent;
    document.title = "Version Date: " + versionDate.split(' ')[0] + " / " + irbNo;
    document.date = "1";
    document.url = "";
    window.print();
}

function browserprintDisengagement() {
    var versionDate = $("#SiteDetailsDTO_DisVersioDate").val();
    var irbNo = $("#SiteDetailsDTO_DisIRBNo").val();
    var printcontent = document.getElementById("divContendArea").innerHTML;
    document.body.innerHTML = printcontent;
    document.title = "Version Date: " + versionDate.split(' ')[0] + " / " + irbNo;
    document.date = "1";
    document.url = "";
    window.print();
}