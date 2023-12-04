function getToken() {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: 'https://halykbpm-auth.halykbank.nb/win-Auth/jwt/bearer?clientId=bp-api',
            type: 'GET',
            success: function(response) {
                resolve(response); // При успешном ответе разрешаем промис с ответом
            },
            error: function(xhr, status, error) {
                reject(error); // При ошибке отклоняем промис с ошибкой
            }
        });
    });
}
