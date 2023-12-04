    function getToken() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: 'https://halykbpm-auth.halykbank.nb/win-Auth/jwt/bearer?clientId=bp-api',
                type: 'GET',
            },
                success: function (response) {
                    resolve(response)
                }
        );
    });
