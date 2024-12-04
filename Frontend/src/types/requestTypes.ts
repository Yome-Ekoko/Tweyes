import { DateType } from "./generalTypes"

export interface AddUserRequest {
  userName: string
  name: string
  email: string
  role: string
}

export interface EditUserRequest {
  userName: string
  name: string
  email: string
  role: string
}

export interface DeleteUserRequest {
  userName: string
}

export interface ResetLockoutRequest {
  userName: string
}

export interface ResetUserRequest {
  userName: string
}

export interface ChangePasswordWithTokenRequest {
  userName: string
  token: string
  password: string
  confirmPassword: string
}

export interface DownloadLogRequest {
  startDate: DateType
  endDate: DateType
}
