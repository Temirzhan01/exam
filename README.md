    $(document).on("change", ":file", function () {
            var data = new FormData();
            var files = $(this).get(0).files;
            if (files.length > 0) {
                data.append("UploadedImage", files[0]);
            }
            data.append("guid", $(this).attr("guid"));
            data.append("contractNumber", $(".ContractNumber").html());
            var target = $(this);
            target.closest(".File").find(".uploadBlock").hide();
            target.closest(".File").find(".loading").show();
            var prefix = $(".Url").html();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: prefix + "/MSBDocumentsPocket/UploadFile",
                contentType: false,
                processData: false,
                data: data
            });
            ajaxRequest.done(function (xhr, textStatus) {
                target.closest(".File").find(".loading").hide();
                if (textStatus == "success" && xhr == "OK") {
                    target.closest(".File").find(".loading").hide();
                    target.closest(".File").find(".download").show();
                }
                else {
                    target.closest(".File").find(".loading").hide();
                    target.closest(".File").find(".uploadBlock").show();
                    alert(xhr);
                }
            });
    });
