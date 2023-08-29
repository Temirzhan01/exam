    $("#btnGenerateStatement").on("click", function () {
        $.post("GenerateStatement", "", function (url) {
            window.open(url);
        });
        
        //window.open(urlBase + 'Base/PrintConclusion' + result["RequestNumber"] + '&taskId=' + result["PtaskId"], '_blank', '');
    });
