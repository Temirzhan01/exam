            if (string.IsNullOrEmpty(comment)) 
            {
                throw new Exception("Необходимо указать причину возврата");
            }

                        $.post("/" + base_url + "/CredManagerAsistant/Revision", data, function (partial) {
                $("#RequiredFields").html(partial);
            }).fail(function (error) { alert(error.message) });
