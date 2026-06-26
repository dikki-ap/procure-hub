import Keycloak from 'keycloak-js';

export const keycloak = new Keycloak({
  url: import.meta.env.VITE_KEYCLOAK_URL ?? 'http://localhost:9090',
  realm: import.meta.env.VITE_KEYCLOAK_REALM ?? 'procurehub',
  clientId: import.meta.env.VITE_KEYCLOAK_CLIENT_ID ?? 'procurehub-web',
});
