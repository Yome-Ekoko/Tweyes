export type ErrCallbackType = (err: { [key: string]: any }) => void

export type LoginParams = {
  username: string
  password: string
}

export type RegisterParams = {
  email: string
  username: string
  password: string
}

export type AuthType = {
  token: string
  expiresAt: string | Date
}

export type UserDataType = {
  id: string
  userName: string
  name: string
  email: number
  isLoggedIn: boolean
  roles: string[]
  status: number
  statusMeaning: string
  lastLoginTime: Date
  jwToken: string
  expiresIn: number
  expiryDate: Date
}

export type AuthValuesType = {
  loading: boolean
  logout: () => void
  user: UserDataType | null
  token: string
  setLoading: (value: boolean) => void
  setUser: (value: UserDataType | null) => void
  login: (params: LoginParams, errorCallback?: ErrCallbackType) => void
}
