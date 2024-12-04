import axios from 'axios'

// ** Config
import generalConfig from 'src/configs/general'
import { AuthType } from 'src/context/types'

const unauthorizedCode = [401]

const baseService = axios.create({
  baseURL: generalConfig.baseUrl,
  timeout: generalConfig.timeout
})

export const headersConfig = (isMultiPart?: boolean) => {
  const authString = window.localStorage.getItem(generalConfig.authTokenKeyName)
  const auth = JSON.parse(authString || '{}') as AuthType
  let config

  if (isMultiPart) {
    config = {
      headers: {
        Authorization: `Bearer ${auth.token}`,
        'Content-Type': 'multipart/form-data'
      }
    }
  } else {
    config = {
      headers: {
        Authorization: `Bearer ${auth.token}`
      }
    }
  }

  return config
}

baseService.interceptors.request.use(
  config => {
    const authString = window.localStorage.getItem(generalConfig.authTokenKeyName)
    const auth = JSON.parse(authString || '{}') as AuthType

    const accessToken = auth.token

    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`
    }

    return config
  },
  error => {
    return Promise.reject(error)
  }
)

baseService.interceptors.response.use(
  response => response,
  error => {
    const { response } = error

    if (response && unauthorizedCode.includes(response.status)) {
      window.localStorage.removeItem('userData')
      window.localStorage.removeItem(generalConfig.authTokenKeyName)
    }

    return Promise.reject(error)
  }
)

export default baseService
