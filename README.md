const { get } = require("jquery");
тут ругается также, проверь корректен ли нижний код 

    function getToken() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: 'https://halykbpm-auth.halykbank.nb/win-Auth/jwt/bearer?clientId=bp-api',
                type: 'GET',
                success: function (response) {
                    resolve(response); // При успешном ответе разрешаем промис с ответом
                },
                error: function (xhr, status, error) {
                    reject(error); // При ошибке отклоняем промис с ошибкой
                }
            });
        });
    }

    $(document).on("click", ".SearchClientInfo", function () {
        var data = new Object();
        data.value = $(document).find(".IINBIN").val();

        var $checkboxes = $('#GesvObjects input[type="checkbox"]');
        var countCheckedCheckboxes = $checkboxes.filter(':checked').filter('.isCommisNeeded').length;

        getToken().then(token => {
            data.token = token;
            $.get("/" + base_url + "/Base/SearchClientInfo", data, function (partial) {
                if (countCheckedCheckboxes > 0) {
                    $('#comisAccBlock').block({
                        message: "<h3 style='color:red'>Нажмите кнопку Обновить</h3>"
                    });
                }

                $(document).find("#StartData").html(partial);
            });
        });
    });
