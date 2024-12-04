// ** React Imports
import { createContext, useState, useEffect, ReactNode } from 'react'

// ** Next Import
import { useRouter } from 'next/router'

// ** Config
import generalConfig from 'src/configs/general'

// ** Types
import { AuthValuesType, LoginParams, ErrCallbackType, UserDataType, AuthType } from './types'
import apiService from 'src/services/ApiService'
import { addSeconds } from 'date-fns'
import { ApiResponse } from 'src/types/responseTypes'

// ** Defaults
const defaultProvider: AuthValuesType = {
  user: null,
  loading: true,
  token: '',
  setUser: () => null,
  setLoading: () => Boolean,
  login: () => Promise.resolve(),
  logout: () => Promise.resolve()
}

const AuthContext = createContext(defaultProvider)

type Props = {
  children: ReactNode
}

const AuthProvider = ({ children }: Props) => {
  // ** States
  const [user, setUser] = useState<UserDataType | null>(defaultProvider.user)
  const [token, setToken] = useState<string>(defaultProvider.token)
  const [loading, setLoading] = useState<boolean>(defaultProvider.loading)

  // ** Hooks
  const router = useRouter()

  useEffect(() => {
    const initAuth = async (): Promise<void> => {
      const authString = window.localStorage.getItem(generalConfig.authUserKeyName)
      if (authString) {
        setLoading(true)
        const authUser: UserDataType = JSON.parse(authString)
        setUser(authUser)
        setToken(authUser.jwToken)
        setLoading(false)
      } else {
        setLoading(false)
      }
    }
    initAuth()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  const handleLogin = async (request: LoginParams, errorCallback?: ErrCallbackType) => {
    await apiService
      .request<ApiResponse<UserDataType>, LoginParams>({
        method: 'POST',
        url: generalConfig.loginEndpoint,
        data: request
      })
      .then(async response => {
        const auth: AuthType = {
          token: response.data.data.jwToken,
          expiresAt: addSeconds(new Date(), response.data.data.expiresIn)
        }
        window.localStorage.setItem(generalConfig.authTokenKeyName, JSON.stringify(auth))
        const returnUrl = router.query.returnUrl

        setUser({ ...response.data.data })
        setToken(response.data.data.jwToken)
        window.localStorage.setItem('userData', JSON.stringify(response.data.data))

        const redirectURL = returnUrl && returnUrl !== '/' ? returnUrl : '/'

        router.replace(redirectURL as string)
      })
      .catch(err => {
        if (errorCallback) errorCallback(err)
      })
  }

  const handleLogout = () => {
    setUser(null)
    window.localStorage.removeItem('userData')
    window.localStorage.removeItem(generalConfig.authTokenKeyName)
    router.push('/login')
  }

  const values = {
    user,
    loading,
    token,
    setUser,
    setLoading,
    login: handleLogin,
    logout: handleLogout
  }

  return <AuthContext.Provider value={values}>{children}</AuthContext.Provider>
}

export { AuthContext, AuthProvider }
