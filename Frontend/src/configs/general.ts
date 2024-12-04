export default {
  baseUrl: process.env.NEXT_PUBLIC_APP_BASEURL,
  timeout: 30000,
  loginEndpoint: '/api/User/authenticate',
  logoutEndpoint: '/api/User/logout',
  registerEndpoint: '/jwt/register',
  authTokenKeyName: 'accessToken',
  authUserKeyName: 'userData',
  onTokenExpiration: 'logout' // logout | refreshToken
}
