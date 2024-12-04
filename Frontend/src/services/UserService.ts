import { ApiResponse } from 'src/types/responseTypes'
import apiService from './ApiService'
import { AddUserRequest, DeleteUserRequest, EditUserRequest, ChangePasswordWithTokenRequest, ResetUserRequest, ResetLockoutRequest } from 'src/types/requestTypes'
import toast from 'react-hot-toast'

export async function apiAddUser (data: AddUserRequest): Promise<boolean> {
    return apiService.request<ApiResponse<string>, any>({
        url: '/api/User/addUser',
        method: 'post',
        data: data
    }).then(res => {
      toast.success(res.data?.message ?? (res.data?.succeeded ? 'User added successfully' : 'Failed to add user'))
      return res.data?.succeeded ?? false
    })
    .catch(err => {
      toast.error(err.response?.data?.message ?? "An error occured.")
      return false
    })
}

export async function apiEditUser (data: EditUserRequest) {
    return apiService.request<ApiResponse<any>, any>({
        url: '/api/User/editUser',
        method: 'post',
        data: data
    }).then(res => {
      toast.success(res.data?.message ?? (res.data?.succeeded ? 'User edited successfully' : 'Failed to edit user'))
    })
    .catch(err => {
      toast.error(err.response?.data?.message ?? "An error occured.")
    })
}

export async function apiDeleteUser (data: DeleteUserRequest): Promise<boolean> {
    return apiService.request<ApiResponse<any>, any>({
        url: '/api/User/deleteUser',
        method: 'post',
        data: data
    }).then(res => {
      toast.success(res.data?.message ?? (res.data?.succeeded ? 'User deleted successfully' : 'Failed to delete user'))
      return res.data?.succeeded ?? false
    })
    .catch(err => {
      toast.error(err.response?.data?.message ?? "An error occured.")
      return false
    })
}

export async function apiResetLockout (data: ResetLockoutRequest): Promise<boolean> {
    return apiService.request<ApiResponse<any>, any>({
        url: '/api/User/resetUserLockout',
        method: 'post',
        data: data
    }).then(res => {
      toast.success(res.data?.message ?? (res.data?.succeeded ? 'Lockout reset successful' : 'Failed to reset lockout'))
      return res.data?.succeeded ?? false
    })
    .catch(err => {
      toast.error(err.response?.data?.message ?? "An error occured.")
      return false
    })
}

export async function apiPasswordResetRequest (data: ResetUserRequest) {
    return apiService.request<ApiResponse<any>, any>({
        url: '/api/User/resetUser',
        method: 'post',
        data
    }).then(res => {
      toast.success(res.data?.message ?? (res.data?.succeeded ? 'Password reset request successful' : 'Failed to request reset password'))
    })
    .catch(err => {
      toast.error(err.response?.data?.message ?? "An error occured.")
    })
}

export async function apiChangePasswordWithToken (data: ChangePasswordWithTokenRequest): Promise<boolean> {
    return apiService.request<ApiResponse<any>, any>({
        url: '/api/User/changePasswordWithToken',
        method: 'post',
        data
    }).then(res => {
      toast.success(res.data?.message ?? (res.data?.succeeded ? 'Password reset successful' : 'Failed to reset password'))
      return true
    })
    .catch(err => {
      toast.error(err.response?.data?.message ?? "An error occured.")
      return false
    })
}
