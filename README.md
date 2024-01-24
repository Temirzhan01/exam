Смотри, немного не понятно выглядит второй код, клиентской стороны. Там в виде json получает клиент список ссылок, и как он их конкретно скачивает не понятно немного
Вот ниже у меня как, как ее переделать?
     
    $(function () {
            $(document).on("click", ".printIDPDF", function () {
            var data = new Object();
            var referrer = $(this).closest('.referrer');
            data.cliIde = referrer.find('.clientID').val();
            data.ViewID = @ViewID
            data.docViewId = @IDdocViewId
            data.docPackId = @IDdocPackId
            data.reqNumber = document.querySelector('.reqnum').textContent.substring(2, 14);
            data.clientId = Number(referrer.find('.clientID').val());
                $.post("@Url.Content("~/")DBCheck/GeneratePdfForFLID", data, function (url) {
                window.open(url);
            }).fail(function (error) { alert("Необходимо осуществить проверки по базам, перед тем как распечать документ") });
        });
    }); 
