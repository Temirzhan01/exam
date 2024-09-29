{
  "exp": 1727635494,
  "iat": 1727635194,
  "jti": "60a9a36e-d0ba-44cb-bc4a-b0175b39d248",
  "iss": "https://cloudkc-test.halykbank.nb/realms/dabp-realm",
  "aud": "account",
  "sub": "268221e6-7bf3-492f-b74f-cab28a562e6d",
  "typ": "Bearer",
  "azp": "TestClient",
  "acr": "1",
  "allowed-origins": [
    "/*"
  ],
  "resource_access": {
    "TestClient": {
      "roles": [
        "uma_protection"
      ]
    },
    "account": {
      "roles": [
        "manage-account",
        "manage-account-links",
        "view-profile"
      ]
    }
  },
  "scope": "profile email",
  "email_verified": false,
  "clientHost": "172.16.16.9",
  "roles": [
    "default-roles-dabp-realm",
    "offline_access",
    "uma_authorization"
  ],
  "preferred_username": "service-account-testclient",
  "clientAddress": "172.16.16.9",
  "client_id": "TestClient"
}
